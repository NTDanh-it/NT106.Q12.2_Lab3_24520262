namespace LAB3_BAI5
{
    partial class CLIENT
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            richTextBox1 = new RichTextBox();
            pictureBox1 = new PictureBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            button2 = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label2 = new Label();
            label3 = new Label();
            button3 = new Button();
            button4 = new Button();
            textBox3 = new TextBox();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Font = new Font("Cascadia Code", 10.2F);
            button1.ForeColor = Color.DarkRed;
            button1.Location = new Point(72, 358);
            button1.Name = "button1";
            button1.Size = new Size(130, 50);
            button1.TabIndex = 0;
            button1.Text = "Tìm kiếm";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Font = new Font("Cascadia Code", 10.2F);
            richTextBox1.ForeColor = Color.DarkRed;
            richTextBox1.Location = new Point(72, 414);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(500, 100);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(578, 114);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(400, 400);
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.0033F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66.9967F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 493F));
            tableLayoutPanel1.Location = new Point(72, 520);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(906, 200);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // button2
            // 
            button2.Font = new Font("Cascadia Code", 10.2F);
            button2.ForeColor = Color.DarkRed;
            button2.Location = new Point(381, 198);
            button2.Name = "button2";
            button2.Size = new Size(130, 138);
            button2.TabIndex = 4;
            button2.Text = "Thêm món ăn vào cộng đồng";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Cascadia Code", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.DarkRed;
            label1.Location = new Point(152, 21);
            label1.Name = "label1";
            label1.Size = new Size(359, 40);
            label1.TabIndex = 6;
            label1.Text = "HÔM NAY ĂN GÌ ?????";
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Cascadia Code", 10.2F);
            textBox1.ForeColor = Color.DarkRed;
            textBox1.Location = new Point(72, 198);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(281, 27);
            textBox1.TabIndex = 7;
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Cascadia Code", 10.2F);
            textBox2.ForeColor = Color.DarkRed;
            textBox2.Location = new Point(72, 309);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(281, 27);
            textBox2.TabIndex = 8;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Cascadia Code", 10.2F);
            label2.ForeColor = Color.DarkRed;
            label2.Location = new Point(72, 175);
            label2.Name = "label2";
            label2.Size = new Size(110, 22);
            label2.TabIndex = 9;
            label2.Text = "Tên món ăn";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Cascadia Code", 10.2F);
            label3.ForeColor = Color.DarkRed;
            label3.Location = new Point(72, 286);
            label3.Name = "label3";
            label3.Size = new Size(190, 22);
            label3.TabIndex = 10;
            label3.Text = "Tên người đóng góp";
            // 
            // button3
            // 
            button3.Font = new Font("Cascadia Code", 10.2F);
            button3.ForeColor = Color.DarkRed;
            button3.Location = new Point(72, 231);
            button3.Name = "button3";
            button3.Size = new Size(184, 27);
            button3.TabIndex = 11;
            button3.Text = "Thêm hình ảnh món ăn";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Font = new Font("Cascadia Code", 10.2F);
            button4.ForeColor = Color.DarkRed;
            button4.Location = new Point(72, 114);
            button4.Name = "button4";
            button4.Size = new Size(130, 50);
            button4.TabIndex = 12;
            button4.Text = "CONNECT";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // textBox3
            // 
            textBox3.Font = new Font("Cascadia Code", 10.2F);
            textBox3.ForeColor = Color.DarkRed;
            textBox3.Location = new Point(262, 231);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(91, 27);
            textBox3.TabIndex = 13;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Cascadia Code", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.DarkRed;
            label4.Location = new Point(557, 21);
            label4.Name = "label4";
            label4.Size = new Size(287, 40);
            label4.TabIndex = 14;
            label4.Text = "ĂN GÌ CŨNG ĐƯỢC";
            // 
            // CLIENT
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FloralWhite;
            ClientSize = new Size(1051, 738);
            Controls.Add(label4);
            Controls.Add(textBox3);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(pictureBox1);
            Controls.Add(richTextBox1);
            Controls.Add(button1);
            Name = "CLIENT";
            Text = "CLIENT";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private RichTextBox richTextBox1;
        private PictureBox pictureBox1;
        private TableLayoutPanel tableLayoutPanel1;
        private Button button2;
        private Label label1;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label2;
        private Label label3;
        private Button button3;
        private Button button4;
        private TextBox textBox3;
        private Label label4;
    }
}
