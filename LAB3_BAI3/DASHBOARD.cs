using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB3_BAI3
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Server f = new Server();

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

            // trỏ đến hàm 'Client_FormClosed'
            f.FormClosed += new FormClosedEventHandler(Client_FormClosed);

            f.Show();
            button2.Enabled = false;
        }

        private void Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.button2.Enabled = true;
        }
    }
}

