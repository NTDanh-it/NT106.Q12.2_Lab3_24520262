using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LAB3_BAI5
{
    public partial class CLIENT : Form
    {
        private Socket clientSocket;
        private const int BUFFER_SIZE = 1024 * 5000;
        private byte[] buffer = new byte[BUFFER_SIZE];
        private Thread receiveThread;

        public CLIENT()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(IPAddress.Loopback, 8080);
                MessageBox.Show("Đã kết nối tới Server!");
                button4.Enabled = false;

                receiveThread = new Thread(ReceiveLoop);
                receiveThread.IsBackground = true;
                receiveThread.Start();

                SendString("GET_LIST");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    // Lọc các định dạng ảnh phổ biến
                    ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                    ofd.Title = "Chọn hình ảnh món ăn";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        textBox3.Text = ofd.FileName;

                        // CÁCH SIÊU AN TOÀN:
                        // Đọc bytes vào bộ nhớ
                        byte[] imageBytes = File.ReadAllBytes(ofd.FileName);
                        // Tạo MemoryStream
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            // Tạo Image tạm từ stream
                            using (Image tempImage = Image.FromStream(ms))
                            {
                                // Tạo một Bitmap MỚI HOÀN TOÀN từ Image tạm.
                                // Điều này giúp loại bỏ các metadata lỗi và ngắt hoàn toàn kết nối với file gốc.
                                pictureBox1.Image = new Bitmap(tempImage);
                            }
                        }
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể mở hình ảnh này.\nHãy thử chọn một hình khác (ví dụ: .jpg hoặc .png chuẩn).\nLỗi chi tiết: {ex.Message}", "Lỗi định dạng ảnh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox3.Clear();
                pictureBox1.Image = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (clientSocket == null || !clientSocket.Connected)
            {
                MessageBox.Show("Chưa kết nối Server!");
                return;
            }
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin và chọn hình ảnh.");
                return;
            }

            try
            {
                byte[] imageBytes = File.ReadAllBytes(textBox3.Text);
                string base64Image = Convert.ToBase64String(imageBytes);
                string request = $"ADD|{textBox1.Text}|{textBox2.Text}|{base64Image}";
                SendString(request);

                MessageBox.Show("Đã gửi món ăn lên Server!");
                textBox1.Clear();
                textBox3.Clear();
                pictureBox1.Image = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gửi: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (clientSocket != null && clientSocket.Connected)
            {
                SendString("SEARCH");
            }
            else
            {
                MessageBox.Show("Chưa kết nối Server!");
            }
        }

        private void SendString(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            clientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, null, null);
        }

        private void ReceiveLoop()
        {
            while (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    int received = clientSocket.Receive(buffer);
                    if (received == 0) break;

                    byte[] data = new byte[received];
                    Array.Copy(buffer, data, received);
                    string message = Encoding.UTF8.GetString(data);

                    this.Invoke(new Action(() => ProcessServerResponse(message)));
                }
                catch
                {
                    break;
                }
            }
        }

        private void ProcessServerResponse(string message)
        {
            string[] parts = message.Split('|');
            string command = parts[0];

            switch (command)
            {
                case "RANDOM_RES":
                    if (parts.Length >= 4)
                    {
                        richTextBox1.Text = $"Món hôm nay: {parts[1]}\nĐóng góp bởi: {parts[2]}";
                        try
                        {
                            byte[] imageBytes = Convert.FromBase64String(parts[3]);
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                pictureBox1.Image = Image.FromStream(ms);
                                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                        }
                        catch { }
                    }
                    break;

                case "LIST_RES":
                    UpdateTableLayoutPanel(parts);
                    break;
            }
        }

        private void UpdateTableLayoutPanel(string[] dataParts)
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowCount = 1;

            Font customFont = new Font("Cascadia Code", 12, FontStyle.Regular);
            Color customColor = Color.DarkRed;

            for (int i = 1; i < dataParts.Length; i++)
            {
                string[] item = dataParts[i].Split('~');
                if (item.Length >= 3)
                {
                    tableLayoutPanel1.RowCount++;
                    int currentRow = tableLayoutPanel1.RowCount - 1;

                    Label lblId = new Label() { Text = item[0], AutoSize = true, Font = customFont, ForeColor = customColor };
                    Label lblTenMon = new Label() { Text = item[1], AutoSize = true, Font = customFont, ForeColor = customColor };
                    Label lblNguoiDongGop = new Label() { Text = item[2], AutoSize = true, Font = customFont, ForeColor = customColor };

                    tableLayoutPanel1.Controls.Add(lblId, 0, currentRow);
                    tableLayoutPanel1.Controls.Add(lblTenMon, 1, currentRow);
                    tableLayoutPanel1.Controls.Add(lblNguoiDongGop, 2, currentRow);
                }
            }
        }
    }
}