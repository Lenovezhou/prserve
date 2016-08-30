using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Net.Sockets;
public class ClientHandler : MonoBehaviour
{
	private TcpClient _client;
	byte[] data;
    public string Ip = "192.168.1.174";
    public int portNo = 500;
    public string NetName = "Client_01";
	public string message = "";
	public string sendMsg = "";
    public string netMsg = null;
    void Start()
    {
        this._client = new TcpClient();
        this._client.Connect(Ip, portNo);
        data = new byte[this._client.ReceiveBufferSize];
        //SendMessage(txtNick.Text);
        SendMessage(NetName);
        this._client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this._client.ReceiveBufferSize), ReceiveMessage, null);
    }

    void Update()
    {
        if (netMsg != null)
        {
            Game.GameSignalMgr.Instance.MachineControl.Dispatch(netMsg);
            netMsg = null;
        }
    }

    void OnGUI()
	{
        //nickName = GUI.TextField(new Rect(10, 10, 100, 20), nickName);
        message = GUI.TextArea(new Rect(10, 40, 300, 200), message);
        sendMsg = GUI.TextField(new Rect(10, 250, 210, 20), sendMsg);
        if (GUI.Button(new Rect(230, 250, 80, 20), "Send"))
		{
			SendMessage(sendMsg);
			sendMsg = "";
		};
    }
	public void SendMessage(string message)
	{
		try
		{
			NetworkStream ns = this._client.GetStream();
			byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
			ns.Write(data, 0, data.Length);
			ns.Flush();
		}
		catch (Exception ex)
		{
			//MessageBox.Show(ex.ToString());
		}
	}
	public void ReceiveMessage(IAsyncResult ar)
	{
		try
		{
			int bytesRead;
			bytesRead = this._client.GetStream().EndRead(ar);
			if (bytesRead < 1)
			{
				return;
			}
			else
			{
				Debug.Log(System.Text.Encoding.ASCII.GetString(data, 0, bytesRead));
                netMsg= System.Text.Encoding.ASCII.GetString(data, 0, bytesRead);;
			    message = netMsg;
                netMsg = netMsg.Replace("[", "").Replace("]", "");
			    Debug.LogWarning("接收到服务器的数据信息："+ netMsg);
			}
			this._client.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this._client.ReceiveBufferSize), ReceiveMessage, null);
        }
		catch (Exception ex)
		{
		    Loger.LogError(ex.Message);
		}
    }
}