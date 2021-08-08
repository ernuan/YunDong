﻿using CloudsMove.Common;
using CloudsMove.Interops;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using static CloudsMove.Interops.Redirector;

namespace CloudsMove.Controllers
{
    public class NFController 
    {
        private static readonly ServiceController NFService = new ServiceController("netfilter2");

        private const string BinDriver = "bin\\nfdriver.sys";
        private static readonly string SystemDriver = $"{Environment.SystemDirectory}\\drivers\\netfilter2.sys";

        public string Name { get; } = "Redirector";

        public void Start(in List<string> mode)
        {
            CheckDriver();

            Dial(NameList.TYPE_FILTERLOOPBACK, "false");
            Dial(NameList.TYPE_FILTERICMP, "true");
            var p = PortHelper.GetAvailablePort();
            Dial(NameList.TYPE_TCPLISN, p.ToString());
            Dial(NameList.TYPE_UDPLISN, p.ToString());

            // Server
            Dial(NameList.TYPE_FILTERUDP, (2 > 1).ToString().ToLower());
            Dial(NameList.TYPE_FILTERTCP, (2 > 1).ToString().ToLower());
            dial_Server(PortType.Both);

            // Mode Rule
            dial_Name(mode);

            // Features
            Dial(NameList.TYPE_DNSHOST, "1.1.1.1:53");

            if (!Init())
                throw new Exception("驱动初始化失败!");
        }

        public void Stop()
        {
            Free();
        }

        #region CheckRule

        /// <summary>
        /// </summary>
        /// <param name="r"></param>
        /// <param name="clear"></param>
        /// <returns>No Problem true</returns>
        private static bool CheckCppRegex(string r, bool clear = true)
        {
            try
            {
                if (r.StartsWith("!"))
                    return Dial(NameList.TYPE_ADDNAME, r.Substring(1));

                return Dial(NameList.TYPE_ADDNAME, r);
            }
            finally
            {
                if (clear)
                    Dial(NameList.TYPE_CLRNAME, "");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="results"></param>
        /// <returns>No Problem true</returns>
        public static bool CheckRules(IEnumerable<string> rules, out IEnumerable<string> results)
        {
            results = rules.Where(r => !CheckCppRegex(r, false));
            Dial(NameList.TYPE_CLRNAME, "");
            return !results.Any();
        }

        public static string GenerateInvalidRulesMessage(IEnumerable<string> rules)
        {
            return $"{string.Join("\n", rules)}\nAbove rules does not conform to C++ regular expression syntax";
        }

        #endregion

        private void dial_Server(in PortType portType)
        {
            if (portType == PortType.Both)
            {
                dial_Server(PortType.TCP);
                dial_Server(PortType.UDP);
                return;
            }

            int offset;

            if (portType == PortType.UDP) offset = UdpNameListOffset;
            else offset = 0;

            Dial(NameList.TYPE_TCPTYPE + offset, "Socks5");
            Dial(NameList.TYPE_TCPHOST + offset, $"127.0.0.1:{GlobalCache.Port}");
            Dial(NameList.TYPE_TCPUSER + offset, string.Empty);
            Dial(NameList.TYPE_TCPPASS + offset, string.Empty);
            Dial(NameList.TYPE_TCPMETH + offset, string.Empty);
        }

        private void dial_Name(List<string> mode)
        {
            Dial(NameList.TYPE_CLRNAME, "");
            var invalidList = new List<string>();
            foreach (var s in mode)
            {
                if (s.StartsWith("!"))
                {
                    if (!Dial(NameList.TYPE_BYPNAME, s.Substring(1)))
                        invalidList.Add(s);

                    continue;
                }

                if (!Dial(NameList.TYPE_ADDNAME, s))
                    invalidList.Add(s);
            }

            if (invalidList.Any())
                throw new Exception(GenerateInvalidRulesMessage(invalidList));

            string path = Environment.CurrentDirectory + "\\bin\\";
            Dial(NameList.TYPE_ADDNAME, @"NTT\.exe");
            Dial(NameList.TYPE_BYPNAME, "^" + path.ToRegexString() + @"((?!NTT\.exe).)*$");
        }

        #region DriverUtil

        private static void CheckDriver()
        {
            var binFileVersion = Utils.GetFileVersion(BinDriver);
            var systemFileVersion = Utils.GetFileVersion(SystemDriver);

            if (!File.Exists(SystemDriver))
            {
                // Install
                InstallDriver();
                return;
            }

            var reinstall = false;
            if (Version.TryParse(binFileVersion, out var binResult) && Version.TryParse(systemFileVersion, out var systemResult))
            {
                if (binResult.CompareTo(systemResult) > 0)
                    // Update
                    reinstall = true;
                else if (systemResult.Major != binResult.Major)
                    // Downgrade when Major version different (may have breaking changes)
                    reinstall = true;
            }
            else
            {
                // Parse File versionName to Version failed
                if (!systemFileVersion.Equals(binFileVersion))
                    // versionNames are different, Reinstall
                    reinstall = true;
            }

            if (!reinstall)
                return;

            UninstallDriver();
            InstallDriver();
        }

        /// <summary>
        ///     安装 NF 驱动
        /// </summary>
        /// <returns>驱动是否安装成功</returns>
        private static void InstallDriver()
        {

            if (!File.Exists(BinDriver))
                throw new Exception("builtin driver files missing, can't install NF driver");

            try
            {
                File.Copy(BinDriver, SystemDriver);
            }
            catch (Exception e)
            {
                throw new Exception($"Copy NF driver file failed\n{e.Message}");
            }

            // 注册驱动文件
            var result = NFAPI.nf_registerDriver("netfilter2");
            if (result == NF_STATUS.NF_STATUS_SUCCESS)
            {
                
            }
            else
            {
                throw new Exception($"Register NF driver failed\n{result}");
            }
        }

        /// <summary>
        ///     卸载 NF 驱动
        /// </summary>
        /// <returns>是否成功卸载</returns>
        public static bool UninstallDriver()
        {
            try
            {
                if (NFService.Status == ServiceControllerStatus.Running)
                {
                    NFService.Stop();
                    NFService.WaitForStatus(ServiceControllerStatus.Stopped);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            if (!File.Exists(SystemDriver))
                return true;

            NFAPI.nf_unRegisterDriver("netfilter2");
            File.Delete(SystemDriver);

            return true;
        }

        #endregion
    }
}