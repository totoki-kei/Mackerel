using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;

namespace MackerelPluginSet.KakaG {

	[ApiVersion(1, 15)]
	public class Plugin : TerrariaPlugin {
		Configuration conf;

		public Plugin(Main game)
			: base(game) {

			conf = Configuration.Load();
		}



		public override void Initialize() {



			if (conf.GodMode.ObsidianSkinEnabled) {
				if (TS.GetDataHandlers.PlayerHP == null) {
					TS.GetDataHandlers.PlayerHP = new TS.HandlerList<TS.GetDataHandlers.PlayerHPEventArgs>();
				}
				var handler = new TS.HandlerList<TS.GetDataHandlers.PlayerHPEventArgs>.HandlerItem();
				handler.Handler = OnHp;
				handler.Priority = 0;

				TS.GetDataHandlers.PlayerHP.Register(handler);
			}

			if (conf.GodMode.PvPfragileEnabled) {
				if (TS.GetDataHandlers.PlayerDamage == null) {
					TS.GetDataHandlers.PlayerDamage = new TS.HandlerList<TS.GetDataHandlers.PlayerDamageEventArgs>();
				}
				var handler = new TS.HandlerList<TS.GetDataHandlers.PlayerDamageEventArgs>.HandlerItem();
				handler.Handler = OnDamage;
				handler.Priority = 0;

				TS.GetDataHandlers.PlayerDamage.Register(handler);
			}
			if (conf.HPRequirement.RequiredHP != 0) {
				ServerApi.Hooks.ServerJoin.Register(this, ServerHooks_Join);
			}

			TS.Log.ConsoleInfo("Mackerel KakaG Plugin is loaded.");

		}

		private void ServerHooks_Join(JoinEventArgs args) {
			var player = TS.TShock.Players[args.Who];
			if (player.FirstMaxHP < conf.HPRequirement.RequiredHP) {
				string msg = string.Format(conf.HPRequirement.KickMessageFormat, conf.HPRequirement.RequiredHP, player.FirstMaxHP);
				TS.TShock.Utils.Kick(player, msg, silent: true);
			}
		}

		private void OnDamage(object sender, TS.GetDataHandlers.PlayerDamageEventArgs e) {
			TS.TSPlayer player = TS.TShock.Players[e.ID];

			if (player.GodMode && e.PVP) {
				// Godmode中にPvPダメージ受けたら

				// 凍らせて
				player.Disable();

				// あいだをおいて ころす
				Task t = new Task(() => { Thread.Sleep(3000); player.DamagePlayer(9999); });
				t.Start();

			}
		}

		private void OnHp(object sender, TS.GetDataHandlers.PlayerHPEventArgs e) {
			// このハンドラが呼ばれるのは、GodModeの回復判定などが行われるときと同じタイミング。
			TS.TSPlayer player = TS.TShock.Players[e.PlayerId];

			if (player.GodMode) {
				// ここにGodModeの時の処理を書く
				player.SetBuff(1, bypass: true);
			}
		}

		public override Version Version {
			get { return new Version("1.2"); }
		}
		public override string Name {
			get { return "Mackerel KakaG Plugin"; }
		}
		public override string Author {
			get { return "Totoki Kei"; }
		}
		public override string Description {
			get { return "Kami, a.k.a God."; }
		}

	}
}
