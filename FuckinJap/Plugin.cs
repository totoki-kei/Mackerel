using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;

namespace MackerelPluginSet.NoJP {

	//*
	[ApiVersion(1, 21)]
	public class Plugin : TerrariaPlugin {

		[Serializable]
		public class Configuration {
			public bool Enabled = false;
			public string LoginKickMessage = "this server is not for you.";
			public string ChatKickMessage = "this server is not for you.";

			[XmlIgnore]
			public static readonly string ConfigurationFileName = "NoJP.xml";

			public Configuration() { }

			public static Configuration Load() {
				XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
				Configuration cfg = null;
				if (!File.Exists(ConfigurationFileName)) {
					// ファイルがない

					cfg = new Configuration(); // 初期値
					try {
						// 初期値のファイルを作成
						using (var f = File.OpenWrite(ConfigurationFileName)) {
							serializer.Serialize(f, cfg);
						}
					}
					catch (IOException) {
						// にぎりつぶす
					}
				}
				else {
					try {
						using (var f = File.OpenRead(ConfigurationFileName)) {
							cfg = serializer.Deserialize(f) as Configuration;
						}
					}
					catch (IOException) {
						// デフォルト値を返す
						cfg = new Configuration();
					}

				}

				return cfg;

			}


		}

		Configuration config;

		public Plugin(Main game)
			: base(game) {
		}

		public override void Initialize() {
			config = Configuration.Load();

			ServerApi.Hooks.ServerJoin.Register(this, ServerHooks_Join);
			ServerApi.Hooks.ServerChat.Register(this, ServerHooks_Chat);
			TS.Hooks.GeneralHooks.ReloadEvent += GeneralHooks_ReloadEvent;

			TS.TShock.Log.ConsoleInfo("Mackerel NoJP Plugin is loaded.");

		}

		void GeneralHooks_ReloadEvent(TS.Hooks.ReloadEventArgs e) {
			config = Configuration.Load();
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
