namespace LAB3_BAI4
{
    partial class SERVER
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
            richTextBox2 = new RichTextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Font = new Font("Cascadia Code", 10.8F);
            richTextBox1.ForeColor = Color.DarkRed;
            richTextBox1.Location = new Point(12, 77);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(556, 391);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // button1
            // 
            button1.Font = new Font("Cascadia Code", 10.8F);
            button1.ForeColor = Color.DarkRed;
            button1.Location = new Point(468, 21);
            button1.Name = "button1";
            button1.Size = new Size(100, 50);
            button1.TabIndex = 1;
            button1.Text = "START";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Cascadia Code", 10.8F);
            button2.ForeColor = Color.DarkRed;
            button2.Location = new Point(12, 21);
            button2.Name = "button2";
            button2.Size = new Size(192, 50);
            button2.TabIndex = 2;
            button2.Text = "UPDATE FILE";
            button2.UseVisualStyleBackColor = true;
            // 
            // richTextBox2
            // 
            richTextBox2.Font = new Font("Cascadia Code", 10.8F);
            richTextBox2.ForeColor = Color.DarkRed;
            richTextBox2.Location = new Point(574, 77);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(340, 391);
            richTextBox2.TabIndex = 3;
            richTextBox2.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Cascadia Code", 10.8F);
            label1.ForeColor = Color.DarkRed;
            label1.Location = new Point(574, 50);
            label1.Name = "label1";
            label1.Size = new Size(43, 24);
            label1.TabIndex = 4;
            label1.Text = "LOG";
            // 
            // SERVER
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FloralWhite;
            ClientSize = new Size(923, 480);
            Controls.Add(label1);
            Controls.Add(richTextBox2);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(richTextBox1);
            Name = "SERVER";
            Text = "SERVER";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox richTextBox1;
        private Button button1;
        private Button button2;
        private RichTextBox richTextBox2;
        private Label label1;
    }
}