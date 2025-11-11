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

namespace LAB3_BAI3
{
    public partial class Client : Form
    {
        // Khai báo 2 biến ở cấp độ class
        // để các hàm (button click) khác có thể dùng chung
        private TcpClient client;
        private NetworkStream ns;
        public Client()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo đối tượng TcpClient
                client = new TcpClient();

                // Kết nối đến Server
                // Lấy IP và Port từ gợi ý (127.0.0.1 và 8080)
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8080);

                client.Connect(ipEndPoint);

                // Lấy luồng (stream)
                ns = client.GetStream();

                MessageBox.Show("Kết nối thành công!");

                // Cập nhật trạng thái các nút
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
            }
            catch (Exception ex)
            {
                // Báo lỗi nếu không kết nối được (ví dụ: Server chưa chạy)
                MessageBox.Show("Không thể kết nối đến server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected)
            {
                MessageBox.Show("Chưa kết nối đến server!");
                return;
            }

            try
            {
                // Giao tiếp
                // Lấy nội dung từ ô text
                string message = richTextBox1.Text;

                // Chuyển chuỗi thành mảng byte
                // Phải dùng Encoding.UTF8 để đồng bộ với Server
                // (Server của bạn ở bài trước đang dùng Encoding.UTF8.GetString)
                byte[] data = Encoding.UTF8.GetBytes(message);

                // Gửi dữ liệu đi
                ns.Write(data, 0, data.Length);

                // Xóa nội dung ô text sau khi gửi
                richTextBox1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gửi dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Đóng luồng và socket
                if (ns != null)
                {
                    ns.Close();
                }
                if (client != null)
                {
                    client.Close();
                }

                MessageBox.Show("Đã ngắt kết nối.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ngắt kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Cập nhật lại trạng thái nút
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void Client_Load(object sender, EventArgs e)
        {
            // Trạng thái ban đầu: Chỉ cho phép kết nối
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
        }
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Đảm bảo đóng kết nối khi tắt form
            if (ns != null)
            {
                ns.Close();
            }
            if (client != null)
            {
                client.Close();
            }
        }
    }
}
