using CloudsMove.ViewModels;
using System.Collections.Generic;

namespace CloudsMove.Common
{
    public class GlobalCache
    {
        public static TCPClientHelper TCPClient = new TCPClientHelper();

        public static T_User CurrentUser = null;

        public static T_Settings CurrentSetting = new T_Settings();

        public static Config Config = new Config("Config.data");

        public static T_EMailManageMent EMailManageMent = null;

        public static List<T_Game> GamesList = new List<T_Game>();

        public static List<T_Game> MyGamesList = new List<T_Game>();

        public static List<T_ShadowsocksR> SSRList = new List<T_ShadowsocksR>();

        public static T_ShadowsocksR CurrentSSR = null;

        public static T_Game CurrentGame = null;

        public static List<string> CurrentRoute = new List<string>();

        public static int Port { get; set; }

        public static List<T_Advertising> Advertising = new List<T_Advertising>();

        public static string MD5 { get; set; } = "26a6fa2c497425a1e11b525f86c47485";
    }
}
