using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB3_BAI1
{

    public partial class Server : Form
    {
        private Thread threadServer;
        private UdpClient udpServer;
        public Server()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();

            try
            {
                int port = int.Parse(textBox1.Text.Trim());
                threadServer = new Thread(() => StartListening(port));
                threadServer.IsBackground = true;
                threadServer.Start();

                MessageBox.Show("Server đang lắng nghe trên cổng " + port);
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng số cho port!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void StartListening(int port)
        {
            try
            {
                udpServer = new UdpClient(port);
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

                while (true)
                {
                    byte[] receiveBytes = udpServer.Receive(ref remoteEP);
                    string receiveData = Encoding.UTF8.GetString(receiveBytes);

                    string message = $"{remoteEP.Address} : {receiveData}";
                    AddMessage(message);
                }
            }
            catch (SocketException)
            {
                AddMessage("Server đã dừng lắng nghe.");
            }
            catch (Exception ex)
            {
                AddMessage("Lỗi nhận dữ liệu: " + ex.Message);
            }
        }

        private void AddMessage(string msg)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action<string>(AddMessage), msg);
            }
            else
            {
                richTextBox1.AppendText(msg + Environment.NewLine);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
