using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data.SQLite;

namespace LAB3_BAI5
{
    public partial class SERVER : Form
    {
        private Socket serverSocket;
        private List<Socket> clientSockets = new List<Socket>();
        private const int BUFFER_SIZE = 1024 * 5000; // Tăng buffer để nhận hình ảnh (5MB)
        private byte[] buffer = new byte[BUFFER_SIZE];
        private string dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\thucdon.db");
        private string connectionString;

        public SERVER()
        {
            InitializeComponent();
            connectionString = $"Data Source={dbFile};Version=3;";
            InitializeDatabase();
        }
        private void InitializeDatabase()
        {
            if (!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "CREATE TABLE MonAn (Id INTEGER PRIMARY KEY AUTOINCREMENT, TenMon TEXT, NguoiDongGop TEXT, HinhAnh TEXT)";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                AppendLog("Đã tạo cơ sở dữ liệu mới.");
            }
        }

        private void AddMonAnToDB(string tenMon, string nguoiDongGop, string hinhAnhBase64)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO MonAn (TenMon, NguoiDongGop, HinhAnh) VALUES (@ten, @nguoi, @hinh)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ten", tenMon);
                    cmd.Parameters.AddWithValue("@nguoi", nguoiDongGop);
                    cmd.Parameters.AddWithValue("@hinh", hinhAnhBase64);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private string GetRandomMonAnFromDB()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                // Lấy ngẫu nhiên 1 món
                string sql = "SELECT * FROM MonAn ORDER BY RANDOM() LIMIT 1";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Format gửi về: RANDOM_RES|TenMon|NguoiDongGop|Base64Hinh
                            return $"RANDOM_RES|{reader["TenMon"]}|{reader["NguoiDongGop"]}|{reader["HinhAnh"]}";
                        }
                    }
                }
            }
            return "ERROR|Không tìm thấy món ăn nào.";
        }

        private string GetAllMonAnFromDB()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("LIST_RES");
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT Id, TenMon, NguoiDongGop FROM MonAn";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sb.Append($"|{reader["Id"]}~{reader["TenMon"]}~{reader["NguoiDongGop"]}");
                        }
                    }
                }
            }
            return sb.ToString();
        }

        // PHẦN SOCKET SERVER
        private void button1_Click(object sender, EventArgs e) // Nút START
        {
            button1.Enabled = false;
            button1.Text = "Server Running...";
            SetupServer();
        }

        private void SetupServer()
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8080));
                serverSocket.Listen(5);
                serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
                AppendLog("Server bắt đầu lắng nghe tại cổng 8080...");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo Server: " + ex.Message);
                button1.Enabled = true;
            }
        }

        private void AcceptCallback(IAsyncResult AR)
        {
            try
            {
                Socket socket = serverSocket.EndAccept(AR);
                clientSockets.Add(socket);
                AppendLog("Client đã kết nối: " + socket.RemoteEndPoint.ToString());

                // Tiếp tục lắng nghe client mới
                serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

                // Bắt đầu nhận dữ liệu từ client này
                socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            }
            catch { }
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            Socket currentSocket = (Socket)AR.AsyncState;
            try
            {
                int received = currentSocket.EndReceive(AR);
                if (received == 0) return;

                byte[] dataBuf = new byte[received];
                Array.Copy(buffer, dataBuf, received);
                string text = Encoding.UTF8.GetString(dataBuf);

                HandleClientRequest(currentSocket, text);

                // Tiếp tục nhận dữ liệu
                currentSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ReceiveCallback), currentSocket);
            }
            catch (SocketException)
            {
                currentSocket.Close();
                clientSockets.Remove(currentSocket);
                AppendLog("Client đã ngắt kết nối.");
            }
        }

        private void HandleClientRequest(Socket client, string request)
        {
            string[] parts = request.Split('|');
            string command = parts[0];

            switch (command)
            {
                case "ADD":
                    // Format: ADD|TenMon|NguoiDongGop|Base64Hinh
                    if (parts.Length >= 4)
                    {
                        AddMonAnToDB(parts[1], parts[2], parts[3]);
                        AppendLog($"Đã thêm món: {parts[1]} từ {parts[2]}");
                        BroadcastToAll(GetAllMonAnFromDB()); // Gửi lại danh sách mới cho tất cả client
                    }
                    break;
                case "SEARCH": // Yêu cầu lấy món ngẫu nhiên
                    string randomMon = GetRandomMonAnFromDB();
                    SendToClient(client, randomMon);
                    break;
                case "GET_LIST":
                    string list = GetAllMonAnFromDB();
                    SendToClient(client, list);
                    break;
            }
        }
        private void SendToClient(Socket client, string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            client.BeginSend(data, 0, data.Length, SocketFlags.None, null, null);
        }
        private void BroadcastToAll(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            foreach (Socket client in clientSockets)
            {
                if (client.Connected)
                {
                    client.BeginSend(data, 0, data.Length, SocketFlags.None, null, null);
                }
            }
        }
        private void AppendLog(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action<string>(AppendLog), text);
            }
            else
            {
                richTextBox1.AppendText(DateTime.Now.ToString("HH:mm:ss") + ": " + text + "\n");
                richTextBox1.ScrollToCaret();
            }
        }
    }
}