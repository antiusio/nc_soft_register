using DataBase;
using Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainWin.Windows
{
    /// <summary>
    /// Interaction logic for SshSocks.xaml
    /// </summary>
    public partial class SshSocks : Window
    {
        public SocksSsh socksSsh = null;
        public SshSocks(proxy p)
        {
            socksSsh = new SocksSsh(p);
            InitializeComponent();
        }
    }
}
