using Concentus;
using Concentus.Common;
using Concentus.Enums;
using Concentus.Structs;
using NAudio.Wave;
using STUN;
using STUN.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;





namespace NAudio_Opus
{
    public partial class Form1 : Form
    {
        private WaveInEvent waveIn;
        private WaveOutEvent waveOut;
        private OpusEncoder encoder;
        private OpusDecoder decoder;
        private List<byte[]> encodedPackets = new List<byte[]>();
        private List<float[]> decodedSamples = new List<float[]>();

        private UdpClient udpSender;
        private UdpClient udpListener;
        private string targetIpAdress;
        private int targetPort;
        private int myListeningPort;
        private CancellationTokenSource cancellationTokenSource;

        private int sampleRate = 48000; // Opus standard sample rate
        private int channels = 1; // Mono audio
        private int channelCount = 1;
        private int frameSize = 960; // Opus frame size for 20ms at 48kHz
        private int bitrate = 64000; // 64 kbps bitrate for Opus
        private const int OpusFrameSizeSamples = 320;
        private List<short> pcmBuffer = new List<short>();

        private string myPublicIpAddress;
        private int myPublicPort;

        // Loopback buffer provider
        private BufferedWaveProvider bufferedWaveProvider;

        public Form1()
        {
            InitializeComponent();

            encoder = new OpusEncoder(sampleRate, channels, OpusApplication.OPUS_APPLICATION_AUDIO);
            encoder.Bitrate = bitrate;

            decoder = new OpusDecoder(sampleRate, channels);

            // Initialize loopback buffer provider
            bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(sampleRate, 16, channels));
            bufferedWaveProvider.DiscardOnBufferOverflow = true;

            // Initialize output device
            waveOut = new WaveOutEvent();
            waveOut.Init(bufferedWaveProvider);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(sampleRate, 16, channels),
                BufferMilliseconds = 20 // 20ms frame
            };
            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.StartRecording();

            // Start playback
            waveOut.Play();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (waveIn != null)
            {
                waveIn.StopRecording();
                waveIn.Dispose();
                waveIn = null;
            }
            if (waveOut != null)
            {
                waveOut.Stop();
            }
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            short[] pcm = new short[e.BytesRecorded / 2];
            Buffer.BlockCopy(e.Buffer, 0, pcm, 0, e.BytesRecorded);

            pcmBuffer.AddRange(pcm);

            while (pcmBuffer.Count >= frameSize)
            {
                short[] frame = pcmBuffer.Take(frameSize).ToArray();
                pcmBuffer.RemoveRange(0, frameSize);

                byte[] encoded = new byte[4000];
                int encodedLength = encoder.Encode(frame, 0, frameSize, encoded, 0, encoded.Length);

                byte[] packet = new byte[encodedLength];
                Array.Copy(encoded, packet, encodedLength);
                encodedPackets.Add(packet);

                short[] decodedPcm = new short[frameSize * channels];
                int decodedSamples = decoder.Decode(packet, 0, packet.Length, decodedPcm, 0, frameSize, false);

                // Loopback: write decoded PCM to output buffer
                byte[] decodedBytes = new byte[decodedSamples * 2]; // 16-bit PCM
                Buffer.BlockCopy(decodedPcm, 0, decodedBytes, 0, decodedBytes.Length);
                bufferedWaveProvider.AddSamples(decodedBytes, 0, decodedBytes.Length);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CleanupNetworkAndAudioResources();


            if (waveIn != null)
            {
                waveIn.StopRecording();
                waveIn.Dispose();
            }
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private async void StartCallButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(targetPortBox.Text, out targetPort) || string.IsNullOrWhiteSpace(targetIPBox.Text))
            {
                connectionStatus.Text = "Invalid target IP or port.";
                return;
            }

            targetIpAdress = targetIPBox.Text;

            if (!int.TryParse(myPortBox.Text, out myListeningPort))
            {
                LogToStatusUi("\nInvalid local port");
                return;
            }

            

