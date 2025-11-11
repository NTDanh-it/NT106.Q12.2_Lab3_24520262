using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LAB3_BAI2
{
    public partial class Telnetlistener : Form
    {
        // Delegate để đảm bảo an toàn khi gọi hàm từ các Thread khác
        private delegate void SafeCallDelegate(string text);

        // Khai báo listenerSocket ở cấp độ class để có thể dừng nó nếu cần
        private Socket listenerSocket;

        public Telnetlistener()
        {
            InitializeComponent();
        }

        // Hàm an toàn để thêm text vào RichTextBox từ thread khác
        private void AppendTextSafe(string text)
        {
            // Kiểm tra xem luồng hiện tại có phải là luồng UI chính hay không
            if (this.richTextBox1.InvokeRequired)
            {
                // Nếu không phải, sử dụng Invoke để chuyển giao tác vụ sang luồng UI
                this.richTextBox1.Invoke(new SafeCallDelegate(AppendTextSafe), text);
            }
            else
            {
                // Nếu đã là luồng UI chính, thực hiện cập nhật UI
                this.richTextBox1.AppendText(text);
                // Tự động cuộn xuống dưới
                this.richTextBox1.ScrollToCaret();
            }
        }

        // Hàm chạy trong Thread nền để khởi tạo và lắng nghe kết nối
        void StartListenerThread()
        {
            try
            {
                // Dùng try-catch bao quanh toàn bộ logic để xử lý ngoại lệ khi đóng/mất kết nối

                int bytesReceived;
                // Đổi kích thước buffer nhận lên 1024 bytes
                byte[] recvBuffer = new byte[1024];
                Socket clientSocket;

                // TẠO VÀ LẮNG NGHE SOCKET
                listenerSocket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp
                );

                IPEndPoint ipepServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

                // Gắn socket vào địa chỉ và port
                listenerSocket.Bind(ipepServer);
                // Bắt đầu lắng nghe, với hàng đợi 10 kết nối
                listenerSocket.Listen(10);

                AppendTextSafe("Server đang lắng nghe trên cổng 8080...\n");

                // Lặp vô hạn để chấp nhận nhiều kết nối lần lượt
                while (true)
                {
                    // CHẤP NHẬN KẾT NỐI (Blocking call)
                    clientSocket = listenerSocket.Accept();
                    AppendTextSafe($"Client mới đã kết nối từ: {clientSocket.RemoteEndPoint}\n");

                    // NHẬN DỮ LIỆU
                    while (clientSocket.Connected)
                    {
                        try
                        {
                            // Nhận dữ liệu
                            bytesReceived = clientSocket.Receive(recvBuffer);

                            // Kiểm tra nếu client đóng kết nối (bytesReceived = 0)
                            if (bytesReceived == 0)
                            {
                                AppendTextSafe($"Client {clientSocket.RemoteEndPoint} đã ngắt kết nối.\n");
                                clientSocket.Close();
                                break;
                            }

                            // Sử dụng Encoding.UTF8 để giải mã dữ liệu, hỗ trợ tiếng Việt
                            string text = Encoding.UTF8.GetString(recvBuffer, 0, bytesReceived);

                            AppendTextSafe($"Client: {text}\n");
                        }
                        catch (SocketException sex)
                        {
                            // Xử lý lỗi Socket (ví dụ: client bị đóng đột ngột)
                            AppendTextSafe($"Lỗi kết nối client: {sex.Message}\n");
                            clientSocket.Close();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ tổng quát (ví dụ: lỗi Bind, lỗi trong Accept khi Listener bị Stop)
                AppendTextSafe($"Lỗi Server: {ex.Message}\n");
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

        // XỬ LÝ SỰ KIỆN UI

        // Sự kiện Click cho nút "Listen" (Giả sử tên là button1)
        private void button1_Click(object sender, EventArgs e)
        {
            // Khởi tạo và chạy luồng Server
            Thread serverThread = new Thread(new ThreadStart(StartListenerThread));
            serverThread.IsBackground = true; // Cho phép thread nền tự dừng khi ứng dụng đóng
            serverThread.Start();

            // vô hiệu hóa nút listen tránh lỗi
            button1.Enabled = false;

        }
        // Sự kiện FormClosing để đảm bảo đóng Listener khi ứng dụng tắt
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (listenerSocket != null)
            {
                // Đóng Listener để giải phóng cổng và dừng luồng Accept
                listenerSocket.Close();
            }
        }
    }
}