using System;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Server
{
    public partial class FrmServer : Form
    {
        int port = 2222;
        SslContext context = new SslContext();
        ChatServer server = new ChatServer();

        public FrmServer()
        {
            InitializeComponent();
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            context = new SslContext(SslProtocols.Tls12, new X509Certificate2("server.pfx", "qwerty"));
            server = new ChatServer(context, IPAddress.Any, port);

            // Start the server
            txtInfo.Text = "Server starting...";
            server.Start();
            txtInfo.Text = "Done!";
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            // Stop the server
            txtInfo.Text = "Server stopping...";
            server.Stop();
            txtInfo.Text = "Done!";
        }

        private void FrmServer_Load(object sender, EventArgs e)
        {

        }
    }
}
