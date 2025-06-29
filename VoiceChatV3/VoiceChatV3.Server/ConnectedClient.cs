// VoiceChat.Server/ConnectedClient.cs

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace VoiceChat.Server
{
    public class ConnectedClient
    {
        public string Id { get; private set; }
        public string Nickname { get; private set; }
        public string IpAddress { get; private set; }

        private readonly TcpClient _tcpClient;
        private readonly Server _server;
        private readonly StreamWriter _writer;
        private readonly StreamReader _reader;

        public ConnectedClient(TcpClient tcpClient, Server server)
        {
            _tcpClient = tcpClient;
            _server = server;

            // Benzersiz bir ID oluştur
            Id = Guid.NewGuid().ToString();
            // Client'ın genel IP adresini al
            IpAddress = ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address.ToString();

            var stream = _tcpClient.GetStream();
            _writer = new StreamWriter(stream) { AutoFlush = true };
            _reader = new StreamReader(stream);

            // Client'tan gelen mesajları dinlemek için bir Task başlat
            Task.Run(ProcessMessagesAsync);
        }

        private async Task ProcessMessagesAsync()
        {
            try
            {
                // İlk gelen mesaj LOGIN olmalı
                string message = await _reader.ReadLineAsync();
                if (message != null && message.StartsWith("LOGIN:"))
                {
                    Nickname = message.Substring(6);
                    // Client'a kendi ID'sini gönder
                    SendMessage($"ID:{this.Id}");
                    // Kendimizi server'ın listesine ekle
                    _server.AddClient(this);
                }
                else
                {
                    throw new Exception("Geçersiz giriş protokolü. İlk mesaj LOGIN olmalı.");
                }

                // Giriş yaptıktan sonra diğer mesajları dinlemeye devam et
                while ((message = await _reader.ReadLineAsync()) != null)
                {
                    var parts = message.Split(new[] { ':' }, 3); // UDP_INFO için 3 parça olabilir
                    var command = parts[0];
                    var targetId = parts.Length > 1 ? parts[1] : string.Empty;
                    var data = parts.Length > 2 ? parts[2] : string.Empty;

                    HandleCommand(command, targetId, data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata ({Nickname} - {Id}): {ex.Message}");
            }
            finally
            {
                // Bağlantı koptuğunda veya hata olduğunda client'ı sunucudan kaldır
                _server.RemoveClient(this.Id);
                _tcpClient.Close();
            }
        }

        private void HandleCommand(string command, string targetId, string data)
        {
            switch (command)
            {
                // Bu komutlar, gönderenin ID'si ile birlikte hedefe yönlendirilir
                case "CALL":
                case "ACCEPT":
                case "REJECT":
                case "END_CALL":
                    // Örnek: Client A "CALL:123" gönderir.
                    // Sunucu bunu "CALL:456" olarak Client B'ye iletir (456, Client A'nın ID'si).
                    _server.ForwardMessage(targetId, $"{command}:{this.Id}");
                    break;

                // Bu komut en önemlisi. Mesajı zenginleştirip yönlendirir.
                case "UDP_INFO":
                    string udpPort = data;
                    // Mesajı zenginleştir: Gönderenin genel IP'sini ekle
                    string forwardMessage = $"UDP_INFO:{this.IpAddress}:{udpPort}";
                    _server.ForwardMessage(targetId, forwardMessage);
                    break;
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                _writer.WriteLine(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mesaj gönderme hatası ({Nickname} - {Id}): {ex.Message}");
            }
        }
    }
}