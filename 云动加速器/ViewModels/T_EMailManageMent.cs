using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudsMove.ViewModels
{
    public class T_EMailManageMent
    {
        public string SerialCode { get; set; }

        public string EMailSendMode { get; set; } //smtp.exmail.qq.com, smtp.qq.com, smtp.126.com等等

        public string EmailAccount { get; set; }  //账户

        public string EmailPwd { get; set; }  //密码或授权码

        public string EmailTemplate { get; set; } //邮件发送模板
    }
}
