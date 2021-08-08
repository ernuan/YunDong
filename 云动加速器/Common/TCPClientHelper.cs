using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudsMove.Common
{
    public enum MsgType
    {
        Login,
        LogOut,

        Heart,
    }
    public class TCPClientHelper
    {
        public Socket ClientSocket;

        public bool Init()
        {
            //1 创建一个Socket
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ClientSocket = socket;
            //2 绑定连接
            try
            {
                socket.Connect(IPAddress.Parse("127.0.0.1"), int.Parse("6666"));
                //3 开始接收客户端的连接
                Thread thread = new Thread(ReciveData);
                thread.IsBackground = true;
                thread.Start(ClientSocket);
                return true;
            }
            catch
            {
                
            }
            return false;

        }

        public void StopConnect()
        {
            if (ClientSocket.Connected)
            {
                ClientSocket.Shutdown(SocketShutdown.Both);
                ClientSocket.Close(100);
            }
        }

        private void ReciveData(object socket)
        {
            var proxSocket = socket as Socket;
            byte[] data = new byte[1024 * 1024];
            while (true)
            {
                int len = 0;
                try
                {
                    len = proxSocket.Receive(data, 0, data.Length, SocketFlags.None);
                }
                catch
                {
                    StopConnect();
                    return;
                }
                if (len <= 0)
                {
                    StopConnect();
                    return;
                }
                string str = Encoding.UTF8.GetString(data, 0, len);
            }
        }

        public void SendMsg(MsgType msgType)
        {
            Task.Run(new Action(()=> 
            {
                if (ClientSocket.Connected)
                {
                    byte[] data;
                    string msg;
                    string id="--";
                    if (GlobalCache.CurrentUser != null)
                    {
                        id = GlobalCache.CurrentUser.ID;
                    }

                    if(msgType == MsgType.Heart)
                    {
                        msg = id + "|在线|";
                        data = Encoding.UTF8.GetBytes(msg);
                        ClientSocket.Send(data, 0, data.Length, SocketFlags.None);
                    }
                    else
                    {
                        if (msgType == MsgType.Login)
                        {
                            msg = id + "|登录|";
                            //data = Encoding.UTF8.GetBytes(GlobalCache.CurrentUser.ID + "|登录");
                        }
                        else
                        {
                            msg = id + "|退出|";
                            //data = Encoding.UTF8.GetBytes(GlobalCache.CurrentUser.ID + "|退出");
                        }

                        for (int i = 1; i < 6; i++)
                        {
                            string msgfull = msg + i;
                            data = Encoding.UTF8.GetBytes(msgfull);
                            ClientSocket.Send(data, 0, data.Length, SocketFlags.None);
                            Thread.Sleep(50);
                        }
                    }

                    
                }
            }));
        }
    }
}
