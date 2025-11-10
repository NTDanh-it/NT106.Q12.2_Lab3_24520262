using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB3_BAI1
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string remoteIP = textBox1.Text.Trim();
            string message = richTextBox1.Text.Trim();
            int remotePort;

            // Kiểm tra IP
            if (string.IsNullOrWhiteSpace(remoteIP))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ IP của Server.");
                return;
            }

            // Kiểm tra port
            if (!int.TryParse(textBox2.Text.Trim(), out remotePort))
            {
                MessageBox.Show("Port không hợp lệ. Vui lòng nhập số.");
                return;
            }

            // Kiểm tra nội dung message
            if (string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show("Nội dung tin nhắn không được để trống.");
                return;
            }

            try
            {
                // Tạo client UDP
                UdpClient udpClient = new UdpClient();

                // Chuyển chuỗi thành mảng byte
                byte[] sendBytes = Encoding.UTF8.GetBytes(message);

                // Gửi dữ liệu đến server
                udpClient.Send(sendBytes, sendBytes.Length, remoteIP, remotePort);

                MessageBox.Show("Đã gửi dữ liệu đến Server!");

                // Xóa nội dung sau khi gửi
                richTextBox1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi dữ liệu: " + ex.Message);
            }
        }
    }
}
