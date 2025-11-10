using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LAB3_BAI4.SERVER;

namespace LAB3_BAI4
{
    public partial class CLIENT : Form
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _reader;
        private StreamWriter _writer;

        private List<Movie> _movies = new List<Movie>();
        private int _currentIndex = 0;
        private bool _isConnected = false;

        public CLIENT()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; // Cho phép update UI từ luồng khác (dùng cho bài lab đơn giản)

            // Đăng ký thêm sự kiện nếu Designer chưa tự động link (dựa vào file designer bạn gửi chỉ thấy button1 có sự kiện)
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button3.Click += new System.EventHandler(this.button3_Click);
            this.button4.Click += new System.EventHandler(this.button4_Click);
        }

        // Nút Connect (button1)
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                _client = new TcpClient();
                // Lưu ý: Nếu server chạy khác máy thì thay "127.0.0.1" bằng IP server
                _client.Connect("127.0.0.1", 8888);
                _stream = _client.GetStream();
                _reader = new StreamReader(_stream, Encoding.UTF8);
                _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };

                _isConnected = true;
                button1.Enabled = false;
                button1.Text = "Connected";

                // Bắt đầu luồng nhận dữ liệu
                Thread receiveThread = new Thread(ReceiveData);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối server: " + ex.Message);
            }
        }

        // Luồng nhận dữ liệu từ Server
        private void ReceiveData()
        {
            try
            {
                string line;
                while (_isConnected && (line = _reader.ReadLine()) != null)
                {
                    if (line.StartsWith("DATA|"))
                    {
                        string json = line.Substring(5);
                        // Deserialize JSON thành danh sách phim
                        _movies = JsonSerializer.Deserialize<List<Movie>>(json);
                        // Cập nhật giao diện
                        Invoke(new Action(() => DisplayCurrentMovie()));
                    }
                    else if (line.StartsWith("SUCCESS|"))
                    {
                        MessageBox.Show(line.Substring(8), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (line.StartsWith("ERROR|"))
                    {
                        MessageBox.Show(line.Substring(6), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // Load lại để reset các ghế chọn sai
                        Invoke(new Action(() => DisplayCurrentMovie()));
                    }
                }
            }
            catch
            {
                if (_isConnected) MessageBox.Show("Mất kết nối tới server.");
                _isConnected = false;
                // Reset trạng thái nút connect nếu bị ngắt
                if (button1.InvokeRequired)
                    button1.Invoke(new Action(() => { button1.Enabled = true; button1.Text = "Connect"; }));
                else
                {
                    button1.Enabled = true;
                    button1.Text = "Connect";
                }
            }
        }

        // Hiển thị thông tin phim hiện tại lên UI
        private void DisplayCurrentMovie()
        {
            if (_movies == null || _movies.Count == 0) return;

            // Đảm bảo index hợp lệ
            if (_currentIndex < 0) _currentIndex = 0;
            if (_currentIndex >= _movies.Count) _currentIndex = _movies.Count - 1;

            Movie m = _movies[_currentIndex];

            // Cập nhật các TextBox theo designer của bạn
            textBox1.Text = m.Name;
            textBox2.Text = m.Price.ToString("N0"); // Định dạng số có dấu phẩy
            textBox3.Text = m.TicketsSold.ToString();
            label13.Text = _currentIndex.ToString(); // Hiển thị chỉ số phim hiện tại

            // Cập nhật trạng thái ghế (CheckBox)
            // Duyệt tất cả control trên Form để tìm các CheckBox ghế (A1, A2...)
            foreach (Control c in this.Controls)
            {
                if (c is CheckBox cb)
                {
                    // Kiểm tra xem có phải là checkbox ghế không (tên dài 2 ký tự, bắt đầu bằng A,B,C)
                    if (cb.Name.Length == 2 && "ABC".Contains(cb.Name[0].ToString()))
                    {
                        // Reset trạng thái cũ
                        cb.Enabled = true;
                        cb.Checked = false;
                        cb.BackColor = SystemColors.Control; // Màu nền mặc định

                        // Nếu ghế đã nằm trong danh sách đã đặt của phim hiện tại
                        if (m.BookedSeats.Contains(cb.Name))
                        {
                            cb.Checked = true;
                            cb.Enabled = false; // Không cho chọn lại
                            cb.BackColor = Color.Red; // Đánh dấu màu đỏ
                        }
                    }
                }
            }
        }

        // Nút "Trước" (button2)
        private void button2_Click(object sender, EventArgs e)
        {
            if (_movies.Count > 0)
            {
                _currentIndex--;
                if (_currentIndex < 0) _currentIndex = _movies.Count - 1; // Quay vòng về cuối
                DisplayCurrentMovie();
            }
        }

        // Nút "Sau" (button3)
        private void button3_Click(object sender, EventArgs e)
        {
            if (_movies.Count > 0)
            {
                _currentIndex++;
                if (_currentIndex >= _movies.Count) _currentIndex = 0; // Quay vòng về đầu
                DisplayCurrentMovie();
            }
        }

        // Nút "Đặt vé" (button4)
        private void button4_Click(object sender, EventArgs e)
        {
            if (!_isConnected)
            {
                MessageBox.Show("Chưa kết nối server!");
                return;
            }

            List<string> selectedSeats = new List<string>();

            // Tìm các ghế đang được tick chọn VÀ còn đang Enable (nghĩa là người dùng mới chọn)
            foreach (Control c in this.Controls)
            {
                if (c is CheckBox cb && cb.Checked && cb.Enabled)
                {
                    // Chỉ lấy các checkbox ghế (A1..C5)
                    if (cb.Name.Length == 2 && "ABC".Contains(cb.Name[0].ToString()))
                    {
                        selectedSeats.Add(cb.Name);
                    }
                }
            }

            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 ghế trống để đặt.");
                return;
            }

            // Gửi yêu cầu lên server: BOOK|Index|A1,A2...
            string request = $"BOOK|{_currentIndex}|{string.Join(",", selectedSeats)}";
            _writer.WriteLine(request);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!_isConnected)
            {
                MessageBox.Show("Chưa kết nối server!");
                return;
            }

            try
            {
                _writer.WriteLine("RELOAD");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gửi yêu cầu reload: " + ex.Message);
            }
        }
    }
}