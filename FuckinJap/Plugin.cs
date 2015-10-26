using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;
using Common = MackerelPluginSet.Common;

namespace MackerelPluginSet.NoJP {

	//*
	[ApiVersion(1, 22)]
	public class Plugin : TerrariaPlugin {

		[Serializable]
		[Common.ConfigurationFile("NoJP.xml")]
		public class Configuration {
			public bool Enabled = false;
			public string LoginKickMessage = "this server is not for you.";
			public string ChatKickMessage = "this server is not for you.";
		}

		Configuration config;

		public Plugin(Main game)
			: base(game) {
		}

		public override void Initialize() {
			config = Common.Configuration.Load<Configuration>(Common.ConfigurationLoadMode.CreateIfFailed);

			ServerApi.Hooks.ServerJoin.Register(this, ServerHooks_Join);
			ServerApi.Hooks.ServerChat.Register(this, ServerHooks_Chat);
			TS.Hooks.GeneralHooks.ReloadEvent += GeneralHooks_ReloadEvent;

			TS.TShock.Log.ConsoleInfo("Mackerel NoJP Plugin is loaded.");

		}

		void GeneralHooks_ReloadEvent(TS.Hooks.ReloadEventArgs e) {
			config = Common.Configuration.Load<Configuration>(Common.ConfigurationLoadMode.CreateIfFailed);
		}

		private void ServerHooks_Chat(ServerChatEventArgs args) {
			if (!config.Enabled) return;

			var player = TS.TShock.Players[args.Who];

			if (Judge(args.Text)) {
				player.Disconnect(config.ChatKickMessage);
			}
		}

		private void ServerHooks_Join(JoinEventArgs args) {
			if (!config.Enabled) return;

			var player = TS.TShock.Players[args.Who];

			if (Judge(player.Name)) {
				player.Disconnect(config.LoginKickMessage);
			}
		}

		private static bool Judge(string s) {
			return s.Any(c => (int)c > 0x7f);
		}

		public override Version Version {
			get { return new Version("1.0"); }
		}
		public override string Name {
			get { return "Mackerel NoJP Plugin"; }
		}
		public override string Author {
			get { return "Totoki Kei"; }
		}
		public override string Description {
			get { return "********************"; }
		}
	}
	//*/
}
