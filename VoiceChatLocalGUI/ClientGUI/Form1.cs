using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using NAudio.Wave;
namespace ClientGUI
{
    public partial class Form1 : Form
    {
        Socket sock;
        private NAudio.Wave.WaveIn waveIn = null;
        private byte[] buffer = new byte[1024]; // Added buffer initialization

        public Form1()
        {
            InitializeComponent();
            sock = socket();
        }
        Socket socket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                sock.Connect(IPAddress.Parse(textBox1.Text), 1234);
                MessageBox.Show("Connected to server!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to server: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<NAudio.Wave.WaveInCapabilities> devices = new List<NAudio.Wave.WaveInCapabilities>();
            for (int i = 0; i < NAudio.Wave.WaveIn.DeviceCount; i++)
            {
                devices.Add(NAudio.Wave.WaveIn.GetCapabilities(i));
            }
            deviceList.Items.Clear();
            foreach (var device in devices)
            {
                ListViewItem item = new ListViewItem(device.ProductName);
                item.SubItems.Add(new ListViewItem.ListViewSubItem(item, device.Channels.ToString()));
                deviceList.Items.Add(item);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (deviceList.SelectedItems.Count == 0) // Corrected condition to check if an item is selected  
            {
                MessageBox.Show("Please select a device from the list.");
                return;
            }
            try
            {
                int deviceNumber = deviceList.SelectedItems[0].Index; // Get the selected device index
                waveIn = new NAudio.Wave.WaveIn();
                waveIn.BufferMilliseconds = 30;
                // Fix for CS1061: Replace 'Index' with 'deviceList.SelectedItems[0].Index'  
                waveIn.DeviceNumber = deviceNumber; // Fixed assignment to DeviceNumber  
                waveIn.WaveFormat = new WaveFormat(44100, 16, 1); // 44.1 kHz, mono 

                waveIn.DataAvailable += (s, a) => // s burda waveIn nesnesi a ise buffer fe bytereads arg
                {
                    try
                    {
                        if (sock.Connected)
                        {
                            sock.Send(a.Buffer, a.BytesRecorded, SocketFlags.None);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error sending audio data: " + ex.Message);
                    }
                };
                waveIn.StartRecording();
                MessageBox.Show("Recording started.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting recording: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (waveIn != null)
            {
                try
                {
                    waveIn.StopRecording();
                    waveIn.Dispose();
                    waveIn = null;
                    MessageBox.Show("Recording stopped.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error stopping recording: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No recording in progress.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (waveIn != null)
                {
                    waveIn.StopRecording();
                    waveIn.Dispose();
                    waveIn = null;
                }
                if (sock != null && sock.Connected)
                {
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();
                    sock = null;
                    MessageBox.Show("Disconnected from server.");
                }
                else
                {
                    MessageBox.Show("Not connected to any server.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error disconnecting: " + ex.Message);
            }
            finally
            {
                deviceList.Items.Clear();
                Application.Exit(); 
            }
        }
    }
}
