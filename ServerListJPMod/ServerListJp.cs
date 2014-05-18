using System;
using System.IO;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
namespace ServerListJp
{
//	[ApiVersion(1, 12)]
	[ApiVersion(1, 16)]
	public class ServerListJp : TerrariaPlugin
	{
		private static string author = "haru_arc";
		public static string description = "Japanese Server List";
		public static string name = "ServerListJp";
		public static string version = "0.4.6.9999";
		internal static Properties prop = null;
		internal CommunicateServerList communicateServerList;
		public override string Author
		{
			get
			{
				return ServerListJp.author;
			}
		}
		public override string Description
		{
			get
			{
				return ServerListJp.description;
			}
		}
		public override string Name
		{
			get
			{
				return ServerListJp.name;
			}
		}
		public override Version Version
		{
			get
			{
				return new Version(0, 4, 5, 0);
			}
		}
		public ServerListJp(Main main) : base(main)
		{
		}
		public override void Initialize()
		{
			ServerListJp.prop = this.loadProperties();
			Console.WriteLine("[{0}] Initialized Plugin - {0} ver {1}", this.Name, this.Version);
			Log.Info(string.Format("[{0}] Initialized Plugin - {0} ver {1}", this.Name, this.Version));
			this.communicateServerList = new CommunicateServerList(this);
		}
		private Properties loadProperties()
		{
			Properties properties = new Properties("ServerListJp");
			if (File.Exists("ServerListJp.properties"))
			{
				properties.loadProperties();
				if (!properties.ContainsKey("ServerName"))
				{
					properties.registerProperty("ServerName", "");
				}
				if (!properties.ContainsKey("HostName"))
				{
					properties.registerProperty("HostName", "");
				}
				if (!properties.ContainsKey("Description"))
				{
					properties.registerProperty("Description", "");
				}
				if (!properties.ContainsKey("ServerWeb"))
				{
					properties.registerProperty("ServerWeb", "");
				}
				if (!properties.ContainsKey("Silent"))
				{
					properties.registerProperty("Silent", "false");
				}

			}
			else
			{
				properties.registerProperty("ServerName", "");
				properties.registerProperty("HostName", "");
				properties.registerProperty("Description", "");
				properties.registerProperty("ServerWeb", "");
				properties.registerProperty("Silent", "false");
			}
			return properties;
		}
		protected override void Dispose(bool disposing)
		{
			if (this.communicateServerList != null)
			{
				this.communicateServerList.Dispose();
			}
			Console.WriteLine("[{0}] Unloaded", this.Name);
		}
	}
}
