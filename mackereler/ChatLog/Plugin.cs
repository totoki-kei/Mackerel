using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TS = TShockAPI;
using Hooks = TShockAPI.Hooks;
using Terraria;
using TerrariaApi.Server;

namespace MackerelPluginSet.ChatLog {
	[ApiVersion(1, 21)]
	public class Plugin : TerrariaPlugin {
		Settings setting;
		LogFile log;
		Formatter format;

		public Plugin(Main game)
			: base(game) {
				setting = Settings.Instance;

				if (setting.Enabled) {
					log = new LogFile();
				}

				format = new Formatter();
			
		}

		public override void Initialize() {

			if (setting.Enabled) {
				ServerApi.Hooks.ServerChat.Register(this, ServerHooks_Chat);
				ServerApi.Hooks.ServerJoin.Register(this, ServerHooks_Join);
				ServerApi.Hooks.ServerLeave.Register(this, ServerHooks_Leave);

				TS.TShock.Log.ConsoleInfo(Name + " is enabled.");
			}
			else {
				TS.TShock.Log.ConsoleInfo(Name + " is disabled by setting.");
			}
		}

		private void ServerHooks_Leave(LeaveEventArgs args) {
			TS.TSPlayer pl = new TS.TSPlayer(args.Who);
			log.Append(format.FormatLeave(pl));
		}

		private void ServerHooks_Join(JoinEventArgs args) {
			TS.TSPlayer pl = new TS.TSPlayer(args.Who);

			log.Append(format.FormatJoin(pl));
		}

		private void ServerHooks_Chat(ServerChatEventArgs args) {
			TS.TSPlayer pl = new TS.TSPlayer(args.Buffer.whoAmI);

			log.Append(format.FormatChat(pl, args.Buffer, args.Who, args.Text));
			//arg4.Handled = true;
		}

		[Obsolete("Old TShock Version.")]
		void ServerHooks_Chat(MessageBuffer arg1, int arg2, string arg3, System.ComponentModel.HandledEventArgs arg4) {
			TS.TSPlayer pl = new TS.TSPlayer(arg1.whoAmI);

			log.Append(format.FormatChat(pl, arg1, arg2, arg3));
			//arg4.Handled = true;
		}

		[Obsolete("Old TShock Version.")]
		void ServerHooks_Join(int arg1, System.ComponentModel.HandledEventArgs arg2) {
			TS.TSPlayer pl = new TS.TSPlayer(arg1);

			log.Append(format.FormatJoin(pl));
		}

		[Obsolete("Old TShock Version.")]
		void ServerHooks_Leave(int obj) {
			TS.TSPlayer pl = new TS.TSPlayer(obj);

			log.Append(format.FormatLeave(pl));
		}


		public override Version Version {
			get { return new Version("1.2.1"); }
		}
		public override string Name {
			get { return "Mackerel ChatLog Plugin"; }
		}
		public override string Author {
			get { return "Totoki Kei"; }
		}
		public override string Description {
			get { return "Log chats to file."; }
		}
	}
}
