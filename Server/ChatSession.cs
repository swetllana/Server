using System;
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Server
{
    public class ChatSession : SslSession
    {
        public ChatSession(SslServer server) : base(server) { }

        protected override void OnConnected()
        {
            Console.WriteLine($"Chat SSL session with Id {Id} connected!");
        }

        protected override void OnHandshaked()
        {
            Console.WriteLine($"Chat SSL session with Id {Id} handshaked!");

            // Send invite message
            string message = "Hello from SSL chat! Please send a message or '!' to disconnect the client!";
            Send(message);
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"Chat SSL session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            string decompressed = "";

            //decompress
            byte[] inputBytes2 = Convert.FromBase64String(message);

            using (var inputStream = new MemoryStream(inputBytes2))
            using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var outputStream = new MemoryStream())
            {
                gZipStream.CopyTo(outputStream);
                var outputBytes = outputStream.ToArray();

                decompressed = Encoding.UTF8.GetString(outputBytes);
            }

            Data data = JsonConvert.DeserializeObject<Data>(decompressed);
            string show = "Name: " + data.Name + "\n" +
                          "Surname: " + data.Surname + "\n" +
                          "Town: " + data.Town + "\n" +
                          "Postal code: " + data.PostalCode + "\n" +
                          "Program version: " + data.ProgramVersion + "\n" +
                          "Email: " + data.Email + "\n" +
                          "Music: " + data.Music + "\n" +
                          "Singer: " + data.Singer + "\n" +
                          "Year: " + data.Year + "\n" +
                          "Hour: " + data.Hour + "\n";
            MessageBox.Show(show);

            //// Multicast message to all connected sessions
            // Server.Multicast(decompressed);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat SSL session caught an error with code {error}");
        }
    }
}
