using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;         
using System.Linq;
using System.Net.Sockets; 
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB3_BAI6
{
    public partial class Client : Form
    {
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        private string myName = "";

        // Dictionary để quản lý các cửa sổ chat riêng
        private Dictionary<string, PRIVATE_MESSAGE> privateChatForms = new Dictionary<string, PRIVATE_MESSAGE>();

        public Client()
        {
            InitializeComponent();
            // Tắt các control cho đến khi kết nối
            textBox2.Enabled = false;
            button2.Enabled = false;
            listBox1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Vui lòng nhập tên của bạn.");
                return;
            }

            try
            {
                tcpClient = new TcpClient("127.0.0.1", 8080); // Kết nối tới localhost, port 8080
                networkStream = tcpClient.GetStream();
                myName = textBox1.Text.Trim();

                // Gửi tên của mình cho Server
                byte[] nameData = Encoding.UTF8.GetBytes(myName);
                networkStream.Write(nameData, 0, nameData.Length);

                // Bắt đầu một luồng riêng để nhận tin nhắn
                Thread receiveThread = new Thread(ReceiveMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();

                // Kích hoạt UI
                button1.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = true;
                button2.Enabled = true;
                listBox1.Enabled = true;
                this.Text = "Client - " + myName;
            }
            catch (SocketException)
            {
                UpdateChatHistory("SERVER_INFO|Không thể kết nối đến server.");
            }
        }

        // Luồng nhận tin nhắn
        private void ReceiveMessages()
        {
            byte[] buffer = new byte[4096];
            int bytesRead;

            try
            {
                while ((bytesRead = networkStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string rawMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    string[] parts = rawMessage.Split(new char[] { '|' }, 2);

                    if (parts.Length < 2) continue;

                    string command = parts[0];
                    string data = parts[1];

                    switch (command)
                    {
                        case "PUBLIC_MSG":
                        case "SERVER_INFO":
                            UpdateChatHistory(data);
                            break;

                        case "SERVER_ERROR":
                            MessageBox.Show(data, "Lỗi từ Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Invoke(new Action(() => CloseConnection()));
                            break;

                        case "PARTICIPANT_LIST":
                            UpdateParticipantList(data); // SỬA LẠI: Bỏ comment
                            break;

                        // THÊM LẠI: Xử lý tin nhắn riêng
                        case "PRIVATE_MSG":
                            string[] pmParts = data.Split(new char[] { ':' }, 2);
                            if (pmParts.Length == 2)
                            {
                                string senderName = pmParts[0].Trim();
                                string messageContent = pmParts[1].Trim();

                                // Xử lý trường hợp nhận lại tin mình gửi
                                if (senderName.StartsWith("Me (to"))
                                {
                                    // "Me (to RecipientName)"
                                    string recipientName = senderName.Substring(senderName.IndexOf('(') + 4, senderName.IndexOf(')') - senderName.IndexOf('(') - 4).Trim();
                                    HandleReceivedPrivateMessage(recipientName, $"Me: {messageContent}", true);
                                }
                                else
                                {
                                    HandleReceivedPrivateMessage(senderName, $"{senderName}: {messageContent}", false);
                                }
                            }
                            break;
                    }
                }
            }
            catch (IOException)
            {
                UpdateChatHistory("SERVER_INFO|Mất kết nối đến server.");
            }
            finally
            {
                CloseConnection();
            }
        }

        // Nút Send (button2 - Gửi tin nhắn chung)
        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                string message = $"PUBLIC_MSG|{textBox2.Text}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                networkStream.Write(data, 0, data.Length);
                textBox2.Clear();
            }
        }
        // Xử lý khi double-click vào listBox1
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string recipientName = listBox1.SelectedItem.ToString();
                if (recipientName != myName)
                {
                    OpenPrivateChat(recipientName);
                }
            }
        }

        // Hàm mở cửa sổ chat riêng (hoặc kích hoạt nếu đã mở)
        private void OpenPrivateChat(string recipientName)
        {
            if (privateChatForms.ContainsKey(recipientName))
            {
                // Nếu đã mở, chỉ cần kích hoạt nó
                privateChatForms[recipientName].Activate();
            }
            else
            {
                // Nếu chưa mở, tạo form mới (đảm bảo bạn có file PrivateMessage.cs)
                PRIVATE_MESSAGE pmForm = new PRIVATE_MESSAGE(recipientName, myName, networkStream);
                pmForm.FormClosed += (s, args) => privateChatForms.Remove(recipientName); // Xóa khỏi dictionary khi đóng
                privateChatForms.Add(recipientName, pmForm);
                pmForm.Show();
            }
        }

        // Hàm xử lý khi nhận được tin nhắn riêng
        private void HandleReceivedPrivateMessage(string partnerName, string message, bool isMyEcho)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => HandleReceivedPrivateMessage(partnerName, message, isMyEcho)));
            }
            else
            {
                // Mở cửa sổ chat nếu chưa có
                if (!privateChatForms.ContainsKey(partnerName))
                {
                    // Chỉ tự động mở khi đó là tin nhắn từ người khác (không phải tin echo của mình)
                    if (!isMyEcho)
                    {
                        OpenPrivateChat(partnerName);
                    }
                }

                // Thêm tin nhắn vào cửa sổ
                if (privateChatForms.ContainsKey(partnerName))
                {
                    privateChatForms[partnerName].AddMessage(message);
                }
            }
        }
        private void UpdateChatHistory(string message)
        {
            // Dùng richTextBox1
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action(() => UpdateChatHistory(message)));
            }
            else
            {
                richTextBox1.AppendText(message + "\n");
                richTextBox1.ScrollToCaret();
            }
        }

        private void UpdateParticipantList(string data)
        {
            // Dùng listBox1
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new Action(() => UpdateParticipantList(data)));
            }
            else
            {
                listBox1.Items.Clear();
                string[] names = data.Split(',');
                foreach (string name in names)
                {
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        listBox1.Items.Add(name);
                    }
                }
            }
        }

        private void CloseConnection()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(CloseConnection));
                return;
            }

            networkStream?.Close();
            tcpClient?.Close();

            // Đóng tất cả cửa sổ chat riêng
            // Thêm .ToList() để tránh lỗi "Collection was modified"
            foreach (var form in privateChatForms.Values.ToList())
            {
                form.Close();
            }
            privateChatForms.Clear();

            // Reset UI
            button1.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = false;
            button2.Enabled = false;

            // Bỏ comment
            listBox1.Enabled = false;
            listBox1.Items.Clear();
            this.Text = "Client (Disconnected)";
        }

        // Đảm bảo đóng kết nối khi tắt form
        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseConnection();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CloseConnection();
        }
    }
}