using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LAB3_BAI6
{
    public partial class SERVER : Form
    {
        private TcpListener tcpListener;
        // Dùng Dictionary để lưu Socket và tên của Client
        private Dictionary<Socket, string> clientNames = new Dictionary<Socket, string>();

        // Một List<Socket> để dễ dàng duyệt qua các client
        private List<Socket> clientSockets = new List<Socket>();

        public SERVER()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;

            tcpListener = new TcpListener(IPAddress.Any, 8080);
            tcpListener.Start();

            Thread acceptThread = new Thread(new ThreadStart(AcceptClients));
            acceptThread.Start();

            Log("Server started! Listening on port 8080...");
            button1.Enabled = false;
        }

        private void AcceptClients()
        {
            while (true)
            {
                Socket clientSocket = tcpListener.AcceptSocket();
                clientSockets.Add(clientSocket);
                Log("New client connected from " + clientSocket.RemoteEndPoint.ToString());

                Thread receiveThread = new Thread(() => ReceiveMessages(clientSocket));
                receiveThread.Start();
            }
        }

        private void ReceiveMessages(Socket clientSocket)
        {
            string name = "A client";
            try
            {
                // Nhận tên của Client
                byte[] nameBuffer = new byte[1024];
                int receivedNameBytes = clientSocket.Receive(nameBuffer);
                name = Encoding.UTF8.GetString(nameBuffer, 0, receivedNameBytes).Trim();

                // Kiểm tra tên trùng lặp
                if (clientNames.ContainsValue(name))
                {
                    Log($"Client {name} tried to connect but name is taken. Disconnecting.");
                    byte[] errorMsg = Encoding.UTF8.GetBytes("SERVER_ERROR|Tên đã được sử dụng. Vui lòng chọn tên khác.");
                    clientSocket.Send(errorMsg);
                    clientSockets.Remove(clientSocket);
                    clientSocket.Close();
                    return;
                }

                clientNames[clientSocket] = name;

                // Gửi thông báo cho tất cả mọi người
                BroadcastMessage($"SERVER_INFO|{name} joined the chat.", clientSocket);
                Log($"{name} joined the chat.");

                // Cập nhật danh sách người tham gia cho MỌI NGƯỜI
                UpdateParticipantList();

                // Vòng lặp nhận tin nhắn
                while (clientSocket.Connected)
                {
                    byte[] buffer = new byte[4096];
                    int receivedBytes = clientSocket.Receive(buffer);
                    if (receivedBytes == 0) break;

                    string rawMessage = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                    string[] parts = rawMessage.Split(new char[] { '|' }, 3);

                    // Phân tích giao thức
                    if (parts.Length > 0)
                    {
                        switch (parts[0])
                        {
                            case "PUBLIC_MSG":
                                if (parts.Length >= 2)
                                {
                                    string publicMsg = $"PUBLIC_MSG|{name}: {parts[1]}";
                                    Log($"{name} (public): {parts[1]}");
                                    BroadcastMessage(publicMsg, null); // Gửi cho tất cả
                                }
                                break;

                            case "PRIVATE_MSG":
                                if (parts.Length >= 3)
                                {
                                    string recipientName = parts[1];
                                    string privateMsgContent = parts[2];
                                    string fullPrivateMsg = $"PRIVATE_MSG|{name}: {privateMsgContent}";
                                    Log($"{name} (private to {recipientName}): {privateMsgContent}");

                                    // Gửi tin nhắn riêng
                                    SendPrivateMessage(fullPrivateMsg, recipientName, name);
                                }
                                break;
                        }
                    }
                }
            }
            catch (SocketException)
            {
                Log($"Client {name} disconnected unexpectedly.");
            }
            catch (Exception ex)
            {
                Log($"Error with client {name}: {ex.Message}");
            }

            // Xử lý khi client ngắt kết nối
            clientSockets.Remove(clientSocket);
            if (clientNames.ContainsKey(clientSocket))
            {
                clientNames.Remove(clientSocket);
            }
            clientSocket.Close();

            Log($"{name} left the group.");
            BroadcastMessage($"SERVER_INFO|{name} left the group.", null);
            UpdateParticipantList();
        }

        // Gửi tin nhắn cho tất cả client (hoặc tất cả TRỪ một client)
        private void BroadcastMessage(string message, Socket excludeClient)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach (Socket client in clientSockets)
            {
                if (client != excludeClient)
                {
                    try
                    {
                        client.Send(data);
                    }
                    catch (SocketException)
                    {
                        // Xử lý nếu client đã ngắt kết nối
                    }
                }
            }
        }

        // Gửi tin nhắn riêng
        private void SendPrivateMessage(string message, string recipientName, string senderName)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);

            // Tìm socket của người nhận và người gửi
            Socket recipientSocket = clientNames.FirstOrDefault(kv => kv.Value == recipientName).Key;
            Socket senderSocket = clientNames.FirstOrDefault(kv => kv.Value == senderName).Key;

            if (recipientSocket != null)
            {
                try
                {
                    recipientSocket.Send(data);
                }
                catch (SocketException) {}
            }

            // Gửi lại cho chính người gửi để xác nhận (trừ khi họ tự gửi cho mình)
            if (senderSocket != null && recipientSocket != senderSocket)
            {
                try
                {
                    // Đổi "SenderName: " thành "Me (to RecipientName): "
                    string echoMsg = message.Replace($"PRIVATE_MSG|{senderName}:",
                        $"PRIVATE_MSG|Me (to {recipientName}):");
                    byte[] echoData = Encoding.UTF8.GetBytes(echoMsg);
                    senderSocket.Send(echoData);
                }
                catch (SocketException) {}
            }
        }


        // Cập nhật danh sách người tham gia
        private void UpdateParticipantList()
        {
            // "Name1,Name2,Name3"
            string participantNames = string.Join(",", clientNames.Values);
            string message = $"PARTICIPANT_LIST|{participantNames}";
            BroadcastMessage(message, null);
        }

        // Ghi log lên RichTextBox
        private void Log(string message)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action(() => Log(message)));
            }
            else
            {
                richTextBox1.AppendText(DateTime.Now.ToString("HH:mm:ss") + ": " + message + "\n");
                richTextBox1.ScrollToCaret();
            }
        }
    }
}