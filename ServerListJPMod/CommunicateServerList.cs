using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using Terraria;
using TShockAPI;
namespace ServerListJp
{
	internal class CommunicateServerList : IDisposable
	{
		private string baseUrl = "http://terraria.arcenserv.info/sl/servers";
		private string reg;
		private Thread heartBeatThread;
		public bool IsSilent;
		private int retry = 5;
		private int retryCount;
		private ServerListJp ServerListJp;
		public CommunicateServerList(ServerListJp ServerListJp)
		{
			this.heartBeatThread = new Thread(new ThreadStart(this.sendHeartbeat));
			this.heartBeatThread.Start();
			this.ServerListJp = ServerListJp;

			IsSilent = ServerListJp.prop.getBoolean("Silent");
		}
		private void sendHeartbeat()
		{
			int millisecondsTimeout = 3000;
			while (this.retryCount < this.retry)
			{
				if (Main.worldSurface > 0.0)
				{
					if (this.reg == null)
					{
						this.reg = Path.GetRandomFileName();
						try
						{
							this.RegistServer(this.reg);
							Console.WriteLine("[{0}] Register ServerList", this.ServerListJp.Name);
							Log.Info(string.Format("[{0}] Register ServerList", this.ServerListJp.Name));
							this.retryCount = 0;
						}
						catch (Exception)
						{
							Console.WriteLine("[{0}] Register Failed", this.ServerListJp.Name);
							Log.Error(string.Format("[{0}] Register Failed", this.ServerListJp.Name));
							this.retryCount++;
						}
						millisecondsTimeout = 60000;
					}
					else
					{
						try
						{
							if (!this.IsSilent)
							{
								Console.WriteLine("[{0}] Sending Heartbeat", this.ServerListJp.Name);
							}
							this.SendHeartBeat(this.reg);
							this.retryCount = 0;
						}
						catch (Exception)
						{
							Console.WriteLine("[{0}] Sending Heartbeat Failed", this.ServerListJp.Name);
							Log.Error(string.Format("[{0}] Sending Heartbeat Failed", this.ServerListJp.Name));
							this.retryCount++;
						}
					}
				}
				Thread.Sleep(millisecondsTimeout);
			}
		}
		private void SubmitData(string url, Hashtable ht)
		{
			string text = "";
			foreach (string text2 in ht.Keys)
			{
				if (ht[text2] != null && !(ht[text2].ToString() == ""))
				{
					text += string.Format("{0}={1}&", text2, HttpUtility.UrlEncode(ht[text2].ToString()));
				}
			}
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Proxy = null;
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			httpWebRequest.ContentLength = (long)bytes.Length;
			using (Stream requestStream = httpWebRequest.GetRequestStream())
			{
				requestStream.Write(bytes, 0, bytes.Length);
			}
			using (WebResponse response = httpWebRequest.GetResponse())
			{
				Stream responseStream = response.GetResponseStream();
				new StreamReader(responseStream);
			}
		}
		public void SendHeartBeat(string regist)
		{
			Hashtable hashtable = new Hashtable();
			hashtable["data[Server][current]"] = this.CountConnectedUsers();
			hashtable["data[Server][version]"] = "1";
			this.SubmitData(this.baseUrl + "/edit/" + regist, hashtable);
		}
		public void RegistServer(string regist)
		{
			string text = null;
			string value = null;
			string value2 = null;
			string value3 = null;
			ServerListJp.prop.TryGetValue("ServerName", out text);
			ServerListJp.prop.TryGetValue("Description", out value);
			ServerListJp.prop.TryGetValue("HostName", out value2);
			ServerListJp.prop.TryGetValue("ServerWeb", out value3);
			Hashtable hashtable = new Hashtable();
			hashtable["data[Server][regist]"] = regist;
			hashtable["data[Server][worldname]"] = ((text == null || text == "") ? Main.worldName : text);
			hashtable["data[Server][max]"] = TShock.Config.MaxSlots;
			hashtable["data[Server][current]"] = this.CountConnectedUsers();
			hashtable["data[Server][size]"] = Main.worldSurface;
			hashtable["data[Server][desc]"] = value;
			hashtable["data[Server][hostname]"] = value2;
			hashtable["data[Server][url]"] = value3;
			hashtable["data[Server][version]"] = "1";
			hashtable["data[Server][port]"] = Netplay.serverPort;
			hashtable["data[Server][type]"] = "2";
			this.SubmitData(this.baseUrl + "/add", hashtable);
		}
		public int CountConnectedUsers()
		{
			int num = 0;
			ServerSock[] serverSock = Netplay.serverSock;
			for (int i = 0; i < serverSock.Length; i++)
			{
				ServerSock serverSock2 = serverSock[i];
				if (serverSock2 != null)
				{
					num = (serverSock2.active ? (num + 1) : num);
				}
			}
			return num;
		}
		public void Dispose()
		{
			if (this.heartBeatThread != null)
			{
				this.heartBeatThread.Abort();
				this.heartBeatThread = null;
			}
		}
	}
}
