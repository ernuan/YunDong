using CloudsMove.BLL;
using CloudsMove.Common;
using CloudsMove.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Window
    {
        T_UserBLL userBLL = new T_UserBLL();

        #region 注册页面的验证码

        private string currentRegCode;
        private string CurrentRegCode
        {
            get { return currentRegCode; }
            set 
            {
                currentRegCode = value;
                try
                {
                    if (CodeAvailabilityTimer != null) CodeAvailabilityTimerTimingStop();
                }
                catch { }
                CodeAvailabilityTimerTiming();
            }
        }

        #endregion

        #region 重置页面的验证码

        private string currentResetCode;
        private string CurrentResetCode
        {
            get { return currentResetCode; }
            set
            {
                currentResetCode = value;
                try
                {
                    if (CodeAvailabilityTimer != null) CodeAvailabilityTimerTimingStop();
                }
                catch { }
                CodeAvailabilityTimerTiming();
            }
        }


        /// <summary>
        /// 获得一个验证码后，有十分钟有效性
        /// </summary>
        private void AutoChangeCurrentResetCode()
        {
            DateTime dateTime = SqlHelper.GetDateTimeFromSQL();
            DateTime endTime = dateTime.AddMinutes(10);

            while (DateTime.Now < endTime)
            {
                Task.Delay(2000);
            }
            currentResetCode = CreateRandom(6);//到期了就重新生成一个
        }
        #endregion

        public LoginView()
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(GlobalCache.CurrentSetting.LoginViewImg))
            {
                bdImg.DataContext = GlobalCache.CurrentSetting;
                imgBackground.Visibility = Visibility.Hidden;
            }

            //如果从未设置过自动登录,默认为自动登录
            if (GlobalCache.Config.Get("AutoLogin") == null) cbAutoLogin.IsChecked = true;
            else
            {
                bool b = Convert.ToBoolean(GlobalCache.Config.Get("AutoLogin"));
                if (b) cbAutoLogin.IsChecked = true;
                else cbAutoLogin.IsChecked = false;
            }

            //如果本地保存了邮箱和密码,则自动填充到textbox里
            string mail = GlobalCache.Config.Get("Mail");
            string pwd = GlobalCache.Config.Get("Pwd");
            if (mail != null && pwd != null)
            {
                try
                {
                    tbLogin.Text = EncryptsHelper.Decrypt(mail);
                    pwdLogin.Password = EncryptsHelper.Decrypt(pwd);
                }
                catch { }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region tabControl翻页
        private void btnGoToRegister_Click(object sender, RoutedEventArgs e)
        {
            tabControl1.SelectedIndex=1;
        }

        private void btnForgetPwd_Click(object sender, RoutedEventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void btnReturnLogin_Click(object sender, RoutedEventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        #endregion

        #region 发信和验证码的实现
        /// <summary>
        /// 生成6位数字和大写字母的验证码
        /// </summary>
        /// <param name="codelengh">验证码长度</param>
        /// <returns></returns>
        private string CreateRandom(int codelengh)
        {
            int rep = 0;
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codelengh; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="mail">发送至邮箱</param>
        private void SendCode(string mail,int type)
        {
            if (GlobalCache.EMailManageMent != null)
            {
                Task.Run(new Action(() =>
                {
                    //实例化一个发送邮件类。
                    MailMessage mailMessage = new MailMessage();
                    //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
                    mailMessage.From = new MailAddress(GlobalCache.EMailManageMent.EmailAccount);
                    //收件人邮箱地址。
                    mailMessage.To.Add(new MailAddress(mail));
                    //邮件标题。
                    mailMessage.Subject = "云动加速器注册";
                    string verificationcode = CreateRandom(6);

                    //邮件内容。
                    List<string> template = NetHelper.ReadUrlRouteToList(GlobalCache.EMailManageMent.EmailTemplate);
                    string content = string.Join("",template.ToArray());
                    content = content.Replace("verificationcode", verificationcode);
                    content = content.Replace("邮箱地址", mail);
                    mailMessage.Body = content.Replace("verificationcode", verificationcode);
                    mailMessage.IsBodyHtml = true; //引用了html样式

                    //当前的验证码为生成的验证码
                    if (type == 0) CurrentRegCode = verificationcode;
                    else CurrentResetCode = verificationcode;

                    //实例化一个SmtpClient类。
                    SmtpClient client = new SmtpClient();
                    //在这里我使用的是qq邮箱，所以是smtp.qq.com，如果你使用的是126邮箱，那么就是smtp.126.com。
                    client.Host = GlobalCache.EMailManageMent.EMailSendMode;
                    //使用安全加密连接。
                    client.EnableSsl = true;
                    //不和请求一块发送。
                    client.UseDefaultCredentials = false;
                    //验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
                    client.Credentials = new NetworkCredential(GlobalCache.EMailManageMent.EmailAccount, GlobalCache.EMailManageMent.EmailPwd);
                    //发送
                    try
                    {
                        client.Send(mailMessage);
                    }
                    catch
                    {
                        this.Dispatcher.Invoke(delegate 
                        {
                            new MessageBoxView("发送失败,请检查本地网络或邮箱号!").ShowDialog();
                        });
                    }
                }));
            }
            else new MessageBoxView("发送失败,无法连接到发信服务器!").ShowDialog();


        }

        #endregion

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbLogin.Text))
            {
                new MessageBoxView("请输入需要登陆的邮箱!").ShowDialog();
                return;
            }
            if (string.IsNullOrEmpty(pwdLogin.Password))
            {
                new MessageBoxView("请输入密码!").ShowDialog();
                return;
            }

            try
            {
                var user = userBLL.Find(EncryptsHelper.Encrypt(tbLogin.Text), EncryptsHelper.Encrypt(pwdLogin.Password.ToString()));
                if (user != null)
                {
                    if (user.State == "在线")
                    {
                        new MessageBoxView("此账号已被登录!").ShowDialog();
                        return;
                    }
                    if (user.State != "正常")
                    {
                        new MessageBoxView("此账号暂时无法登录,请联系管理员!").ShowDialog();
                        return;
                    }

                    user.LoginCount++;
                    userBLL.Update(user);
                    GlobalCache.CurrentUser = user;
                    if (cbAutoLogin.IsChecked == true)
                    {
                        GlobalCache.Config.Set("AutoLogin", true.ToString());
                        GlobalCache.Config.Set("Mail", user.EMail);
                        GlobalCache.Config.Set("Pwd", user.Pwd);
                    }
                    else
                    {
                        GlobalCache.Config.Set("AutoLogin", false.ToString());
                    }
                    GlobalCache.Config.Save();
                    this.DialogResult = true;
                }
                else
                {
                    new MessageBoxView("账号或密码错误,请重新输入!").ShowDialog();
                    pwdLogin.Clear();
                }
            }
            catch(Exception ex)
            {
                //new MessageBoxView(ex.ToString()).ShowDialog();
            }

        }

        private void btnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (GlobalCache.EMailManageMent.EmailAccount == null)
            {
                new MessageBoxView("发信服务器异常!请联系管理员后重试!").ShowDialog();
                return;
            }

            if (btn.Tag.ToString() == "0")//注册
            {
                if (IsEmail(tbRegMail.Text.Trim()))
                {
                    var user = userBLL.Find(EncryptsHelper.Encrypt(tbRegMail.Text));
                    if (user != null)
                    {
                        new MessageBoxView("此邮箱已被注册,请勿重复注册!").ShowDialog();
                        return;
                    }
                    CurrentRegCode = CreateRandom(6);
                    SendCode(tbRegMail.Text.Trim(),0);
                }
                else
                {
                    new MessageBoxView("请输入合法的邮箱!").ShowDialog();
                    return;
                }
            }
            else //重置
            {
                if (IsEmail(tbResetMail.Text.Trim()))
                {
                    var user = userBLL.Find(EncryptsHelper.Encrypt(tbResetMail.Text));
                    if (user == null)
                    {
                        new MessageBoxView("此邮箱尚未被注册!").ShowDialog();
                        return;
                    }
                    CurrentResetCode = CreateRandom(6);
                    SendCode(tbResetMail.Text.Trim(), 1);
                }
                else
                {
                    new MessageBoxView("请输入合法的邮箱!").ShowDialog();
                    return;
                }
            }


            btn.IsEnabled = false;
            btnSendRegEmail.IsEnabled = false;
            btnSendResetEmail.IsEnabled = false;
            try
            {
                if (BtnStateTimer != null) BtnStateTimerTimingStop();
            }
            catch { }
            BtnStateTimerTiming();
        }

        #region 发送验证码按钮计时器
        System.Threading.Timer BtnStateTimer;
        public int TimeCount;
        delegate void SetValue();
        private void BtnStateTimerTimerUp(object state)
        {
            Dispatcher.Invoke(new SetValue(BtnStateTimerShowTime));
        }
        public void BtnStateTimerShowTime()
        {
            TimeCount--;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                btnSendRegEmail.Content = TimeCount;
                btnSendResetEmail.Content = TimeCount;
            }));
            if (TimeCount == 1)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    btnSendRegEmail.Content = "发送";
                    btnSendResetEmail.Content = "发送";
                    btnSendRegEmail.IsEnabled = true;
                    btnSendResetEmail.IsEnabled = true;
                }));
                BtnStateTimerTimingStop();
            }

        }
        //开始计时
        private void BtnStateTimerTiming()
        {
            BtnStateTimer = new System.Threading.Timer(new TimerCallback(BtnStateTimerTimerUp), null, Timeout.Infinite, 1000);
            TimeCount = 60;
            BtnStateTimer.Change(0, 1000);
        }
        //停止计时
        private void BtnStateTimerTimingStop()
        {
            BtnStateTimer.Change(Timeout.Infinite, 1000);
        }

        #endregion

        #region 验证码十分钟有效性计时器
        System.Threading.Timer CodeAvailabilityTimer;
        public int CodeAvailabilityTimerCount;
        delegate void CodeAvailabilityTimerSetValue();
        private void CodeAvailabilityTimerTimerUp(object state)
        {
            Dispatcher.Invoke(new CodeAvailabilityTimerSetValue(CodeAvailabilityTimerShowTime));
        }
        public void CodeAvailabilityTimerShowTime()
        {
            CodeAvailabilityTimerCount--;
            Console.WriteLine(CodeAvailabilityTimerCount);
            if (CodeAvailabilityTimerCount == 1)
            {
                currentRegCode = CreateRandom(6);
                currentResetCode = CreateRandom(6);
                CodeAvailabilityTimerTimingStop();
            }

        }
        //开始计时
        private void CodeAvailabilityTimerTiming()
        {
            CodeAvailabilityTimer = new System.Threading.Timer(new TimerCallback(CodeAvailabilityTimerTimerUp), null, Timeout.Infinite, 60000);
            CodeAvailabilityTimerCount = 11;
            CodeAvailabilityTimer.Change(0, 60000);
        }
        //停止计时
        private void CodeAvailabilityTimerTimingStop()
        {
            CodeAvailabilityTimer.Change(Timeout.Infinite, 1000);
        }

        #endregion

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!IsEmail(tbRegMail.Text.Trim()))
            {
                new MessageBoxView("请输入有效的邮箱号!").ShowDialog();
                return;
            }
            if (string.IsNullOrEmpty(pwdReg.Password))
            {
                new MessageBoxView("请输入密码!").ShowDialog();
                return;
            }
            if (string.IsNullOrEmpty(tbRegCode.Text))
            {
                new MessageBoxView("请输入验证码!").ShowDialog();
                return;
            }

            if (tbRegCode.Text.Trim() == CurrentRegCode)
            {
                try
                {
                    var user = userBLL.Find(EncryptsHelper.Encrypt(tbRegMail.Text));
                    if (user != null)
                    {
                        new MessageBoxView("此邮箱已被注册,请勿重复注册!").ShowDialog();
                    }
                    else
                    {
                        DateTime dt = DateTime.Now;
                        DateTime blockTime = dt.AddDays(3);
                        T_User newUser = new T_User()
                        {
                            ID = Guid.NewGuid().ToString("N"),
                            EMail = EncryptsHelper.Encrypt(tbRegMail.Text.Trim()),
                            Pwd = EncryptsHelper.Encrypt(pwdReg.Password.ToString().Trim()),
                            CreatTime = SqlHelper.GetDateTimeFromSQL().ToString("yyyy-MM-dd HH:mm:ss"),
                            LoginCount = 0,
                            State = "正常",
                            BlockingTime = blockTime.ToString("yyyy-MM-dd HH:mm:ss")
                        };
                        if (userBLL.Add(newUser))
                        {
                            new MessageBoxView("注册成功!").ShowDialog();
                            tabControl1.SelectedIndex = 0;
                        }
                        try
                        {
                            if (CodeAvailabilityTimer != null) CodeAvailabilityTimerTimingStop();
                        }
                        catch { }
                    }
                }
                catch(Exception ex)
                {
                    new MessageBoxView("注册失败,请稍后重试!").ShowDialog();
                }

            }
            else
            {
                new MessageBoxView("验证码错误,请尝试重新发送!").ShowDialog();
            }

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            if (!IsEmail(tbResetMail.Text.Trim()))
            {
                new MessageBoxView("请输入有效的邮箱号!").ShowDialog();
                return;
            }
            if (string.IsNullOrEmpty(pwdReset.Password.ToString()))
            {
                new MessageBoxView("请输入密码!").ShowDialog();
                return;
            }
            if (string.IsNullOrEmpty(tbResetCode.Text))
            {
                new MessageBoxView("请输入验证码!").ShowDialog();
                return;
            }

            if (tbResetCode.Text.Trim() == CurrentResetCode)
            {
                try
                {
                    var user = userBLL.Find(EncryptsHelper.Encrypt(tbLogin.Text));
                    if (user == null)
                    {
                        new MessageBoxView("出现异常!请稍后重试!").ShowDialog();
                        return;
                    }
                    else
                    {
                        user.Pwd = EncryptsHelper.Encrypt(pwdReset.Password.ToString());
                        if (userBLL.Update(user))
                        {
                            new MessageBoxView("修改成功!").ShowDialog();
                            tabControl1.SelectedIndex = 0;
                        }
                    }

                    try
                    {
                        if (CodeAvailabilityTimer != null) CodeAvailabilityTimerTimingStop();
                    }
                    catch { }
                }
                catch
                {
                    new MessageBoxView("修改失败,请稍后重试!").ShowDialog();
                }
            }
            else
            {
                new MessageBoxView("验证码错误,请尝试重新发送!").ShowDialog();
            }

        }

        public bool IsEmail(string mail)
        {
            return Regex.IsMatch(mail, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.IgnoreCase);
        }

        private void cbAutoLogin_Checked(object sender, RoutedEventArgs e)
        {
            if (cbAutoLogin.IsChecked == true)
            {
                GlobalCache.Config.Set("AutoLogin", true.ToString());
            }
            else
            {
                GlobalCache.Config.Set("AutoLogin", false.ToString());
            }
            GlobalCache.Config.Save();
        }
    }
}
