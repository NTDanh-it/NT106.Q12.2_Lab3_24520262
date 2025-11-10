namespace LAB3_BAI1
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            richTextBox1 = new RichTextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Cascadia Code", 12F);
            label1.ForeColor = Color.DarkRed;
            label1.Location = new Point(169, 63);
            label1.Name = "label1";
            label1.Size = new Size(204, 27);
            label1.TabIndex = 0;
            label1.Text = "IP Remote Host :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Cascadia Code", 12F);
            label2.ForeColor = Color.DarkRed;
            label2.Location = new Point(488, 63);
            label2.Name = "label2";
            label2.Size = new Size(84, 27);
            label2.TabIndex = 1;
            label2.Text = "Port :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Cascadia Code", 12F);
            label3.ForeColor = Color.DarkRed;
            label3.Location = new Point(169, 168);
            label3.Name = "label3";
            label3.Size = new Size(120, 27);
            label3.TabIndex = 2;
            label3.Text = "Message :";
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Cascadia Code", 12F);
            textBox1.ForeColor = Color.DarkRed;
            textBox1.Location = new Point(169, 93);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(298, 31);
            textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Cascadia Code", 12F);
            textBox2.ForeColor = Color.DarkRed;
            textBox2.Location = new Point(488, 93);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(125, 31);
            textBox2.TabIndex = 4;
            // 
            // richTextBox1
            // 
            richTextBox1.Font = new Font("Cascadia Code", 12F);
            richTextBox1.ForeColor = Color.DarkRed;
            richTextBox1.Location = new Point(169, 198);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(444, 169);
            richTextBox1.TabIndex = 5;
            richTextBox1.Text = "";
            // 
            // button1
            // 
            button1.Font = new Font("Cascadia Code", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.DarkRed;
            button1.Location = new Point(346, 373);
            button1.Name = "button1";
            button1.Size = new Size(100, 40);
            button1.TabIndex = 6;
            button1.Text = "SEND";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Client
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FloralWhite;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(richTextBox1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Client";
            Text = "Client";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBox1;
        private TextBox textBox2;
        private RichTextBox richTextBox1;
        private Button button1;
    }
}