using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace Game
{
    public class SocketManager
    {
        public static SocketManager Instance { get { return Singleton.GetInstance<SocketManager>(); } }
        public const int portNo = 500;
        public SocketClient User;
        private Thread socketThead;
        public void Start()
        {
            socketThead = new Thread(StartNet);
            socketThead.Start();
            Loger.Log(GetIPAddress());
        }
        private void StartNet()
        {
            Debug.Log("开启一个线程,初始化服务器！");
            // 初始化服务器IP
            System.Net.IPAddress localAdd = System.Net.IPAddress.Parse(GetIPAddress());
            // 创建TCP侦听器
            TcpListener listener = new TcpListener(localAdd, portNo);
            listener.Start();
            // 显示服务器启动信息
            Debug.Log("Server is starting...n");
            // 循环接受客户端的连接请求
            while (true)
            {
                User = new SocketClient(listener.AcceptTcpClient());
                // 显示连接客户端的IP与端口
                Debug.Log(User._clientIP + " is joined...n");
                Thread.Sleep(1000);
            }
        }

        public void Destroy()
        {
            socketThead.Abort();
        }

        private String GetIPAddress()
        {
            String str;
            String Result = "";
            String hostName = Dns.GetHostName();
            IPAddress[] myIP = Dns.GetHostAddresses(hostName);
            foreach (IPAddress address in myIP)
            {
                str = address.ToString();
                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] >= '0' && str[i] <= '9' || str[i] == '.') Result = str;
                }
            }
            return Result;
        }
    }



    public class SocketClient 
    {
        public static Hashtable ALLClients = new Hashtable(); // 客户列表
        private TcpClient _client; // 客户端实体
        public string _clientIP; // 客户端IP
        private string _clientNick; // 客户端昵称
        private byte[] data; // 消息数据
        private bool ReceiveNick = true;
        public SocketClient(TcpClient client)
        {
            this._client = client;
            this._clientIP = client.Client.RemoteEndPoint.ToString();
            // 把当前客户端实例添加到客户列表当中
            ALLClients.Add(this._clientIP, this);
            data = new byte[this._client.ReceiveBufferSize];
            // 从服务端获取消息
            client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this._client.ReceiveBufferSize), ReceiveMessage, null);
        }
        // 从客戶端获取消息
        public void ReceiveMessage(IAsyncResult ar)
        {
            int bytesRead;
            try
            {
                lock (this._client.GetStream())
                {
                    bytesRead = this._client.GetStream().EndRead(ar);
                }
                if (bytesRead < 1)
                {
                    ALLClients.Remove(this._clientIP);
                    Broadcast(this._clientNick + " has left the chat");
                    return;
                }
                else
                {
                    string messageReceived = System.Text.Encoding.ASCII.GetString(data, 0, bytesRead);
                    if (ReceiveNick)
                    {
                        this._clientNick = messageReceived;
                        Broadcast(this._clientNick + " has joined the chat.");
                        //this.sendMessage("hello");
                        ReceiveNick = false;
                    }
                    else
                    {
                        Broadcast(this._clientNick + ">" + messageReceived);
                    }
                }
                lock (this._client.GetStream())
                {
                    this._client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this._client.ReceiveBufferSize), ReceiveMessage, null);
                }
            }
            catch (Exception ex)
            {
                ALLClients.Remove(this._clientIP);
                Broadcast(this._clientNick + " has left the chat.");
            }
        }
        // 向客戶端发送消息
        public void sendMessage(string message)
        {
            try
            {
                System.Net.Sockets.NetworkStream ns;
                lock (this._client.GetStream())
                {
                    ns = this._client.GetStream();
                }
                // 对信息进行编码
                byte[] bytesToSend = System.Text.Encoding.ASCII.GetBytes(message);
                ns.Write(bytesToSend, 0, bytesToSend.Length);
                ns.Flush();
            }
            catch (Exception ex)
            {
            }
        }
        // 向客户端广播消息
        public void Broadcast(string message)
        {
            Debug.Log(message);
            foreach (DictionaryEntry c in ALLClients)
            {
                ((SocketClient)(c.Value)).sendMessage(message + Environment.NewLine);
            }
        }
    }
}