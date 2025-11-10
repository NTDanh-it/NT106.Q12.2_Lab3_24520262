namespace LAB3_BAI4
{
    partial class DASHBOARD
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
            button2 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(58, 13);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(457, 54);
            button1.TabIndex = 0;
            button1.Text = "SERVER";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(58, 94);
            button2.Margin = new Padding(4);
            button2.Name = "button2";
            button2.Size = new Size(457, 54);
            button2.TabIndex = 1;
            button2.Text = "CLIENT";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // DASHBOARD
            // 
            AutoScaleDimensions = new SizeF(12F, 27F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FloralWhite;
            ClientSize = new Size(581, 171);
            Controls.Add(button2);
            Controls.Add(button1);
            Font = new Font("Cascadia Code SemiBold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.DarkRed;
            Margin = new Padding(4);
            Name = "DASHBOARD";
            Text = "DASHBOARD";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
    }
}
