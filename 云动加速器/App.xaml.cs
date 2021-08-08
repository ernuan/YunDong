using CloudsMove.BLL;
using CloudsMove.Common;
using CloudsMove.Interops;
using CloudsMove.ViewModels;
using CloudsMove.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace 云动加速器
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        System.Threading.Mutex mutex;
        T_GameBLL gameBLL = new T_GameBLL();
        T_ShadowsocksRBLL ssrBLL = new T_ShadowsocksRBLL();
        T_YDVersionBLL versionBLL = new T_YDVersionBLL();
        T_AdvertisingBLL advBLL = new T_AdvertisingBLL();
        T_SettingsBLL settingBLL = new T_SettingsBLL();
        T_UserBLL userBLL = new T_UserBLL();
        T_EMailManageMentBLL emailBLL = new T_EMailManageMentBLL();
        public App()
        {
            this.Startup += new StartupEventHandler(App_Startup);
        }

        public void App_Startup(object sender, StartupEventArgs e)
        {
            #region 互斥窗体
            bool ret;
            mutex = new System.Threading.Mutex(true, "ElectronicNeedleTherapySystem", out ret);

            if (!ret)
            {
                new MessageBoxView("云动加速器已经在托盘中有实例了,请退出后再重新打开!").ShowDialog();
                Environment.Exit(0);
            }
            #endregion

            //if (!GlobalCache.TCPClient.Init())
            //{
            //    new MessageBoxView("连接服务器失败!").ShowDialog();
            //    return;
            //}
            //检查更新
            CheckVersion();
            
            // 设置当前目录
            Directory.SetCurrentDirectory(Environment.CurrentDirectory);
            var binPath = Path.Combine(Environment.CurrentDirectory, "bin");
            Environment.SetEnvironmentVariable("PATH", $"{Environment.GetEnvironmentVariable("PATH")};{binPath}");

            //随机一个空闲端口
            GlobalCache.Port = PortHelper.GetAvailablePort();

            //获取所有游戏和节点
            GlobalCache.GamesList = gameBLL.FindAll();
            GlobalCache.SSRList = ssrBLL.FindAll();

            // 加载图片文件
            CheckResources();

            //读取邮件发送配置
            T_EMailManageMent email= emailBLL.Find();
            if (email != null)
            {
                GlobalCache.EMailManageMent = new T_EMailManageMent();
                GlobalCache.EMailManageMent.EMailSendMode = EncryptsHelper.Decrypt(email.EMailSendMode);
                GlobalCache.EMailManageMent.EmailAccount = EncryptsHelper.Decrypt(email.EmailAccount);
                GlobalCache.EMailManageMent.EmailPwd = EncryptsHelper.Decrypt(email.EmailPwd);
                GlobalCache.EMailManageMent.EmailTemplate = EncryptsHelper.Decrypt(email.EmailTemplate);
            }

            //读取系统默认配置
            var setting = settingBLL.Find();
            if (setting != null)
            {
                GlobalCache.CurrentSetting = new T_Settings();
                GlobalCache.CurrentSetting.SerialCode = setting.SerialCode;
                GlobalCache.CurrentSetting.LoginViewImg = setting.LoginViewImg;
                GlobalCache.CurrentSetting.UserRegBlockTime = setting.UserRegBlockTime;
                GlobalCache.CurrentSetting.QQHeadApi = setting.QQHeadApi;
            }

            //解密SSR节点
            DecryptSSR(GlobalCache.SSRList);

            //读取主界面广告列表
            var advList=advBLL.FindAll();
            if (advList.Count>0)
            {
                GlobalCache.Advertising = advList;
            }


            //CheckUpdate();
            Console.WriteLine(SqlHelper.GetDateTimeFromSQL());
            LoadLocalConfig();
        }

        /// <summary>
        /// 加载本地配置文件
        /// </summary>
        private void LoadLocalConfig()
        {
            try
            {
                bool b = Convert.ToBoolean(GlobalCache.Config.Get("AutoLogin"));
                if (b)
                {
                    string mail = GlobalCache.Config.Get("Mail");
                    string pwd = GlobalCache.Config.Get("Pwd");
                    var user = userBLL.Find(mail, pwd);
                    if (user != null && user.State == "正常")
                    {
                        user.LoginCount++;
                        userBLL.Update(user);
                        GlobalCache.CurrentUser = user;
                        
                    }
                    else if(user.State == "在线")
                    {
                        //new MessageBoxView("当前账号已被登录!").ShowDialog();
                        GlobalCache.CurrentUser = null;
                    }
                }
            }
            catch(Exception ex)
            {
                new MessageBoxView("自动登陆失败!").ShowDialog();
            }

            GlobalCache.MyGamesList.Clear();
            string txt = GlobalCache.Config.Get("Games");
            if (!string.IsNullOrEmpty(txt))
            {
                string[] games = txt.Split('|');
                foreach (var item in games)
                {
                    T_Game game= GlobalCache.GamesList.Find(t=>item==t.SerialCode);
                    if(game!=null) GlobalCache.MyGamesList.Add(game);

                }

            }
            


        }

        private void CheckVersion()
        {
            var version = versionBLL.Find("YunDong");
            if (version == null)
            {
                new MessageBoxView("获取版本号失败,请尝试联系客服解决!").ShowDialog();
                Environment.Exit(0);
            }
            string own= Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (own != version.Version)
            {
                
                string file = Environment.CurrentDirectory + "\\YD_Update.exe";
                if (!File.Exists(file))
                {
                    new MessageBoxView("检测到新版本,但自动更新程序丢失,请手动重新下载本软件!").ShowDialog();
                    Environment.Exit(0);
                }
                else
                {
                    Process.Start(file);
                    Environment.Exit(0);
                }
            }

        }

        private void CheckResources() 
        {
            string path = Environment.CurrentDirectory + "\\res";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (var item in GlobalCache.GamesList)
            {
                if (!File.Exists(path + "\\" + item.SerialCode + ".png"))
                {
                    NetHelper.DownLoadImg(item.GameImageUrl, path + "\\" + item.SerialCode + ".png");
                }
                item.GameImageUrl_ = path + "\\" + item.SerialCode + ".png";
            }
        }

        /// <summary>
        /// 解密SSR节点
        /// </summary>
        /// <param name="list"></param>
        private void DecryptSSR(List<T_ShadowsocksR> list)
        {
            foreach (var item in list)
            {
                item.HostName = EncryptsHelper.Decrypt(item.HostName);
                item.Port = EncryptsHelper.Decrypt(item.Port);
                item.Password = EncryptsHelper.Decrypt(item.Password);
                item.Method = EncryptsHelper.Decrypt(item.Method);
                item.Protocol = EncryptsHelper.Decrypt(item.Protocol);
                item.ProtocolParam = EncryptsHelper.Decrypt(item.ProtocolParam);
                item.OBFS = EncryptsHelper.Decrypt(item.OBFS);
                item.OBFSParam = EncryptsHelper.Decrypt(item.OBFSParam);
                item.State_ = "空闲";
                Task.Run(()=> 
                {
                    item.Delay_ = NetHelper.PingIP(item.HostName).ToString();
                });
            }
        }

        #region 更新程序下载
        private void CheckUpdate()
        {
            try
            {
                string file = Environment.CurrentDirectory + "\\YD_Update.exe";
                if (File.Exists(file))
                {
                    string info = FileVersionInfo.GetVersionInfo(file).FileVersion ?? "";
                    if (info != "1.0.0.2")
                    {
                        DownLoad(file);
                    }
                }
                else
                {
                    DownLoad(file);
                }

            }
            catch
            {

            }
        }

        private void DownLoad(string file)
        {
            try
            {
                File.Delete(file);

                Task.Run(() =>
                {
                    foreach (var proc in Process.GetProcessesByName("YD_Update"))
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
                    string url = "https://www.zsyf.link/Games/Update";
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //加上这一句解决win7出现请求被中止: 未能创建 SSL/TLS 安全通道
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    WebResponse respone = request.GetResponse();
                    Stream netStream = respone.GetResponseStream();
                    using (Stream fileStream = new FileStream(file, FileMode.Create))
                    {
                        byte[] read = new byte[1024];
                        int realReadLen = netStream.Read(read, 0, read.Length);
                        while (realReadLen > 0)
                        {
                            fileStream.Write(read, 0, realReadLen);
                            realReadLen = netStream.Read(read, 0, read.Length);
                        }
                        netStream.Close();
                    }
                });
            }
            catch { }
        }

        #endregion
    }
}
