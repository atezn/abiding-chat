// VoiceChat.Server/Server.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace VoiceChat.Server
{
    public class Server
    {
        private readonly TcpListener _listener;
        // Thread-safe (iş parçacığı güvenli) bir Dictionary kullanalım
        private readonly Dictionary<string, ConnectedClient> _clients = new Dictionary<string, ConnectedClient>();
        private readonly object _lock = new object(); // Dictionary'ye erişimi kilitlemek için

        public Server(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            _listener.Start();
            Task.Run(AcceptClientsAsync); // Bağlantıları arka planda kabul et
        }

        private async Task AcceptClientsAsync()
        {
            while (true)
            {
                try
                {
                    var tcpClient = await _listener.AcceptTcpClientAsync();
                    Console.WriteLine("Yeni bir bağlantı isteği alındı...");
                    // Yeni client'ı yönetmesi için bir nesne oluştur.
                    // Bu nesne kendini listeye ekleyip çıkaracak.
                    new ConnectedClient(tcpClient, this);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Bağlantı kabul hatası: {ex.Message}");
                }
            }
        }

        public void AddClient(ConnectedClient client)
        {
            lock (_lock)
            {
                _clients[client.Id] = client;
            }
            Console.WriteLine($"Client giriş yaptı: {client.Nickname} ({client.Id})");
            BroadcastUserList();
        }

        public void RemoveClient(string clientId)
        {
            lock (_lock)
            {
                if (clientId != null && _clients.ContainsKey(clientId))
                {
                    _clients.Remove(clientId);
                    Console.WriteLine($"Client ayrıldı: {clientId}");
                }
            }
            BroadcastUserList();
        }

        public void ForwardMessage(string targetId, string message)
        {
            ConnectedClient targetClient;
            lock (_lock)
            {
                _clients.TryGetValue(targetId, out targetClient);
            }

            if (targetClient != null)
            {
                targetClient.SendMessage(message);
                Console.WriteLine($"Mesaj yönlendirildi -> Hedef: {targetId}, Mesaj: {message}");
            }
            else
            {
                Console.WriteLine($"Yönlendirme hatası: Hedef client bulunamadı: {targetId}");
            }
        }

        public void BroadcastUserList()
        {
            string payload;
            lock (_lock)
            {
                // Format: "id1=nick1,id2=nick2"
                payload = string.Join(",", _clients.Values.Select(c => $"{c.Id}={c.Nickname}"));
            }

            string message = $"USERLIST:{payload}";

            lock (_lock)
            {
                foreach (var client in _clients.Values)
                {
                    client.SendMessage(message);
                }
            }
            Console.WriteLine("Kullanıcı listesi tüm client'lara yayınlandı.");
        }
    }
}