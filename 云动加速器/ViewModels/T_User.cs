

using System;

namespace CloudsMove.ViewModels
{
    public class T_User
    {
        public string ID { get; set; }

        public string EMail { get; set; }

        public string Pwd { get; set; }

        public string CreatTime { get; set; }

        public string BlockingTime { get; set; }

        public int LoginCount { get; set; }

        public string State { get; set; }

        public string _QQHead { get; set; } 
    }
}
