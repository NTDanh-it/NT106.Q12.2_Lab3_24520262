using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB3_BAI6
{
    public partial class DashBoard : Form
    {
        public DashBoard()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SERVER f = new SERVER();

            // trỏ đến hàm 'Server_FormClosed'
            f.FormClosed += new FormClosedEventHandler(Server_FormClosed);

            f.Show();
            button1.Enabled = false;
        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Client f = new Client();
            f.Show();
        }
    }
}
