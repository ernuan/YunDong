using CloudsMove.Common;
using CloudsMove.ViewModels;
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
    /// Interaction logic for ChooseLineView.xaml
    /// </summary>
    public partial class ChooseLineView : Window
    {
        List<T_ShadowsocksR> list = new List<T_ShadowsocksR>();
        public ChooseLineView()
        {
            InitializeComponent();
        }

        public ChooseLineView(string linetype) : this()
        {
            switch (linetype)
            {
                case "游戏":
                    GlobalCache.SSRList.ForEach(t =>
                    {
                        if (t.LineType == "游戏") list.Add(t);
                    });
                    break;
                case "":
                    GlobalCache.SSRList.ForEach(t =>
                    {
                        if (string.IsNullOrEmpty(t.LineType)) list.Add(t);
                    });
                    break;
            }
            RefreshSSR();
            listView1.ItemsSource = list;
            listView1.Items.Refresh();


        }

        private void DragWindow(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void RefreshSSR()
        {
            Task.Run(new Action(() =>
            {
                list.ForEach(t =>
                {
                    t.Delay_ = NetHelper.PingIP(t.HostName).ToString();
                });
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    listView1.Items.Refresh();
                }));
            }));
        }

        private void brnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshSSR();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnChoose_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedIndex == -1)
            {
                new MessageBoxView("请选择一个节点!").ShowDialog();
            }
            else 
            {
                GlobalCache.CurrentSSR = list[listView1.SelectedIndex];
                this.DialogResult = true;
            }
        }
    }
}
