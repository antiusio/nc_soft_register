using DataBase;
using Proxy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Registration
{
    public partial class Tunnels : Form
    {
        public SocksSsh socksSsh = null;
        public Tunnels( proxy proxyDB)
        {
            InitializeComponent();
            socksSsh = new SocksSsh(proxyDB);
            label1.Text = socksSsh.Port.ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
