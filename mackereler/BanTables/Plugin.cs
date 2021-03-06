﻿using System;

using TS = TShockAPI;
using Terraria;
using TerrariaApi.Server;

namespace MackerelPluginSet.BanTables {
#if false
	[ApiVersion(1, 12)]
	public class Plugin : TerrariaPlugin {
		public DBTable DBTable { get; private set; }
		public Plugin(Main game)
			: base(game) {
		}

		public override void Initialize() {

			TS.Log.ConsoleInfo("Mackerel BanTable Plugin is loaded.");
			DBTable = new DBTable(TS.TShock.DB);

			TShockAPI.Hooks.PlayerHooks.PlayerPreLogin += PlayerHooks_PlayerPreLogin;

			Commands.Initialize(DBTable);
		}

		void PlayerHooks_PlayerPreLogin(TS.Hooks.PlayerPreLoginEventArgs e) {
			var player = e.Player;
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
				e.Handled = true;
				if (string.IsNullOrEmpty(ban.Reason)) {
					TS.TShock.Utils.Kick(player, "You are banned.", true);
				}
				else {
					TS.TShock.Utils.Kick(player, ban.Reason, true);
				}
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
			get { return new Version("1.1"); }
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
