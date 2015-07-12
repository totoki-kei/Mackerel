using System;

using TS = TShockAPI;
using Terraria;
using TerrariaApi.Server;

namespace MackerelPluginSet.BanTables {
#if true
	[ApiVersion(1, 19)]
	public class Plugin : TerrariaPlugin {
		public DBTable DBTable { get; private set; }
		public Plugin(Main game)
			: base(game) {
		}

		public override void Initialize() {

			TS.TShock.Log.ConsoleInfo("Mackerel BanTable Plugin is loaded.");
			DBTable = new DBTable(TS.TShock.DB);

			ServerApi.Hooks.ServerJoin.Register(this, ServerHooks_Join);

			Commands.Initialize(DBTable);
		}

		private void ServerHooks_Join(JoinEventArgs args) {
			var player = TS.TShock.Players[args.Who];
			var banList = DBTable.GetBanTables();
			BanEntry ban = null;

			// 結果に関係なく、とりあえずホスト名をログに表示する。
			string hostname = DnsResolver.GetHostByIP(player.IP) ?? "(null)";
			TS.TShock.Log.Info(string.Format(@"IP {0}'s hostname is ""{1}""", player.IP, hostname));

			foreach (var b in banList) {
				if (b.IsMatches(player)) {
					if (b.AllowJudge) {
						// allowed
						TS.TShock.Log.Info(string.Format("User {0} (IP = {1}, Hostname = {2}) is Allowed by entry #{3}.",
							player.Name, player.IP, hostname, b.Priority));
						return;
					}
					else {
						// denied
						ban = b;
						break;
					}
				}
			}

			if (ban != null) {
				args.Handled = true;
				if (string.IsNullOrEmpty(ban.Reason)) {
					player.Disconnect("You are banned.");
				}
				else {
					player.Disconnect(ban.Reason);
				}
				TS.TShock.Log.Info(string.Format("User {0} (IP = {1}, Hostname = {2}) is kicked by entry #{3}.", 
					player.Name, player.IP, hostname, ban.Priority));
			}
		}

		[Obsolete("Old TShock Version.")]
		void ServerHooks_Join(int playierID, System.ComponentModel.HandledEventArgs handler) {
			var player = TS.TShock.Players[playierID];
			var banList = DBTable.GetBanTables();
			var hostname = DnsResolver.GetHostByIP(player.IP);
			BanEntry ban = null;

			foreach (var b in banList) {
				if (b.IsMatches(player)) {
					if (b.AllowJudge) {
						// allowed
						return;
					}
					else {
						// denied
						ban = b;
						break;
					}
				}
			}

			if (ban != null) {
				handler.Handled = true;
				if (string.IsNullOrEmpty(ban.Reason)) {
					TS.TShock.Utils.Kick(player, "You are banned.", true);
				}
				else {
					TS.TShock.Utils.Kick(player, ban.Reason, true);
				}
			}
		}

		public override Version Version {
			get { return new Version("1.2.1"); }
		}
		public override string Name {
			get { return "Mackerel BanTable Plugin"; }
		}
		public override string Author {
			get { return "Totoki Kei"; }
		}
		public override string Description {
			get { return "Flexible Ban Mechanism."; }
		}
	}


#endif
}
