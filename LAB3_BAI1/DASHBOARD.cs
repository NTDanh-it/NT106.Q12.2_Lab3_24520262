using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB3_BAI1
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Client f = new Client();
            f.Show();
            button1.Enabled = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Server f = new Server();
            f.Show();
            button2.Enabled = false;
        }
    }
}
