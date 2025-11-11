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
    public partial class Server : Form
    {
        // Delegate để đảm bảo an toàn khi gọi hàm từ các Thread khác
        private delegate void SafeCallDelegate(string text);
        // Delegate cho hàm không tham số (dùng để Clear)
        private delegate void SafeCallVoidDelegate();
        // Khai báo listenerSocket ở cấp độ class để có thể dừng nó
        private Socket listenerSocket;
        public Server()
        {
            InitializeComponent();
        }
        // Hàm an toàn để THÊM text vào RichTextBox
        private void AppendTextSafe(string text)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                this.richTextBox1.Invoke(new SafeCallDelegate(AppendTextSafe), text);
            }
            else
            {
                this.richTextBox1.AppendText(text);
                this.richTextBox1.ScrollToCaret();
            }
        }

        // Hàm an toàn để XÓA text khỏi RichTextBox
        private void ClearRichTextBoxSafe()
        {
            if (this.richTextBox1.InvokeRequired)
            {
                this.richTextBox1.Invoke(new SafeCallVoidDelegate(ClearRichTextBoxSafe));
            }
            else
            {
                this.richTextBox1.Clear();
            }
        }
        // Hàm chạy trong Thread nền
        void StartListenerThread()
        {
            try
            {
                int bytesReceived;
                byte[] recvBuffer = new byte[1024];
                Socket clientSocket;

                listenerSocket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp
                );
                IPEndPoint ipepServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
                listenerSocket.Bind(ipepServer);
                listenerSocket.Listen(10);

                AppendTextSafe("Server đang lắng nghe trên cổng 8080...\n");

                while (true)
                {
                    // Chấp nhận kết nối (Blocking call)
                    // Dòng này sẽ ném ra lỗi 10004 khi ta gọi .Close() từ luồng khác
                    clientSocket = listenerSocket.Accept();
                    AppendTextSafe($"Client mới đã kết nối từ: {clientSocket.RemoteEndPoint}\n");

                    while (clientSocket.Connected)
                    {
                        try
                        {
                            bytesReceived = clientSocket.Receive(recvBuffer);
                            if (bytesReceived == 0)
                            {
                                AppendTextSafe($"Client {clientSocket.RemoteEndPoint} đã ngắt kết nối.\n");
                                clientSocket.Close();
                                break;
                            }
                            string text = Encoding.UTF8.GetString(recvBuffer, 0, bytesReceived);
                            AppendTextSafe($"Client: {text}\n");
                        }
                        catch (SocketException sx_inner)
                        {
                            // Lỗi này xảy ra nếu client bị ngắt đột ngột
                            AppendTextSafe($"Lỗi kết nối client: {sx_inner.Message}\n");
                            clientSocket.Close();
                            break;
                        }
                    }
                }
            }
            catch (SocketException sx)
            {
                // Lỗi 10004 (Interrupted) là mã lỗi Windows trả về khi
                // chúng ta gọi .Close() từ luồng khác (nút Reset) trong khi luồng này
                // đang bị chặn tại .Accept(). Đây là hành vi mong muốn.
                if (sx.SocketErrorCode == SocketError.Interrupted)
                {
                    // Thay vì báo lỗi, chúng ta dọn dẹp log và thông báo dừng
                    ClearRichTextBoxSafe();
                    AppendTextSafe("Server đã dừng.\n");
                }
                else
                {
                    // Nếu là một lỗi SocketException khác, thì đó mới là lỗi thật
                    AppendTextSafe($"Lỗi Socket Server: {sx.Message}\n");
                }
            }
            catch (Exception ex)
            {
                AppendTextSafe($"Lỗi Server không xác định: {ex.Message}\n");
            }
            finally
            {
                // Dọn dẹp tài nguyên
                if (listenerSocket != null)
                {
                    listenerSocket.Close();
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread serverThread = new Thread(new ThreadStart(StartListenerThread));
            serverThread.IsBackground = true;
            serverThread.Start();

            button1.Enabled = false;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (listenerSocket != null)
            {
                listenerSocket.Close();
            }
        }
    }
}
