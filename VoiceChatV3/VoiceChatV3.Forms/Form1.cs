using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO; // StreamWriter ve StreamReader için gerekli
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks; // Task için gerekli
using System.Windows.Forms;

namespace VoiceChatV3.Forms
{
    public partial class Form1 : Form
    {
        // Að ile ilgili deðiþkenler
        private UdpClient _udpClient;
        private TcpClient _tcpClient;
        private StreamWriter _streamWriter;
        private StreamReader _streamReader;
        private IPEndPoint _remoteEndPoint; // Ses göndereceðimiz kiþinin adresi

        // Ses ile ilgili deðiþkenler
        private WaveInEvent _waveInEvent;
        private WaveOutEvent _waveOutEvent;
        private BufferedWaveProvider _bufferedWaveProvider;

        // Kullanýcý ve durum bilgileri
        private Dictionary<string, string> _userList = new Dictionary<string, string>();
        private string _myId;
        private string _activeCallPartnerId;

        public Form1()
        {
            InitializeComponent();
            LoadDevices(); // LoadDevice yerine standart C# isimlendirmesi
        }

        // DÜZELTME: Cihaz listeleme ComboBox için daha basit ve doðru.
        private void LoadDevices()
        {
            deviceList.Items.Clear();
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var deviceInfo = WaveIn.GetCapabilities(i);
                deviceList.Items.Add(deviceInfo.ProductName);
            }
            if (deviceList.Items.Count > 0)
            {
                deviceList.SelectedIndex = 0;
            }
        }

        private async Task ListenToServer()
        {
            try
            {
                while (_tcpClient != null && _tcpClient.Connected)
                {
                    var message = await _streamReader.ReadLineAsync();
                    if (string.IsNullOrEmpty(message))
                    {
                        // Baðlantý sunucu tarafýndan kapatýldý veya koptu
                        throw new Exception("Sunucu ile baðlantý koptu.");
                    }
                    ProcessServerMessage(message);
                }
            }
            catch (Exception)
            {
                this.Invoke((MethodInvoker)delegate {
                    if (Text != "Baðlantý Koptu") // Tekrar tekrar mesaj göstermesin
                    {
                        MessageBox.Show("Sunucu ile baðlantý kesildi.");
                    }
                    ResetConnection();
                });
            }
        }

        // DÜZELTME: StartAudio artýk parametre almayacak.
        private void StartAudio()
        {
            if (string.IsNullOrEmpty(_activeCallPartnerId)) return;

            // UDP client'ý oluþtur ve dinlemeye baþla
            _udpClient = new UdpClient();
            _udpClient.BeginReceive(OnUdpDataReceived, null);

            // Kendi UDP portumuzu karþý tarafa bildirmesi için sunucuya gönder
            int localPort = ((IPEndPoint)_udpClient.Client.LocalEndPoint).Port;
            _streamWriter.WriteLine($"UDP_INFO:{_activeCallPartnerId}:{localPort}");

            // Ses yakalamayý baþlat
            _waveInEvent = new WaveInEvent();
            _waveInEvent.DeviceNumber = deviceList.SelectedIndex;
            _waveInEvent.WaveFormat = new WaveFormat(16000, 16, 1);
            _waveInEvent.DataAvailable += OnDataAvailable;
            _waveInEvent.StartRecording();

            // Ses oynatmayý baþlat
            _waveOutEvent = new WaveOutEvent();
            _bufferedWaveProvider = new BufferedWaveProvider(_waveInEvent.WaveFormat);
            _waveOutEvent.Init(_bufferedWaveProvider);
            _waveOutEvent.Play();

            Start.Enabled = false;
            End.Enabled = true;
        }

        // YENÝ: Ses akýþýný durduran ve her þeyi temizleyen metot
        private void EndAudio()
        {
            // Ses kaynaklarýný serbest býrak
            _waveInEvent?.StopRecording();
            if (_waveInEvent != null)
            {
                _waveInEvent.DataAvailable -= OnDataAvailable;
                _waveInEvent.Dispose();
                _waveInEvent = null;
            }

            _waveOutEvent?.Stop();
            _waveOutEvent?.Dispose();
            _waveOutEvent = null;

            _bufferedWaveProvider = null;

            // Að kaynaklarýný serbest býrak
            _udpClient?.Close();
            _udpClient = null;
            _remoteEndPoint = null;

            // Durumu sýfýrla
            _activeCallPartnerId = null;

            // Butonlarý ayarla
            Start.Enabled = true;
            End.Enabled = false;
            Text = $"Baðlandý - ID: {_myId}";
        }

