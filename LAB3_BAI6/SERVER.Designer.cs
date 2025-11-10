namespace LAB3_BAI6
{
    partial class SERVER
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
            richTextBox1 = new RichTextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Font = new Font("Cascadia Code SemiBold", 12F, FontStyle.Bold);
            richTextBox1.ForeColor = Color.DarkRed;
            richTextBox1.Location = new Point(11, 60);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(600, 400);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // button1
            // 
            button1.Font = new Font("Cascadia Code SemiBold", 12F, FontStyle.Bold);
            button1.ForeColor = Color.DarkRed;
            button1.Location = new Point(443, 12);
            button1.Name = "button1";
            button1.Size = new Size(169, 42);
            button1.TabIndex = 1;
            button1.Text = "LISTEN";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // SERVER
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FloralWhite;
            ClientSize = new Size(623, 474);
            Controls.Add(button1);
            Controls.Add(richTextBox1);
            Name = "SERVER";
            Text = "SERVER";
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox1;
        private Button button1;
    }
}
