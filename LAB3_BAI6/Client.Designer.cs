namespace LAB3_BAI6
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            button1 = new Button();
            button2 = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            listBox1 = new ListBox();
            button3 = new Button();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Font = new Font("Cascadia Code SemiBold", 10.2F, FontStyle.Bold);
            richTextBox1.ForeColor = Color.DarkRed;
            richTextBox1.Location = new Point(12, 12);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(557, 257);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Cascadia Code SemiBold", 10.2F, FontStyle.Bold);
            label1.ForeColor = Color.DarkRed;
            label1.Location = new Point(595, 12);
            label1.Name = "label1";
            label1.Size = new Size(120, 22);
            label1.TabIndex = 2;
            label1.Text = "PARTICIPANT";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Cascadia Code SemiBold", 10.2F, FontStyle.Bold);
            label2.ForeColor = Color.DarkRed;
            label2.Location = new Point(12, 268);
            label2.Name = "label2";
            label2.Size = new Size(100, 22);
            label2.TabIndex = 3;
            label2.Text = "Your name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Cascadia Code SemiBold", 10.2F, FontStyle.Bold);
            label3.ForeColor = Color.DarkRed;
            label3.Location = new Point(12, 361);
            label3.Name = "label3";
            label3.Size = new Size(80, 22);
            label3.TabIndex = 4;
            label3.Text = "Message";
            // 
            // button1
            // 
            button1.Font = new Font("Cascadia Code SemiBold", 10.2F, FontStyle.Bold);
            button1.ForeColor = Color.DarkRed;
            button1.Location = new Point(405, 291);
            button1.Name = "button1";
            button1.Size = new Size(164, 29);
            button1.TabIndex = 5;
            button1.Text = "Connect";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Cascadia Code SemiBold", 10.2F, FontStyle.Bold);
            button2.ForeColor = Color.DarkRed;
            button2.Location = new Point(575, 382);
            button2.Name = "button2";
            button2.Size = new Size(164, 29);
            button2.TabIndex = 6;
            button2.Text = "Send";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Cascadia Code SemiBold", 10.2F, FontStyle.Bold);
            textBox1.ForeColor = Color.DarkRed;
            textBox1.Location = new Point(12, 291);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(350, 27);
            textBox1.TabIndex = 7;
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Cascadia Code SemiBold", 10.2F, FontStyle.Bold);
            textBox2.ForeColor = Color.DarkRed;
            textBox2.Location = new Point(12, 384);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(557, 27);
            textBox2.TabIndex = 8;
            // 
            // listBox1
            // 
            listBox1.Font = new Font("Cascadia Code", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            listBox1.ForeColor = Color.DarkRed;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 22;
            listBox1.Location = new Point(595, 42);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(123, 290);
            listBox1.TabIndex = 9;
            listBox1.MouseDoubleClick += listBox1_MouseDoubleClick;
            // 
            // button3
            // 
            button3.Font = new Font("Cascadia Code SemiBold", 10.2F, FontStyle.Bold);
            button3.ForeColor = Color.DarkRed;
            button3.Location = new Point(405, 337);
            button3.Name = "button3";
            button3.Size = new Size(164, 29);
            button3.TabIndex = 10;
            button3.Text = "Disconnect";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Client
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FloralWhite;
            ClientSize = new Size(754, 429);
            Controls.Add(button3);
            Controls.Add(listBox1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(richTextBox1);
            Name = "Client";
            Text = "Client";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox richTextBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button button1;
        private Button button2;
        private TextBox textBox1;
        private TextBox textBox2;
        private ListBox listBox1;
        private Button button3;
    }
}