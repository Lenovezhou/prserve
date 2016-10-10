using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LibNet
{
	public class AConnector
	{
		Socket connector;
		public bool Connected
		{
			get
			{
				return connector != null && connector.Connected;
			}
		}
		public void StartAsServer(int port, string identifiedIP = null)
		{
			IPAddress ip;
			if (string.IsNullOrEmpty(identifiedIP))
			{
				ip = IPAddress.Any;
			}
			else
			{
				ip = IPAddress.Parse(identifiedIP);
			}

			connector = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			connector.Bind(new IPEndPoint(ip, port));

			(new Thread(new ThreadStart(StartListening))).Start();
		}
		private void StartListening()
		{
			connector.Listen(1024);

			while (true)
			{
				Socket client = connector.Accept();

				(new Thread(new ThreadStart((new AReceiveHandler(client)).OnStartToReceive))).Start();
			}
		}
		public void OnServerSend(Socket socket, MemoryStream ms)
		{
			OnSend(socket, ms);
		}

		public void StartAsClient(string ip, int port)
		{
			connector = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			connector.Connect(new IPEndPoint(IPAddress.Parse(ip), port));

			(new AReceiveHandler(connector)).OnStartToReceive();
		}
		public void OnClientSend(MemoryStream ms)
		{
			OnSend(connector, ms);
		}

		private void OnSend(Socket s, MemoryStream ms)
		{
			(new ASendHandler(s)).StartToSend(ms);
		}
	}
}
