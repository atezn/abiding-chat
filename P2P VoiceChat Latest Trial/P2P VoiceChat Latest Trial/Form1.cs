using System;
using System.Windows.Forms;
using NAudio.Wave;
using Concentus;
using Concentus.Structs;
using Concentus.Enums;
using System.IO;
using System.Net;
using System.Net.Sockets;
using STUN; // Correct namespace for STUNUtils

namespace P2P_VoiceChat_Latest_Trial
{
    public partial class Form1 : Form
    {

        private WaveInEvent? waveIn;
        private WaveOutEvent? waveOut;
        private BufferedWaveProvider? bufferedWaveProvider;

        private OpusEncoder? encoder;
        private OpusDecoder? decoder;

        private UdpClient? udpSender; // Declare UDP client if needed for future use
        private UdpClient? udpReceiver; // Declare STUN client if needed for future use


        private bool isLooping = false;
        private bool isReceiving = false;


        public Form1()
        {
            InitializeComponent();
        }

        private void startLoopBackButton_Click(object sender, EventArgs e)
        {
            if (isLooping) // eger zaten calisiyorsa pop up mesaj
            {
                MessageBox.Show("Loopback is already running.");
                return;
            }

            encoder = new OpusEncoder(48000, 1, OpusApplication.OPUS_APPLICATION_VOIP)
            {   // encoder ve decoder baska sekilde declare edildiginde hata veriyor sadece bu sekilde calistirabildim
                Bitrate = 64000
            };

            decoder = new OpusDecoder(48000, 1);

            waveIn = new WaveInEvent
            {
                DeviceNumber = 0,
                WaveFormat = new WaveFormat(48000, 16, 1),
                BufferMilliseconds = 20
            };

            waveIn.DataAvailable += WaveIn_DataAvailable;
            waveIn.StartRecording();

            bufferedWaveProvider = new BufferedWaveProvider(waveIn.WaveFormat);
            waveOut = new WaveOutEvent();
            waveOut.Init(bufferedWaveProvider);
            waveOut.Play();

            isLooping = true;

        }



        private void stopLoopBackButton_Click(object sender, EventArgs e)
        {
            if (!isLooping) // eger zaten calismiyorsa pop up mesaj
            {
                MessageBox.Show("Loopback is not running.");
                return;
            }

            waveIn.StopRecording();
            waveIn.Dispose();
            waveIn = null;

            waveOut.Stop();
            waveOut.Dispose();
            waveOut = null;

            encoder.Dispose();
            decoder.Dispose();

            isLooping = false;

        }


        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (!isLooping) return;

            short[] pcm = new short[e.BytesRecorded / 2];
            Buffer.BlockCopy(e.Buffer, 0, pcm, 0, e.BytesRecorded);

            byte[] encoded = new byte[4000];
            int encodedLength = encoder.Encode(pcm, 0, pcm.Length, encoded, 0, encoded.Length);

            short[] decodedPcm = new short[960 * 6];
            int decodedSamples = decoder.Decode(encoded, 0, encodedLength, decodedPcm, 0, decodedPcm.Length, false);

            byte[] decodedBytes = new byte[decodedSamples * 2];
            Buffer.BlockCopy(decodedPcm, 0, decodedBytes, 0, decodedBytes.Length);

            bufferedWaveProvider.AddSamples(decodedBytes, 0, decodedBytes.Length);
        }


        private void stunBox_Click(object sender, EventArgs e)
        {
            ///"stun.l.google.com:19302"; // Google STUN server  

            if (!STUNClient.TryParseHostAndPort($"stun.schlund.de:3478", out IPEndPoint stunEndPoint))
                throw new Exception("Failed to resolve STUN server address");

            STUNClient.ReceiveTimeout = 500;
            var queryResult = STUNClient.Query(stunEndPoint, STUNQueryType.ExactNAT, true); // Removed invalid parameter 'NATTypeDetectionRFC.Rfc5780'  

            if (queryResult.QueryError != STUNQueryError.Success)
                throw new Exception("Query Error: " + queryResult.QueryError.ToString());

            stunIPTxt.Text = queryResult.PublicEndPoint.Address.ToString();
            stunPortTxt.Text = queryResult.PublicEndPoint.Port.ToString();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void startSendingButton_Click(object sender, EventArgs e)
        {
            string targetIp = targetIPBox.Text.Trim();
            int targetPort = 5500; //int.Parse(targetPortBox.Text.Trim());
            udpSender = new UdpClient();
            IPEndPoint targetEndPoint = new IPEndPoint(IPAddress.Parse(targetIp), targetPort);

            encoder = new OpusEncoder(48000, 1, OpusApplication.OPUS_APPLICATION_VOIP);
            encoder.Bitrate = 64000; // Set bitrate for Opus encoder

            waveIn = new WaveInEvent();
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = new WaveFormat(48000, 16, 1); // mono, 48kHz
            waveIn.BufferMilliseconds = 20;
            waveIn.DataAvailable += (s, a) =>
            {
                short[] pcm = new short[a.BytesRecorded / 2];
                Buffer.BlockCopy(a.Buffer, 0, pcm, 0, a.BytesRecorded);

                byte[] encoded = new byte[4000];
                int encodedLength = encoder.Encode(pcm, 0, pcm.Length, encoded, 0, encoded.Length);
                
                if (encodedLength < 0)
                {
                    MessageBox.Show("Encoding failed, length is negative.");
                    return;
                }
                byte[] toSend = new byte[encodedLength];
                Array.Copy(encoded, toSend, encodedLength);



                udpSender.Send(toSend, toSend.Length, targetEndPoint);
                //Console.WriteLine($"[SEND] Sending {toSend.Length} bytes to {targetEndPoint}");
                MessageBox.Show($"Sending {toSend.Length} bytes to {targetEndPoint}");
            };

            waveIn.StartRecording();
        }

        private void stopSendingButton_Click(object sender, EventArgs e)
        {
            waveIn?.StopRecording();
            waveIn?.Dispose();
            waveIn = null;

            udpSender?.Close();
            udpSender = null;

            encoder?.Dispose();
            encoder = null;
        }

        private void startReceivingButton_Click(object sender, EventArgs e)
        {
            int listenPort = 5500; //int.Parse(targetPortBox.Text.Trim()); // aynı textbox kullanılabilir
            udpReceiver = new UdpClient(listenPort);

            decoder = new OpusDecoder(48000, 1);

            bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(48000, 16, 1));
            waveOut = new WaveOutEvent();
            waveOut.Init(bufferedWaveProvider);
            waveOut.Play();

            isReceiving = true;
            Task.Run(() =>
            {
                while (isReceiving)
                {
                    try
                    {
                        IPEndPoint remoteEP = null;
                        byte[] received = udpReceiver.Receive(ref remoteEP);

                        short[] decoded = new short[1920];
                        int samplesDecoded = decoder.Decode(received, 0, received.Length, decoded, 0, decoded.Length, false);

                        byte[] buffer = new byte[samplesDecoded * 2];
                        Buffer.BlockCopy(decoded, 0, buffer, 0, buffer.Length);
                        bufferedWaveProvider.AddSamples(buffer, 0, buffer.Length);
                        MessageBox.Show($"Received {buffer.Length} bytes from {remoteEP}");
                    }
                    catch { }
                }
            });
        }

        private void stopReceivingButton_Click(object sender, EventArgs e)
        {
            isReceiving = false;

            udpReceiver?.Close();
            udpReceiver = null;

            waveOut?.Stop();
            waveOut?.Dispose();
            waveOut = null;

            decoder = null;
        }
    }
}