        // YENÝ: Eksik olan olay yöneticileri
        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (_udpClient != null && _remoteEndPoint != null)
            {
                _udpClient.Send(e.Buffer, e.BytesRecorded, _remoteEndPoint);
            }
        }

        private void OnUdpDataReceived(IAsyncResult ar)
        {
            try
            {
                if (_udpClient == null) return;
                IPEndPoint receivedEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = _udpClient.EndReceive(ar, ref receivedEndPoint);

                if (_bufferedWaveProvider != null)
                {
                    _bufferedWaveProvider.AddSamples(buffer, 0, buffer.Length);
                }
                _udpClient.BeginReceive(OnUdpDataReceived, null);
            }
            catch (ObjectDisposedException) { /* Normal, görmezden gel */ }
        }

        private void ProcessServerMessage(string message)
        {
            var parts = message.Split(new[] { ':' }, 2);
            var command = parts[0];
            var data = parts.Length > 1 ? parts[1] : string.Empty;

            this.Invoke((MethodInvoker)delegate
            {
                switch (command)
                {
                    case "ID":
                        _myId = data;
                        Text = $"Baðlandý - ID: {_myId}";
                        break;

                    case "USERLIST":
                        _userList.Clear();
                        userList.Items.Clear();
                        if (!string.IsNullOrEmpty(data))
                        {
                            var users = data.Split(',');
                            foreach (var user in users)
                            {
                                var userParts = user.Split('=');
                                if (userParts.Length == 2)
                                {
                                    string id = userParts[0];
                                    string nickname = userParts[1];
                                    if (id != _myId)
                                    {
                                        _userList[id] = nickname;
                                        userList.Items.Add($"{nickname} ({id})");
                                    }
                                }
                            }
                        }
                        break;

                    case "INCOMING_CALL":
                        var callerId = data;
                        var callerNickname = _userList.ContainsKey(callerId) ? _userList[callerId] : "Bilinmeyen";
                        var result = MessageBox.Show($"{callerNickname} sizi arýyor. Kabul et?", "Gelen Arama", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            _streamWriter.WriteLine($"ACCEPT:{callerId}");
                            _activeCallPartnerId = callerId;
                            StartAudio();
                        }
                        else
                        {
                            _streamWriter.WriteLine($"REJECT:{callerId}");
                        }
                        break;

                    // DÜZELTME: CALL_ACCEPTED'da StartAudio parametresiz çaðrýlmalý
                    case "CALL_ACCEPTED":
                        var accepterId = data;
                        MessageBox.Show("Aramanýz kabul edildi.");
                        _activeCallPartnerId = accepterId;
                        StartAudio();

                        break;

                    // YENÝ: Reddedilme durumunu yönet
                    case "CALL_REJECTED":
                        MessageBox.Show("Aramanýz reddedildi.");
                        // Butonlarý eski haline getir
                        Start.Enabled = true;
                        break;

                    case "END_CALL":
                        MessageBox.Show("Karþý taraf çaðrýyý sonlandýrdý.");
                        EndAudio();
                        break;

                    case "UDP_INFO":
                        var remoteIpPort = data.Split(':');
                        _remoteEndPoint = new IPEndPoint(IPAddress.Parse(remoteIpPort[0]), int.Parse(remoteIpPort[1]));
                        break;
                }
            });
        }

        // DÜZELTME: Metot adý standartlaþtýrýldý
        private async void connect_Click(object sender, EventArgs e)
        {
            if (_tcpClient != null && _tcpClient.Connected)
            {
                MessageBox.Show("Zaten baðlýsýnýz.");
                return;
            }
            try
            {
                string serverIp = txtServerIp.Text;
                int port = int.Parse(txtPort.Text);

                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(serverIp, port);

                var stream = _tcpClient.GetStream();
                _streamWriter = new StreamWriter(stream) { AutoFlush = true };
                _streamReader = new StreamReader(stream);

                // DÜZELTME: Önce giriþ yap, sonra mesaj göster
                await _streamWriter.WriteLineAsync($"LOGIN:User_{new Random().Next(1000)}");
                Task.Run(() => ListenToServer());

                connect.Enabled = false;
                Text = "Sunucuya baðlanýlýyor...";
            }
            catch (FormatException) { MessageBox.Show("Lütfen geçerli bir port numarasý girin."); }
            catch (SocketException) { MessageBox.Show("Sunucuya ulaþýlamýyor."); ResetConnection(); }
            catch (Exception ex) { MessageBox.Show("Beklenmedik bir hata: " + ex.Message); ResetConnection(); }
        }

        // DÜZELTME: Start_Click'teki komut adý sunucuyla tutarlý olmalý
        private void Start_Click(object sender, EventArgs e)
        {
            if (userList.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir kullanýcý seçin.");
                return;
            }

            string selectedItem = userList.SelectedItem.ToString();
            string targetId = selectedItem.Substring(selectedItem.IndexOf('(') + 1).TrimEnd(')');

            // Sunucunun anlayacaðý komut "CALL" olmalý
            _streamWriter.WriteLine($"CALL:{targetId}");
            Start.Enabled = false; // Tekrar arama yapýlmasýn
            Text = "Arama isteði gönderildi...";
        }

        // DÜZELTME: End_Click'in doðru çalýþmasý
        private void End_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_activeCallPartnerId))
            {
                _streamWriter.WriteLine($"END_CALL:{_activeCallPartnerId}");
            }
            EndAudio();
        }

        // YENÝ: Baðlantý sýfýrlama metodu
        private void ResetConnection()
        {
            _tcpClient?.Close();
            _tcpClient = null;
            _streamWriter = null;
            _streamReader = null;
            connect.Enabled = true;
            userList.Items.Clear();
            _userList.Clear();
            Text = "Baðlantý Yok";
        }

        // DÜZELTME: Form kapanýrken kaynaklarý temizle
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            EndAudio();
            ResetConnection();
        }

        // Kullanýlmayan metotlar þimdilik boþ kalabilir
        private void txtServerIp_TextChanged(object sender, EventArgs e) { }
        private void deviceList_SelectedIndexChanged(object sender, EventArgs e) { }
        private void refresh_Click(object sender, EventArgs e) { LoadDevices(); }
        private void userList_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtPort_TextChanged(object sender, EventArgs e) { }
        private void Exit_Click(object sender, EventArgs e) { this.Close(); }
    }
}