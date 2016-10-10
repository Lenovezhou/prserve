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
	public class AReceiveHandler
	{
		private Socket socket;
		/// <summary>
		/// 初始化构造方法
		/// </summary>
		/// <param name="chat">套接字</param>
		public AReceiveHandler(Socket chat)
		{
			this.socket = chat;
			OnStartToReceive();
		}

		public void OnStartToReceive()
		{
			byte[] bs = new byte[1024];
			try
			{
				socket.BeginReceive(bs, 0, bs.Length, SocketFlags.None, DoReceiveComplete, bs);
			}
			catch (Exception ex)
			{

			}
		}

		internal static void OnUnregistPktHandler(int iPktType)
		{
			if (dPktHandlers.ContainsKey(iPktType))
			{
				dPktHandlers.Remove(iPktType);
			}
		}

		public delegate void delegatePktHandler(Socket s, BinaryReader br);
		private static Dictionary<int, delegatePktHandler> dPktHandlers = new Dictionary<int, delegatePktHandler>();
		public static void OnRegistPktHandler(int iPktType, delegatePktHandler del)
		{
			dPktHandlers.Add(iPktType, del);
		}
		private MemoryStream localStream = new MemoryStream();
		private bool bH1;
		private bool bH2;
		private bool[] bPktLengthValued = new bool[4];
		private byte[] pktLength = new byte[4];
		private int iPktLength;
		private bool bPacketHeadValid;
		private object receiveLock = new object();
		List<byte[]> lReceivedDatas = new List<byte[]>();
		private void DoReceiveComplete(IAsyncResult ar)
		{
			try
			{
				byte[] buff = (byte[])ar.AsyncState;
				int len = socket.EndReceive(ar);
				if (len > 0)
				{
					byte[] data = new byte[len];
					Array.ConstrainedCopy(buff, 0, data, 0, len);
					lock (receiveLock)
					{
						lReceivedDatas.Add(data);
						ThreadPool.QueueUserWorkItem(new WaitCallback(DoProcessReceivedDatas));
					}
				}
				else
				{
					//
				}
			}
			catch
			{

			}
			finally
			{
				OnStartToReceive();
			}
		}

		private void DoProcessReceivedDatas(object o)
		{
			lock (receiveLock)
			{
				byte[] buff;
				if (lReceivedDatas.Count == 0)
				{
					return;
				}
				buff = lReceivedDatas[0];
				lReceivedDatas.RemoveAt(0);
				int len = buff.Length;

				long rawPos = localStream.Position;
				localStream.Position = localStream.Length;
				localStream.Write(buff, 0, len);
				localStream.Position = rawPos;
				bNeedToProcessPacket = true;
				try
				{
					while (bNeedToProcessPacket)
					{
						if (!bPacketHeadValid)
						{
							if (PacketHeadProcess())
							{
								PacketProcess();
							}
							else
							{
								//has invalid packet;

								ClearLocalStream();
								break;
							}
						}
						else
						{
							PacketProcess();
						}
					}
				}
				catch
				{
					ClearLocalStream();
				}
			}

		}

		bool localStreamCanRead
		{
			get
			{
				return localStream.Position < localStream.Length - 1;
			}
		}
		long localStreamByteCountUnread
		{
			get
			{
				return localStream.Length - localStream.Position;
			}
		}
		private void ResetPacketHead()
		{
			bH1 = false;
			bH2 = false;
			bPacketHeadValid = false;
			for (int i = 0; i < bPktLengthValued.Length; i++)
			{
				bPktLengthValued[i] = false;
			}
		}
		private bool PacketHeadProcess()
		{
			if (!bH1)
			{
				if (localStreamCanRead)
				{
					bH1 = localStream.ReadByte() == ANetConsts.H1;
					if (!bH1)
					{
						ClearLocalStream();
						throw new Exception("Invalid H1");
					}
				}
			}
			if (!bH2)
			{
				if (localStreamCanRead)
				{
					bH2 = localStream.ReadByte() == ANetConsts.H2;
					if (!bH2)
					{
						ClearLocalStream();
						throw new Exception("Invalid H2");
					}
				}
			}

			for (int i = 0; i < bPktLengthValued.Length; i++)
			{
				if (bPktLengthValued[i])
				{
					continue;
				}
				if (!localStreamCanRead)
				{
					return false;
				}
				bPktLengthValued[i] = true;
				pktLength[i] = (byte)localStream.ReadByte();

			}
			if (localStreamCanRead)
			{
				bPacketHeadValid = true;
				iPktLength = BitConverter.ToInt32(pktLength, 0);
			}
			return true;
		}

		private void PacketProcess()
		{
			if (iPktLength > localStreamByteCountUnread)
			{
				bNeedToProcessPacket = false;
				return;
			}
			byte[] pktTypeDatas = new byte[4];
			for (int i = 0; i < pktTypeDatas.Length; i++)
			{
				pktTypeDatas[i] = (byte)localStream.ReadByte();
			}
			int pktType = BitConverter.ToInt32(pktTypeDatas, 0);
			byte[] data = new byte[iPktLength - 4];//minus pkt type length
			localStream.Read(data, 0, data.Length);
			ResetLocalStream();

			CreatePacket(pktType, data);
		}

		private static Action<Socket, int, byte[]> packetCreateHandler;
		public static void OnRegistPacketCreateHandler(Action<Socket, int, byte[]> action)
		{
			packetCreateHandler = action;
		}
		private void CreatePacket(int pktType, byte[] data)
		{
			if (packetCreateHandler == null)
			{
				return;
			}
			packetCreateHandler(socket, pktType, data);
		}

		bool bNeedToProcessPacket = false;
		void ResetLocalStream()
		{
			ResetPacketHead();
			if (localStreamCanRead)
			{
				bNeedToProcessPacket = true;
			}
			else
			{
				bNeedToProcessPacket = false;
				localStream.Close();
				localStream = new MemoryStream();
			}
		}
		void ClearLocalStream()
		{
			ResetPacketHead();
			bNeedToProcessPacket = false;
			localStream.Close();
			localStream = new MemoryStream();
		}
	}

}