            try
            {
                connectionStatus.Text = "Connecting...";

                // udpleri olusturma
                udpSender = new UdpClient();

                if (udpListener != null) { udpListener.Close(); udpListener = null; } // onceden acik olan varsa kapat
                udpListener = new UdpClient(myListeningPort);
                
                cancellationTokenSource = new CancellationTokenSource();
                Task.Run(() => ListenForIncomingVoiceAsync(cancellationTokenSource.Token));  ///// method sonradan declare edilecek

                InitializeAudioResources();

                waveIn.StartRecording();
                waveOut.Play();

                byte[] punchPacket = System.Text.Encoding.UTF8.GetBytes("PUNCH");
                for (int i = 0; i < 5; i++)
                {
                    await udpSender.SendAsync(punchPacket, punchPacket.Length, targetIpAdress, targetPort);
                    await Task.Delay(100); // Küçük bir gecikme
                }

                StartCallButton.Enabled = false;
                StopCallButton.Enabled = true;
                connectionStatus.Text = $"Target: {targetIpAdress}:{targetPort}, Listener: {myListeningPort}";

            }
            catch (SocketException ex)
            {
                connectionStatus.Text = $"Port might be in another use";
                CleanupNetworkAndAudioResources();
            }
            catch (Exception ex)
            {
                connectionStatus.Text = $"Error: {ex.Message}";
                CleanupNetworkAndAudioResources();
            }




        }

        private void InitializeAudioResources()
        {
            waveIn = new WaveInEvent
            {
                DeviceNumber = 0, // Default device
                WaveFormat = new WaveFormat(sampleRate, 16, channels),
                BufferMilliseconds = 40 // 40ms frame
            };
            waveIn.DataAvailable += WaveIn_DataAvailable;

            OpusEncoder encoder = new OpusEncoder(sampleRate, channels, OpusApplication.OPUS_APPLICATION_AUDIO);
            encoder.Bitrate = 32000;
            OpusDecoder decoder = new OpusDecoder(sampleRate, channels);


            waveOut = new WaveOutEvent { DeviceNumber = 0 };
            bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(sampleRate, 16, channels))
            {
                BufferDuration = TimeSpan.FromMilliseconds(100), // 0.1 second buffer
                DiscardOnBufferOverflow = true
            };
            waveOut.Init(bufferedWaveProvider);

        }


        private async void WaveIn_DataAvailable_SendOverNetwork(object sender, WaveInEventArgs e)
        {
            if (encoder == null || udpSender == null || string.IsNullOrEmpty(targetIpAdress) || targetPort == 0)
            {
                return;
            }

            try
            {
                short[] pcmSamples = new short[e.BytesRecorded / 2];
                Buffer.BlockCopy(e.Buffer, 0, pcmSamples, 0, e.BytesRecorded);

                int pcmOffset = 0;
                while (pcmOffset + OpusFrameSizeSamples <= pcmSamples.Length)
                {
                    short[] frameToEncode = new short[OpusFrameSizeSamples];
                    Array.Copy(pcmSamples, pcmOffset, frameToEncode, 0, OpusFrameSizeSamples);

                    byte[] encodedData = new byte[OpusFrameSizeSamples * 2]; // Max buffer for safety
                    int encodedLength = encoder.Encode(frameToEncode, 0, OpusFrameSizeSamples, encodedData, 0, encodedData.Length);

                    if (encodedLength > 0)
                    {
                        // Sıkıştırılmış veriyi UDP ile hedefe gönder
                        await udpSender.SendAsync(encodedData, encodedLength, targetIpAdress, targetPort);
                    }
                    pcmOffset += OpusFrameSizeSamples;
                }
            }
            catch (Exception ex)
            {
                if (connectionStatus.InvokeRequired) // Kontrol farklı bir thread'den mi erişiliyor?
                {
                    connectionStatus.Invoke((Action)(() =>
                    {
                        connectionStatus.Text = $"Gönderme hatası: {ex.Message}";
                    }));
                }
                else // Zaten UI thread'indeyse doğrudan güncelle
                {
                    connectionStatus.Text = $"Gönderme hatası: {ex.Message}";
                }
            }
        }


        private async Task ListenForIncomingVoiceAsync(CancellationToken token)
        {
            if (udpListener == null || decoder == null || bufferedWaveProvider == null)
            {
                LogToStatusUi("Dinleme başlatılamadı: Gerekli nesneler null.");
                return;
            }

            LogToStatusUi($"\n{myListeningPort} portu dinleniyor..."); // UI güncellemesi için yardımcı metot

            try
            {
                // CancellationToken tetiklendiğinde UdpClient'ı kapatmak için bir geri arama kaydet.
                // Bu, ReceiveAsync'in bir istisna ile sonlanmasını sağlar.
                // 'using' bloğu, token dispose edildiğinde kaydın kaldırılmasını sağlar.
                using (token.Register(() => udpListener?.Close())) // udpListener null değilse kapat
                {
                    while (!token.IsCancellationRequested)
                    {
                        // Parametresiz ReceiveAsync'i çağır
                        UdpReceiveResult receivedResult = await udpListener.ReceiveAsync(); // CancellationToken BURADA YOK

                        // Artık token.IsCancellationRequested kontrolüne gerek yok çünkü
                        // iptal durumunda Close() çağrılacak ve yukarıdaki satır istisna fırlatacak.

                        byte[] receivedBytes = receivedResult.Buffer;

                        if (receivedBytes.Length > 0)
                        {
                            // Gelen sıkıştırılmış veriyi Opus ile aç
                            short[] decodedPcm = new short[OpusFrameSizeSamples * channels * 2]; // Max buffer
                            int decodedLengthShorts = 0;
                            try
                            {
                                decodedLengthShorts = decoder.Decode(receivedBytes, 0, receivedBytes.Length, decodedPcm, 0, OpusFrameSizeSamples, false);
                            }
                            catch (Exception decodeEx)
                            {
                                System.Diagnostics.Debug.WriteLine($"Opus Decode Hatası: {decodeEx.Message}");
                                continue; // Bu paketi atla
                            }


                            if (decodedLengthShorts > 0)
                            {
                                byte[] bytesToPlay = new byte[decodedLengthShorts * 2];
                                Buffer.BlockCopy(decodedPcm, 0, bytesToPlay, 0, bytesToPlay.Length);

                                // Açılan sesi oynatma kuyruğuna ekle
                                bufferedWaveProvider.AddSamples(bytesToPlay, 0, bytesToPlay.Length);
                            }
                        }
                    }
                }
            }
            catch (ObjectDisposedException) when (token.IsCancellationRequested)
            {
                // UdpClient, token iptali nedeniyle kapatıldı. Bu beklenen bir durum.
                LogToStatusUi("\nDinleme iptal edildi (ObjectDisposed).");
            }
            catch (SocketException ex) when (token.IsCancellationRequested &&
                                             (ex.SocketErrorCode == SocketError.Interrupted ||
                                              ex.SocketErrorCode == SocketError.OperationAborted)) // ERROR_OPERATION_ABORTED (995)
            {
                // UdpClient, token iptali nedeniyle kapatıldı ve SocketException fırlattı.
                // Interrupted genellikle bekleyen bir engelleme çağrısının (blocking call)
                // WSAEINTR ile sonlandırılması anlamına gelir, ki bu Close() ile olabilir.
                LogToStatusUi("\nDinleme iptal edildi (SocketException - Interrupted/Aborted).");
            }
            catch (Exception ex)
            {
                // Diğer beklenmedik hatalar
                LogToStatusUi($"\nAlma hatası: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Alma hatası: {ex.Message}");
            }
            finally
            {
                LogToStatusUi("\nDinleme döngüsü sonlandı.");
            }
        }

        // UI güncellemelerini güvenli bir şekilde yapmak için yardımcı bir metot (Windows Forms için)
        private void LogToStatusUi(string message)
        {
            if (connectionStatus.InvokeRequired)
            {
                connectionStatus.Invoke((Action)(() => connectionStatus.Text += message));
            }
            else
            {
                connectionStatus.Text += message;
            }
        }

        private void StopCallButton_Click(object sender, EventArgs e)
        {
            CleanupNetworkAndAudioResources();

            StartCallButton.Enabled = false;
            StopCallButton.Enabled = true;
            connectionStatus.Text = "Call stopped.";
        }


        private void CleanupNetworkAndAudioResources()
        {
            // Dinleme döngüsünü durdur
            cancellationTokenSource?.Cancel();

            // NAudio Kaynakları
            waveIn?.StopRecording();
            if (waveIn != null) waveIn.DataAvailable -= WaveIn_DataAvailable_SendOverNetwork;
            waveIn?.Dispose();
            waveIn = null;

            waveOut?.Stop();
            waveOut?.Dispose();
            waveOut = null;

            bufferedWaveProvider?.ClearBuffer();
            bufferedWaveProvider = null;

            // Opus Kaynakları (Eğer IOpus arayüzleri kullanılıyorsa ve IDisposable ise Dispose çağrılabilir)
            // Concentus'un doğrudan sınıfları IDisposable değil, null yapmak yeterli.
            // IOpusEncoder veya IOpusDecoder IDisposable ise:
            // (opusEncoder as IDisposable)?.Dispose();
            // (opusDecoder as IDisposable)?.Dispose();
            encoder = null;
            decoder = null;

            // UDP Kaynakları
            udpSender?.Close(); // Close() aynı zamanda Dispose() eder
            udpSender = null;

            udpListener?.Close();
            udpListener = null;

            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;

            //lblStatus.Text = "Kaynaklar temizlendi."; // UI thread'inden çağrılmalı
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private async void stunBox_Click(object sender, EventArgs e)
        {
            

            string stunServer = "stun.l.google.com"; // Google STUN server  
            int stunPort = 19302; // STUN server port  

            LogToStatusUi($"\nbaglaniyor: {stunServer}:{stunPort}");
            // koddaki stun :    {stunServer}:{stunPort}
            if (!STUNClient.TryParseHostAndPort($"stun.schlund.de:3478", out IPEndPoint stunEndPoint))
                throw new Exception("Failed to resolve STUN server address");

            STUNClient.ReceiveTimeout = 500;
            var queryResult = STUNClient.Query(stunEndPoint, STUNQueryType.ExactNAT, true); // Removed invalid parameter 'NATTypeDetectionRFC.Rfc5780'  

            if (queryResult.QueryError != STUNQueryError.Success)
                throw new Exception("Query Error: " + queryResult.QueryError.ToString());


            //LogToStatusUi($"Public End: {queryResult.PublicEndPoint}, Local End: {queryResult.LocalEndPoint}, NAT Type: {queryResult.NATType}");
            stunLabel.Text = $"Public End: {queryResult.PublicEndPoint}, Local End: {queryResult.LocalEndPoint}, NAT Type: {queryResult.NATType}";


            Console.WriteLine("PublicEndPoint: {0}", queryResult.PublicEndPoint);
            Console.WriteLine("LocalEndPoint: {0}", queryResult.LocalEndPoint);
            Console.WriteLine("NAT Type: {0}", queryResult.NATType);
        }
    }
}
