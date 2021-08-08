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

namespace CloudsMove.Views
{
    /// <summary>
    /// MessageBoxView.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxView : Window
    {
        public MessageBoxView()
        {
            InitializeComponent();
        }

        public MessageBoxView(string msg) : this() 
        {
            this.txtMsg.Text = msg;
        }
        private void btnSysClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOK_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
