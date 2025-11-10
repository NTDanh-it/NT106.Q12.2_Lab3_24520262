using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB3_BAI4
{

    public partial class SERVER : Form
    {
        public class Movie
        {
            public string Name { get; set; }
            public long Price { get; set; }
            public int TicketsSold { get; set; }
            public List<string> BookedSeats { get; set; } = new List<string>();

            // Helper để chuyển đổi danh sách ghế thành chuỗi "A1, B2" cho việc lưu file
            public string GetBookedSeatsString()
            {
                return string.Join(", ", BookedSeats);
            }
        }

        private TcpListener _listener;
        private bool _isRunning = false;
        private List<Movie> _movies = new List<Movie>();
        private List<TcpClient> _connectedClients = new List<TcpClient>();
        // Sửa đường dẫn này thành đường dẫn thật của bạn nếu cần, hoặc để relative nếu file nằm cùng thư mục exe
        private string _filePath = @"INPUT.txt";

        public SERVER()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; // Đơn giản hóa việc update UI từ thread khác (chỉ dùng cho bài lab đơn giản)
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            // Tự động tìm file INPUT.txt ở thư mục chạy nếu đường dẫn cứng sai
            if (!File.Exists(_filePath))
            {
                _filePath = Path.Combine(Application.StartupPath, "INPUT.txt");
            }
        }

        // 1. Xử lý nút START
        private void button1_Click(object sender, EventArgs e)
        {
            if (_isRunning) return;

            try
            {
                LoadDataFromFile();
                StartServer();
                button1.Enabled = false;
                Log("Server đã khởi động...");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi động: " + ex.Message);
            }
        }

        // Đọc file INPUT.txt
        private void LoadDataFromFile()
        {
            if (!File.Exists(_filePath))
            {
                MessageBox.Show($"Không tìm thấy file tại: {_filePath}");
                return;
            }

            // Hiển thị nội dung gốc lên RichTextBox trái
            richTextBox1.Text = File.ReadAllText(_filePath);

            _movies.Clear();
            string[] lines = File.ReadAllLines(_filePath);
            for (int i = 0; i < lines.Length; i += 4)
            {
                if (i + 3 >= lines.Length) break;
                Movie movie = new Movie
                {
                    Name = lines[i].Trim(),
                    Price = long.Parse(lines[i + 1].Trim()),
                    TicketsSold = int.Parse(lines[i + 2].Trim()),
                    BookedSeats = lines[i + 3].Trim().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                              .Select(s => s.Trim()).ToList()
                };
                _movies.Add(movie);
            }
        }

        // Lưu lại file INPUT.txt
        private void SaveDataToFile()
        {
            // Dùng lock để tránh xung đột khi nhiều client cùng đặt vé
            lock (_filePath)
            {
                using (StreamWriter sw = new StreamWriter(_filePath))
                {
                    foreach (var movie in _movies)
                    {
                        sw.WriteLine(movie.Name);
                        sw.WriteLine(movie.Price);
                        sw.WriteLine(movie.TicketsSold);
                        sw.WriteLine(movie.GetBookedSeatsString());
                    }
                }
            }
            // Cập nhật lại giao diện hiển thị file
            Invoke(new Action(() => {
                richTextBox1.Text = File.ReadAllText(_filePath);
            }));
        }

        private void StartServer()
        {
            _listener = new TcpListener(IPAddress.Any, 8888);
            _listener.Start();
            _isRunning = true;
            Task.Run(() => ListenForClients());
        }

        private async void ListenForClients()
        {
            while (_isRunning)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                _connectedClients.Add(client);
                Log($"Client mới kết nối từ: {client.Client.RemoteEndPoint}");

                // Gửi dữ liệu phim hiện tại ngay khi client kết nối
                BroadcastDataToClient(client);

                // Tạo luồng riêng để xử lý client này
                Task.Run(() => HandleClient(client));
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            try
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Giao thức: BOOK|<MovieIndex>|<Seat1,Seat2,...>
                    if (line.StartsWith("BOOK|"))
                    {
                        ProcessBooking(line, client);
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Client ngắt kết nối: {client.Client.RemoteEndPoint} ({ex.Message})");
            }
            finally
            {
                _connectedClients.Remove(client);
                client.Close();
            }
        }

        // Xử lý đặt vé
        private void ProcessBooking(string request, TcpClient senderClient)
        {
            // request format: BOOK|0|A1,A5
            string[] parts = request.Split('|');
            int movieIndex = int.Parse(parts[1]);
            string[] seatsToBook = parts[2].Split(',');

            lock (_movies) // Khóa dữ liệu để xử lý đồng bộ
            {
                Movie movie = _movies[movieIndex];
                List<string> newlyBooked = new List<string>();
                bool allSuccess = true;

                foreach (string seat in seatsToBook)
                {
                    if (movie.BookedSeats.Contains(seat))
                    {
                        allSuccess = false; // Có ghế đã bị người khác nhanh tay đặt trước
                        break;
                    }
                    newlyBooked.Add(seat);
                }

                if (allSuccess && newlyBooked.Count > 0)
                {
                    // Cập nhật dữ liệu
                    movie.BookedSeats.AddRange(newlyBooked);
                    movie.TicketsSold += newlyBooked.Count;

                    // Lưu file & Log
                    SaveDataToFile();
                    string logMsg = $"[{DateTime.Now:HH:mm:ss}] {senderClient.Client.RemoteEndPoint} đặt thành công phim '{movie.Name}': {string.Join(", ", newlyBooked)}";
                    Log(logMsg);

                    // Gửi thông báo thành công cho client đặt
                    SendToClient(senderClient, "SUCCESS|Đặt vé thành công!");
                    // Gửi dữ liệu mới nhất cho TẤT CẢ client để đồng bộ
                    BroadcastData();
                }
                else
                {
                    SendToClient(senderClient, "ERROR|Đặt vé thất bại. Một số ghế đã vừa được chọn bởi người khác.");
                }
            }
        }

        private void Log(string msg)
        {
            if (richTextBox2.InvokeRequired)
            {
                richTextBox2.Invoke(new Action(() => Log(msg)));
            }
            else
            {
                richTextBox2.AppendText(msg + Environment.NewLine);
                richTextBox2.ScrollToCaret();
            }
        }

        private void BroadcastData()
        {
            string jsonData = JsonSerializer.Serialize(_movies);
            string message = "DATA|" + jsonData;
            foreach (var client in _connectedClients.ToList())
            {
                if (client.Connected) SendToClient(client, message);
            }
        }

        private void BroadcastDataToClient(TcpClient client)
        {
            string jsonData = JsonSerializer.Serialize(_movies);
            SendToClient(client, "DATA|" + jsonData);
        }

        private void SendToClient(TcpClient client, string msg)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(msg + "\n");
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }
            catch {}
        }
    }
}