namespace LAB3_BAI3
{
    partial class Client
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            richTextBox1 = new RichTextBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Font = new Font("Cascadia Code SemiBold", 12F, FontStyle.Bold);
            richTextBox1.ForeColor = Color.DarkRed;
            richTextBox1.Location = new Point(89, 65);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(514, 162);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // button1
            // 
            button1.Font = new Font("Cascadia Code SemiBold", 12F, FontStyle.Bold);
            button1.ForeColor = Color.DarkRed;
            button1.Location = new Point(648, 65);
            button1.Name = "button1";
            button1.Size = new Size(150, 50);
            button1.TabIndex = 1;
            button1.Text = "Connect";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Cascadia Code SemiBold", 12F, FontStyle.Bold);
            button2.ForeColor = Color.DarkRed;
            button2.Location = new Point(648, 121);
            button2.Name = "button2";
            button2.Size = new Size(150, 50);
            button2.TabIndex = 2;
            button2.Text = "Send";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Font = new Font("Cascadia Code SemiBold", 12F, FontStyle.Bold);
            button3.ForeColor = Color.DarkRed;
            button3.Location = new Point(648, 177);
            button3.Name = "button3";
            button3.Size = new Size(150, 50);
            button3.TabIndex = 3;
            button3.Text = "Disconnect";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Client
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FloralWhite;
            ClientSize = new Size(878, 299);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(richTextBox1);
            Name = "Client";
            Text = "Client";
            Load += Client_Load;
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox1;
        private Button button1;
        private Button button2;
        private Button button3;
    }
}