using CloudsMove.BLL;
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
    /// RechargeView.xaml 的交互逻辑
    /// </summary>
    public partial class RechargeView : Window
    {
        T_CDKeyBLL keyBLL = new T_CDKeyBLL();
        T_UserBLL userBLL = new T_UserBLL();
        public RechargeView()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RechargeView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnRecharge_Click(object sender, RoutedEventArgs e)
        {
            string str = tbCDKey.Text.Trim();
            T_CDKey key = keyBLL.Find(str);
            if (str != null)
            {
                try
                {
                    if (key != null)
                    {
                        if (key.State == "未激活")
                        {
                            DateTime dt = Convert.ToDateTime(GlobalCache.CurrentUser.BlockingTime);
                            DateTime dtNow = SqlHelper.GetDateTimeFromSQL();
                            DateTime updateTime;
                            if(dt< dtNow)
                            {
                                updateTime = dtNow.AddDays(Convert.ToInt32(key.Length));
                            }
                            else
                            {
                                updateTime= dt.AddDays(Convert.ToInt32(key.Length));
                            }
                            GlobalCache.CurrentUser.BlockingTime = updateTime.ToString("yyyy-MM-dd HH:mm:ss");
                            key.State = "已激活";
                            key.UserEmail = EncryptsHelper.Decrypt(GlobalCache.CurrentUser.EMail);
                            key.UseTime = SqlHelper.GetDateTimeFromSQL().ToString("yyyy-MM-dd HH:mm:ss");

                            keyBLL.Update(key);
                            userBLL.Update(GlobalCache.CurrentUser);
                            new MessageBoxView($"兑换成功!账户成功增加 {key.Length} 天").ShowDialog();
                            return;
                        }
                        new MessageBoxView("此兑换码已被使用!").ShowDialog();
                        return;
                    }
                    new MessageBoxView("请输入有效的CDKey!").ShowDialog();
                    return;
                }
                catch { }
            }
            new MessageBoxView("请输入CDKey!").ShowDialog();
        }
    }
}
