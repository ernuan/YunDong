using CloudsMove.Common;
using CloudsMove.Interops;
using CloudsMove.ViewModels;
using CloudsMove.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Threading;

namespace CloudsMove.Controllers
{
    public class SSRController
    {
        public Process controller;
        public T_ShadowsocksR ssr;
        public SSRController(T_ShadowsocksR ssrr)
        {
            ssr = ssrr;
        }
        public bool Start()
        {
            string HostName = ssr.HostName;
            string Port = ssr.Port;
            string Password = ssr.Password;
            string Method = ssr.Method;
            string Protocol = ssr.Protocol;
            string ProtocolParam = ssr.ProtocolParam;
            string Obfs = ssr.OBFS;
            string OBFSParam = ssr.OBFSParam;

            string arguments = $"-s {HostName} -p {Port} -k \"{Password}\" -m {Method} -t 120 -b 127.0.0.1 -l {GlobalCache.Port} -u";
            if (!string.IsNullOrEmpty(Protocol))
            {
                arguments += $" -O {Protocol}";
                if (!string.IsNullOrEmpty(ProtocolParam)) arguments += $" -G \"{ProtocolParam}\"";
            }
            if (!string.IsNullOrEmpty(Obfs))
            {
                arguments += $" -o {Obfs}";
                if (!string.IsNullOrEmpty(OBFSParam)) arguments += $" -g \"{OBFSParam}\"";
            }
            string path = System.Environment.CurrentDirectory;
            controller = new Process();
            controller.StartInfo.FileName = path + @"\bin\YD_socks.exe";
            controller.StartInfo.Arguments = arguments.ToString();
            controller.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            controller.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            controller.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            controller.StartInfo.CreateNoWindow = true;//不显示程序窗口

            controller.Start();
            return true;
        }

        /// <summary>
        ///		停止
        /// </summary>
        public void Stop()
        {
            try
            {
                if (controller != null && !controller.HasExited)
                {
                    controller.Kill();
                }
            }
            catch
            {

            }
        }

    }
}
