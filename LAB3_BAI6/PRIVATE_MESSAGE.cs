using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;

namespace LAB3_BAI6
{
    public partial class PRIVATE_MESSAGE : Form
    {
        private string recipientName;

        private string senderName;

        private NetworkStream networkStream;

        public PRIVATE_MESSAGE()
        {
            InitializeComponent();
        }

        // Constructor chính mà chúng ta sử dụng
        // Được gọi từ form Client.cs
        public PRIVATE_MESSAGE(string recipientName, string senderName, NetworkStream networkStream)
        {
            InitializeComponent();

            // Lưu lại các thông tin được truyền vào
            this.recipientName = recipientName;
            this.senderName = senderName;
            this.networkStream = networkStream;

            // Đổi tiêu đề cửa sổ
            this.Text = $"Private chat with {recipientName}";
        }

        // Hàm public này được gọi bởi Client.cs để thêm tin nhắn vào chat box
        // Nó phải là thread-safe vì Client.cs gọi nó từ một luồng khác
        public void AddMessage(string message)
        {
            // Kiểm tra xem có cần gọi Invoke (chuyển luồng) không
            if (richTextBox1.InvokeRequired)
            {
                // Nếu đang ở luồng khác, hãy gọi lại chính hàm này trên luồng UI
                richTextBox1.Invoke(new Action(() => AddMessage(message)));
            }
            else
            {
                // Nếu đã ở trên luồng UI, cập nhật RichTextBox
                richTextBox1.AppendText(message + "\n");
                richTextBox1.ScrollToCaret(); // Tự động cuộn xuống cuối
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Lấy nội dung tin nhắn từ textBox1
            string messageContent = textBox1.Text;

            if (!string.IsNullOrWhiteSpace(messageContent) && networkStream != null && networkStream.CanWrite)
            {
                try
                {
                    // Đóng gói tin nhắn theo giao thức đã định
                    // PRIVATE_MSG|TênNgườiNhận|Nội dung tin nhắn
                    string message = $"PRIVATE_MSG|{recipientName}|{messageContent}";

                    // Chuyển chuỗi thành byte array để gửi đi
                    byte[] data = Encoding.UTF8.GetBytes(message);

                    // Gửi dữ liệu qua NetworkStream
                    networkStream.Write(data, 0, data.Length);

                    // Xóa nội dung trong textBox1 sau khi gửi
                    textBox1.Clear();
                }
                catch (IOException)
                {
                    AddMessage("Lỗi: Không thể gửi tin nhắn, kết nối đã mất.");
                }
                catch (Exception ex)
                {
                    // Hiển thị lỗi nếu có sự cố
                    AddMessage($"Lỗi khi gửi tin: {ex.Message}");
                }
            }
        }
    }
}