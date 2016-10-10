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
	public class ASendHandler
	{
		public ASendHandler(Socket sentSocket)
		{
			socketSent = sentSocket;
		}
		public void StartToSend(MemoryStream dataMS)
		{
			MemoryStream ms = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(ms);
			bw.Write(ANetConsts.H1);
			bw.Write(ANetConsts.H2);
			bw.Write((int)dataMS.Length);
			bw.Write(dataMS.GetBuffer(), 0, (int)dataMS.Length);
			byte[] bs = new byte[ms.Length];
			Array.ConstrainedCopy(ms.GetBuffer(), 0, bs, 0, bs.Length);
			AddToDataArray(bs);
		}

		private void AddToDataArray(byte[] data)
		{
			lock (olock)
			{
				lData.Add(data);

				DoSend();
			}
		}

		private void DoSend()
		{
			if (lData.Count == 0)
			{
				return;
			}
			byte[] bs = lData[0];
			lData.RemoveAt(0);
			DoRawSend(bs);
		}

		public Socket socketSent = null;
		List<byte[]> lData = new List<byte[]>();
		private object olock = new object();
		private void DoRawSend(byte[] data)
		{
			socketSent.BeginSend(data, 0, data.Length, SocketFlags.None, DoSendCompelete, null);
		}

		private void DoSendCompelete(IAsyncResult ar)
		{
			socketSent.EndSend(ar);

			lock (olock)
			{
				if (lData.Count > 0)
				{
					DoSend();
				}
			}
		}
	}

}