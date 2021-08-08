using CloudsMove.BLL;
using CloudsMove.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace YD_Update
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        T_YDVersionBLL bll = new T_YDVersionBLL();
        public MainView()
        {
            InitializeComponent();
        }

        public delegate void ProgressBarSetter(double value);
        public void SetProgressBar(double value)
        {
            progressBar1.Value = value;
            txtValue.Text = Convert.ToInt32(value / progressBar1.Maximum) * 100  + " %";
        }

        private void DownLoadFile(string url, string path)
        {
            Process[] processes = Process.GetProcessesByName("云动加速器");
            try
            {
                foreach (var item in processes)
                {
                    item.Kill();
                }
            }
            catch
            {

            }
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("更新异常!");
                return;
            }

            Task.Run(() =>
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //加上这一句解决win7出现请求被中止: 未能创建 SSL/TLS 安全通道
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                WebResponse respone = request.GetResponse();
                Stream netStream = respone.GetResponseStream();
                progressBar1.Dispatcher.BeginInvoke(new Action(()=> progressBar1.Maximum = respone.ContentLength));
                using (Stream fileStream = new FileStream(path, FileMode.Create))
                {
                    byte[] read = new byte[1024];
                    int realReadLen = netStream.Read(read, 0, read.Length);
                    long progressBarValue = 0;
                    while (realReadLen > 0)
                    {
                        fileStream.Write(read, 0, realReadLen);
                        realReadLen = netStream.Read(read, 0, read.Length);

                        progressBarValue += realReadLen;
                        progressBar1.Dispatcher.BeginInvoke(new ProgressBarSetter(SetProgressBar), progressBarValue);

                    }
                    netStream.Close();
                    fileStream.Close();

                    Task.Delay(1000);
                    Dispatcher.BeginInvoke(new Action(() => 
                    {
                        var result=MessageBox.Show("更新完成,是否立即打开?", "更新完毕", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes) 
                        {
                            this.Close();
                            Process.Start(path);
                        }
                    }));
                }
            });

        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            #region 读取更新日志
            List<string> txt = ReadUrlToList("https://www.zsyf.link/Games/UpdateLog");
            StringBuilder sb = new StringBuilder();
            foreach (var item in txt)
            {
                sb.Append(item + "\r\n");
            }
            txtUpdateLog.Text = sb.ToString();

            #endregion


            T_YDVersion version = bll.Find("yundong");
            if (version == null)
            {
                return;
            }

            string file = Environment.CurrentDirectory + "\\云动加速器.exe";
            if (File.Exists(file))
            {
                string info = FileVersionInfo.GetVersionInfo(file).FileVersion ?? "";
                if (info != version.Version)
                {
                    File.Delete(file);
                    DownLoadFile(version.DownLoadUrl, file);
                }
                else
                {
                    progressBar1.Value = progressBar1.Maximum;
                    txtValue.Text = "100 %";
                    MessageBox.Show("已是最新版本,不需要更新!");
                }
            }
            else
            {
                DownLoadFile(version.DownLoadUrl, file);
            }
        }

        private void Window_DragMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed) 
            {
                this.DragMove();
            }
        }

        public static List<string> ReadUrlToList(string url)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //加上这一句解决win7出现请求被中止: 未能创建 SSL/TLS 安全通道
                List<string> list = new List<string>();
                HttpWebRequest oHttp_Web_Req = (HttpWebRequest)WebRequest.Create(url);
                Stream oStream = oHttp_Web_Req.GetResponse().GetResponseStream();
                using (StreamReader respStreamReader = new StreamReader(oStream, Encoding.UTF8))
                {
                    string line = string.Empty;
                    // 从数据流中读取每一行，直到文件的最后一行
                    string tmp = respStreamReader.ReadLine();
                    while (tmp != null)
                    {
                        list.Add(tmp);
                        tmp = respStreamReader.ReadLine();
                    }
                    //关闭此StreamReader对象 
                    respStreamReader.Close();
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
