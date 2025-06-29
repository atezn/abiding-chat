
using System;

namespace VoiceChat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 55000; // Sunucunun çalışacağı port
            try
            {
                Server server = new Server(port);
                server.Start();

                Console.WriteLine($"Sunucu {port} portunda başlatıldı.");
                Console.WriteLine("Sunucuyu kapatmak için herhangi bir tuşa basın.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sunucu başlatılırken bir hata oluştu: {ex.Message}");
                Console.WriteLine("Çıkmak için bir tuşa basın.");
                Console.ReadKey();
            }
        }
    }
}