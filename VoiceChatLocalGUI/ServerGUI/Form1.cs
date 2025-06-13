using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using NAudio.Wave;

namespace ServerGUI
{
    public partial class Form1 : Form
    {
        private Socket serverSocket;
        private Thread listenThread;
        private WaveOutEvent waveOut;
        private BufferedWaveProvider waveProvider;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int port = 1234;
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
            serverSocket.Listen(1);

            listenThread = new Thread(ListenForClient);
            listenThread.IsBackground = true;
            listenThread.Start();

            MessageBox.Show("Sunucu baþlatýldý ve dinliyor (Port: " + port + ")");
        }

        private void ListenForClient()
        {
            try
            {
                using (Socket clientSocket = serverSocket.Accept())
                using (NetworkStream stream = new NetworkStream(clientSocket))
                {
                    // PCM formatý: 44100 Hz, 16 bit, mono
                    var format = new WaveFormat(44100, 16, 1);
                    waveProvider = new BufferedWaveProvider(format);
                    waveProvider.DiscardOnBufferOverflow = true;

                    waveOut = new WaveOutEvent();
                    waveOut.Init(waveProvider);
                    waveOut.Play();

                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        waveProvider.AddSamples(buffer, 0, bytesRead);
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj göstermek için (isteðe baðlý)
                Invoke(new Action(() =>
                {
                    MessageBox.Show("Baðlantý sýrasýnda hata: " + ex.Message);
                }));
            }
            finally
            {
                if (waveOut != null)
                {
                    waveOut.Stop();
                    waveOut.Dispose();
                    waveOut = null;
                }
            }
        }
    }
}
