using CloudsMove.Common;
using CloudsMove.Controllers;
using CloudsMove.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CloudsMove.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public List<T_Game> AllGamesList = new List<T_Game>();

        private SSRController ssrController;
        private NFController nfController;

        private bool isStarting = false;
        private bool IsStarting
        {
            get { return isStarting; }
            set
            {
                isStarting = value;
                if (isStarting)
                {
                    tbTip.Dispatcher.BeginInvoke(new Action(() => { tbTip.Visibility = Visibility.Visible; }));
                }
                else tbTip.Dispatcher.BeginInvoke(new Action(() => { tbTip.Visibility = Visibility.Hidden; }));

            }
        }
        DispatcherTimer Advtimer = new DispatcherTimer();
        DispatcherTimer TcpTimer = new DispatcherTimer();
        public MainView()
        {
            InitializeComponent();

            //初始化托盘图标事件
            InitNotifyIcon();
            ChangePage(0);
            nav_Classification_All.IsChecked = true;
            list_MyGames.ItemsSource = GlobalCache.MyGamesList;
            listBoxAdv.ItemsSource = GlobalCache.Advertising;
            RefreshUser();
            if (GlobalCache.MyGamesList.Count > 0)
            {
                ChangePage(1);
            }

            TcpTimer.Interval = new TimeSpan(20000);
            TcpTimer.Tick += TcpTimer_Tick;
            TcpTimer.Start();
        }

        private void TcpTimer_Tick(object sender, EventArgs e)
        {
            GlobalCache.TCPClient.SendMsg(MsgType.Heart);
        }

        /// <summary>
        /// 刷新当前登陆账户信息
        /// </summary>
        private void RefreshUser()
        {
            if (GlobalCache.CurrentUser != null)
            {
                txtUserID.Text = EncryptsHelper.Decrypt(GlobalCache.CurrentUser.EMail);
                txtBlokingTime.Text = "到期: " + GlobalCache.CurrentUser.BlockingTime;
                GetUserQQHead();
                GlobalCache.TCPClient.SendMsg(MsgType.Login);
                tabControlUser.SelectedIndex = 1;
            }
        }

        private void GetUserQQHead()
        {
            try
            {
                string mail = txtUserID.Text;
                string[] temp = mail.Split('@'); //xxxxx@qq.com 分割邮箱
                string qqNumTemp = temp[0];
                //qq邮箱的前半段可能包含字母,先全部转小写,然后去除字母获得QQ号
                qqNumTemp=qqNumTemp.ToLower();
                string qqNum = Regex.Replace(qqNumTemp, "[a - z]", "", RegexOptions.IgnoreCase);
                string api = GlobalCache.CurrentSetting.QQHeadApi;
                if (!string.IsNullOrEmpty(api))
                {
                    GlobalCache.CurrentUser._QQHead = api.Replace("号码", qqNum);
                    bdHead.DataContext = GlobalCache.CurrentUser;
                }
            }
            catch { }


        }


        #region 置托盘

        private NotifyIcon _notifyIcon = null;

        private void InitNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = "云动加速器";
            //_notifyIcon.Icon = new System.Drawing.Icon(@"Appicon.ico");//程序图标
            _notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);//当前程序图标
            _notifyIcon.Visible = true;


            //打开菜单项
            System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("打开云动");
            open.Click += new EventHandler(Show);
            ////隐藏菜单项
            //System.Windows.Forms.MenuItem hide = new System.Windows.Forms.MenuItem("Hide");
            //hide.Click += new EventHandler(Hide);
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += new EventHandler(Close);
            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { open, exit };
            _notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            //双击图标
            _notifyIcon.MouseDoubleClick += OnMouseDoubleClickHandler;


            //Hide(null, null);
        }

        private void OnMouseDoubleClickHandler(object sender, EventArgs e)
        {
            this.Show(null, null);
        }

        private void Show(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            this.ShowInTaskbar = true;
            this.Activate();
        }

        private void Hide(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Close(object sender, EventArgs e)
        {
            try
            {
                GlobalCache.TCPClient.StopConnect();
            }
            catch { }
            System.Windows.Application.Current.Shutdown();
        }

        #endregion

        #region 系统按钮
        private void DragWindow(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void SystemBtn_Close(object sender, RoutedEventArgs e)
        {
            if (IsStarting) 
            {
                try
                {
                    ssrController.Stop();
                    nfController.Stop();
                }
                catch { }
            }
            this.Close();
        }

        private void SystemBtn_Min(object sender, RoutedEventArgs e)
        {
            Hide();
            //this.WindowState = WindowState.Minimized;
        }

        private void SystemBtn_Setting(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region 左侧导航栏
        private void ChangePage(int index)
        {
            switch (index)
            {
                case 0:
                    nav_AllGames.IsChecked = true;
                    tabControl1.SelectedIndex = 0;
                    break;
                case 1: nav_MyGames.IsChecked = true;
                    tabControl1.SelectedIndex = 1;
                    break;
            }
        }

        private void nav_AllGames_Checked(object sender, RoutedEventArgs e)
        {
            ChangePage(0);
        }

        private void nav_MyGames_Checked(object sender, RoutedEventArgs e)
        {
            ChangePage(1);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (new LoginView().ShowDialog() == true) RefreshUser();
        }

        private void btnLogOff_Click(object sender, MouseButtonEventArgs e)
        {
            tabControlUser.SelectedIndex = 0;
            GlobalCache.TCPClient.SendMsg(MsgType.LogOut);
            Thread.Sleep(100);
            GlobalCache.CurrentUser = null;
        }        
        
        private void btnSponsored_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("tencent://message/?uin=1351334506");
        }
        #endregion

        #region 游戏列表分类
        private void RefreshAllGames(string type) 
        {
            switch (type) 
            {
                case "全部游戏":
                    AllGamesList = CommonHelper.Clone(GlobalCache.GamesList);
                    break;

                case "热门游戏":
                    AllGamesList.Clear();
                    GlobalCache.GamesList.ForEach(t =>
                    {
                        if (t.GameType == "热门游戏")
                        {
                            AllGamesList.Add(t);
                        }
                    });
                    break;
                case "游戏平台":
                    AllGamesList.Clear();
                    GlobalCache.GamesList.ForEach(t =>
                    {
                        if (t.GameType == "游戏平台")
                        {
                            AllGamesList.Add(t);
                        }
                    });
                    break;
                case "最新上线":
                    AllGamesList.Clear();
                    GlobalCache.GamesList.ForEach(t =>
                    {
                        if (t.GameType == "最新上线")
                        {
                            AllGamesList.Add(t);
                        }
                    });
                    break;
                case "无界浏览":
                    AllGamesList.Clear();
                    GlobalCache.GamesList.ForEach(t =>
                    {
                        if (t.GameType == "无界浏览")
                        {
                            AllGamesList.Add(t);
                        }
                    });
                    break;
            }
            list_AllGames.ItemsSource = AllGamesList;
            list_AllGames.Items.Refresh();

        }
        private void Classification_All(object sender, RoutedEventArgs e)
        {
            RefreshAllGames("全部游戏");
        }

        private void Classification_Hot(object sender, RoutedEventArgs e)
        {
            RefreshAllGames("热门游戏");
        }

        private void Classification_Pla(object sender, RoutedEventArgs e)
        {
            RefreshAllGames("游戏平台");
        }

        private void Classification_New(object sender, RoutedEventArgs e)
        {
            RefreshAllGames("最新上线");
        }

        private void Classification_Bro(object sender, RoutedEventArgs e)
        {
            RefreshAllGames("无界浏览");
        }

        #endregion

        #region 添加/删除游戏到我的列表

        private void AddToMyGamesList(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsStarting)
                {
                    new MessageBoxView("正在加速中!").ShowDialog();
                    return;
                }
                T_Game game = AllGamesList.Find(t => t.SerialCode == ((System.Windows.Controls.Button)sender).Tag.ToString());

                var tempGame=GlobalCache.MyGamesList.Find(t=>t.SerialCode==game.SerialCode);
                if (tempGame==null)
                {
                    GlobalCache.MyGamesList.Add(game);
                }
                else
                {
                    GlobalCache.MyGamesList.Remove(tempGame);
                    GlobalCache.MyGamesList.Insert(0,game);
                }

                string temp = GlobalCache.Config.Get("Games");
                if (temp==null)
                {
                    string newConfig = temp + game.SerialCode + "|";
                    GlobalCache.Config.Set("Games", newConfig);
                    GlobalCache.Config.Save();

                }
                else
                {
                    List<string> list = new List<string>(temp.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries));
                    if (!list.Contains(game.SerialCode))
                    {
                        string newConfig = temp + game.SerialCode + "|";
                        GlobalCache.Config.Set("Games", newConfig);
                        GlobalCache.Config.Save();
                    }
                }
                

                list_MyGames.Items.Refresh();
                ChangePage(1);
                tabControl2.SelectedIndex = 0;
            }
            catch
            {

            }
        }

        private void RemoveFromMyGames(object sender, RoutedEventArgs e)
        {
            try
            {
                T_Game game = GlobalCache.MyGamesList.Find(t => t.SerialCode == ((System.Windows.Controls.Button)sender).Tag.ToString());
                if (GlobalCache.MyGamesList.Contains(game))
                {
                    GlobalCache.MyGamesList.Remove(game);
                    if (GlobalCache.MyGamesList.Count == 0) ChangePage(0);
                    list_MyGames.Items.Refresh();
                }

                string temp = GlobalCache.Config.Get("Games");
                List<string> list = new List<string>(temp.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries));
                if (list.Contains(game.SerialCode)) list.Remove(game.SerialCode);
                StringBuilder newConfig=new StringBuilder();
                list.ForEach(t => newConfig.Append(t + "|"));
                GlobalCache.Config.Set("Games", newConfig.ToString());
                GlobalCache.Config.Save();
            }
            catch
            {

            }
        }

        #endregion

        #region 游戏加速
        /// <summary>
        /// 准备加速,添加游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrepare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                T_Game game = GlobalCache.MyGamesList.Find(t => t.SerialCode == ((System.Windows.Controls.Button)sender).Tag.ToString());
                GlobalCache.CurrentGame = game;

                txtCurrentGameName.Text = game.GameName;
                ImageSourceConverter image= new ImageSourceConverter();
                ImageBrush brush = new ImageBrush();
                CurrentGameImg.DataContext = GlobalCache.CurrentGame;
                tabControl2.SelectedIndex = 1;
            }
            catch
            {

            }
        }

        /// <summary>
        /// 返回我的游戏列表界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (IsStarting)
            {
                new MessageBoxView("正在加速中,请先断开连接!").ShowDialog();
                return;
            }

            GlobalCache.CurrentGame = null;
            GlobalCache.CurrentSSR = null;

            tabControl2.SelectedIndex = 0;
            txtDelay.Text = "0";
            btnChooseLine.Content = "选择节点";
        }

        private void btnChooseLine_Click(object sender, RoutedEventArgs e)
        {
            if (IsStarting)
            {
                new MessageBoxView("加速过程中无法切换节点!").ShowDialog();
                return;
            }
            if (GlobalCache.CurrentGame != null) 
            {
                switch (GlobalCache.CurrentGame.LineType)
                {
                    case "游戏":
                        if (new ChooseLineView("游戏").ShowDialog() == true)
                        {
                            Connect();
                        };
                        break;
                    case "":
                        if (new ChooseLineView("").ShowDialog() == true)
                        {
                            Connect();
                        };
                        break;
                    default:new MessageBoxView("该游戏暂无可用节点!").ShowDialog();
                        break;
                }
            }

            if (GlobalCache.CurrentSSR != null)
            {
                btnChooseLine.Content = GlobalCache.CurrentSSR.Remark;
                txtDelay.Text = GlobalCache.CurrentSSR.Delay_;
            }
            else 
            {
                txtDelay.Text = "0";
                btnChooseLine.Content = "选择节点";
            }

        }

        private void Connect()
        {
            btnStart.IsEnabled = false;
            if (IsStarting)
            {
                try
                {
                    Break();
                }
                catch
                {

                }
            }
            if (GlobalCache.CurrentUser == null)
            {
                new MessageBoxView("请先登陆账号!").ShowDialog();
                btnStart.IsEnabled = true;
                return;
            }

            //判断账号有效期
            DateTime currentTime = SqlHelper.GetDateTimeFromSQL();
            if (Convert.ToDateTime(GlobalCache.CurrentUser.BlockingTime) < currentTime)
            {
                new MessageBoxView("账号已到期！").ShowDialog();
                btnStart.IsEnabled = true;
                return;
            }

            if (GlobalCache.CurrentSSR == null)
            {
                switch (GlobalCache.CurrentGame.LineType)
                {
                    case "游戏":
                        if (new ChooseLineView("游戏").ShowDialog() == false)
                        {
                            btnStart.IsEnabled = true;
                            return;
                        };
                        break;
                    case "":
                        if (new ChooseLineView("").ShowDialog() == false)
                        {
                            btnStart.IsEnabled = true;
                            return;
                        };
                        break;
                    default:
                        new MessageBoxView("该游戏暂无可用节点!").ShowDialog();
                        break;
                }
                if (GlobalCache.CurrentSSR != null)
                {
                    btnChooseLine.Content = GlobalCache.CurrentSSR.Remark;
                    txtDelay.Text = GlobalCache.CurrentSSR.Delay_;
                }
                else
                {
                    txtDelay.Text = "0";
                    btnChooseLine.Content = "选择节点";
                    return;
                }
            }

            Task.Run(() =>
            {
                foreach (var proc in Process.GetProcessesByName("YD_socks"))
                {
                    try
                    {
                        proc.Kill();
                    }
                    catch
                    {
                            // 跳过
                    }
                }

                try
                {
                    GlobalCache.CurrentRoute.Clear();
                }
                catch
                {

                }
                if (string.IsNullOrEmpty(GlobalCache.CurrentGame.GameRouteUrl))
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        new MessageBoxView("该游戏暂不支持加速!").ShowDialog();
                        btnStart.IsEnabled = true;
                    }));
                    return;
                }
                GlobalCache.CurrentRoute = NetHelper.ReadUrlRouteToList(GlobalCache.CurrentGame.GameRouteUrl);
                try
                {
                    if (GlobalCache.CurrentRoute == null)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            new MessageBoxView("路由表获取失败!").ShowDialog();
                            btnStart.IsEnabled = true;
                        }));
                        return;
                    }
                }
                catch
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        new MessageBoxView("路由表获取失败!").ShowDialog();
                        btnStart.IsEnabled = true;
                    }));
                }

                string path = System.Environment.CurrentDirectory;
                if (!File.Exists(path + @"\bin\YD_socks.exe"))
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        new MessageBoxView("缺少运行文件，请尝试重新安装！").ShowDialog();
                        btnStart.IsEnabled = false;
                    }));
                    return;
                }

                string md5 = GetMD5HashFromFile(path + @"\bin\YD_socks.exe");
                if (GlobalCache.MD5 != md5)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        new MessageBoxView("文件已损坏，请尝试关闭杀毒软件重新安装！").ShowDialog();
                        btnStart.IsEnabled = false;
                    }));
                    return;
                }

                ssrController = new SSRController(GlobalCache.CurrentSSR);
                nfController = new NFController();

                Dispatcher.Invoke(new Action(() =>
                {
                    progressBar1.Value = 30;
                }));

                if (ssrController.Start())
                {
                    nfController.Start(GlobalCache.CurrentRoute);
                    IsStarting = true;
                    Dispatcher.Invoke(new Action(() =>
                    {
                        progressBar1.Value = 100;
                        btnStart.Visibility = Visibility.Hidden;
                        btnBreak.Visibility = Visibility.Visible;
                        Timing();
                    }));
                }
                else
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        progressBar1.Value = 0;
                        new MessageBoxView("加速异常!").ShowDialog();
                        btnStart.IsEnabled = true;
                        IsStarting = false;
                    }));
                }
            });

        }

        private void Break()
        {
            Task.Run(() =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    IsStarting = false;
                    try
                    {
                        TimingStop();
                        txtTimer.Text = "00:00:00";
                        btnStart.IsEnabled = true;
                        progressBar1.Value = 0;
                        btnStart.Visibility = Visibility.Visible;
                        btnBreak.Visibility = Visibility.Hidden;
                    }
                    catch { }

                }));
                ssrController.Stop();
                nfController.Stop();
            });
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Connect();
        }

        private void AddQQ(object sender, RoutedEventArgs e)
        {
            
        }

        #region 加速时长计时器
        System.Threading.Timer Mytimer;
        long TimeCount;
        delegate void SetValue();
        private void TimerUp(object state)
        {
            Dispatcher.Invoke(new SetValue(ShowTime));
            TimeCount++;
        }
        public void ShowTime()
        {
            TimeSpan t = new TimeSpan(0, 0, (int)TimeCount);
            txtTimer.Text = string.Format("{0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds);

        }
        //开始计时
        private void Timing()
        {
            Mytimer = new System.Threading.Timer(new TimerCallback(TimerUp), null, Timeout.Infinite, 1000);
            TimeCount = 0;
            Mytimer.Change(0, 1000);
        }
        //停止计时
        private void TimingStop()
        {
            Mytimer.Change(Timeout.Infinite, 1000);
        }


        #endregion

        private void btnBreak_Click(object sender, RoutedEventArgs e)
        {
            Break();
            btnBreak.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// 获取文件的MD5码
        /// </summary>
        /// <param name="fileName">传入的文件名（含路径及后缀名）</param>
        /// <returns></returns>
        public string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, System.IO.FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        #endregion

        #region 轮播广告
        private void listBoxAdv_Loaded(object sender, RoutedEventArgs e)
        {
            if (GlobalCache.Advertising.Count > 0)
            {
                listBoxAdv.Cursor = System.Windows.Input.Cursors.Hand;
                Advtimer.Interval = TimeSpan.FromMilliseconds(8000);
                Advtimer.Tick += new EventHandler(Advtimer_Tick);
                listBoxAdv.SelectedIndex = 0;
                Advtimer.Start();
            }
        }
        private void listBoxAdv_UnLoaded(object sender, RoutedEventArgs e)
        {
            Advtimer.Stop();
        }
        private void Advtimer_Tick(object sender, EventArgs e)
        {
            if (listBoxAdv.Items.Count > 0)
            {
                if (listBoxAdv.SelectedIndex == listBoxAdv.Items.Count - 1)
                {
                    listBoxAdv.SelectedIndex = 0;
                    listBoxAdv.ScrollIntoView(listBoxAdv.Items[listBoxAdv.SelectedIndex]);
                }
                else
                {
                    listBoxAdv.SelectedIndex += 1;
                    listBoxAdv.ScrollIntoView(listBoxAdv.Items[listBoxAdv.SelectedIndex]);
                }
            }
        }
        private void btnAdv_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (GlobalCache.Advertising.Count > 0)
                {
                    Process.Start(GlobalCache.Advertising[listBoxAdv.SelectedIndex].ImageLink);
                }
            }
            catch { }
        }



        #endregion

        private void btnRecharge_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalCache.CurrentUser != null)
            {
                new RechargeView().ShowDialog();
                txtBlokingTime.Text = "到期: " +GlobalCache.CurrentUser.BlockingTime;
                return;
            }
            new MessageBoxView("请先登录账号!").ShowDialog();
        }
    }
}
