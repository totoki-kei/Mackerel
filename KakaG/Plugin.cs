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

	[ApiVersion(1, 20)]
	public class Plugin : TerrariaPlugin {
		Configuration conf;

		public Plugin(Main game)
			: base(game) {

			conf = Configuration.Load();
		}



		public override void Initialize() {
			if (conf.GodMode.ObsidianSkinEnabled) {
				var handler = new TS.HandlerList<TS.GetDataHandlers.PlayerHPEventArgs>.HandlerItem();
				handler.Handler = OnHp_GodMode;
				handler.Priority = 0;

				TS.GetDataHandlers.PlayerHP += handler;
			}

			if (conf.GodMode.PvPfragileEnabled) {
				var handler = new TS.HandlerList<TS.GetDataHandlers.PlayerDamageEventArgs>.HandlerItem();
				handler.Handler = OnDamage;
				handler.Priority = 0;

				TS.GetDataHandlers.PlayerDamage += handler;
			}
			if (conf.HPRequirement.RequiredHP != 0) {
				TS.GetDataHandlers.PlayerHP += new EventHandler<TS.GetDataHandlers.PlayerHPEventArgs>(OnHP_RequiredHP);
				ServerApi.Hooks.ServerJoin.Register(this, ServerHooks_Join);
			}
			if (conf.PotionSickness.Enabled) {
				var handler = new TS.HandlerList<TS.GetDataHandlers.PlayerUpdateEventArgs>.HandlerItem();
				handler.Handler = OnPlayerUpdate;
				handler.Priority = 0;

				TS.GetDataHandlers.PlayerUpdate += handler;
			}

			TS.TShock.Log.ConsoleInfo("Mackerel KakaG Plugin is loaded.");

		}

		int[] hpcache;

		private void ServerHooks_Join(JoinEventArgs args) {
			if (hpcache == null) return;

			var player = TS.TShock.Players[args.Who];
			if (hpcache[args.Who] < conf.HPRequirement.RequiredHP) {
				string msg = string.Format(conf.HPRequirement.KickMessageFormat, conf.HPRequirement.RequiredHP, hpcache[args.Who]);
				TS.TShock.Utils.Kick(player, msg, silent: true);
			}
		}

		private void OnHP_RequiredHP(object sender, TS.GetDataHandlers.PlayerHPEventArgs e) {
			if (hpcache == null) hpcache = new int[TS.TShock.Players.Length];

			hpcache[e.PlayerId] = e.Max;
		}

		private void OnPlayerUpdate(object sender, TS.GetDataHandlers.PlayerUpdateEventArgs e) {
			var player = TS.TShock.Players[e.PlayerId];

			int buf21 = player.TPlayer.HasBuff(21);
			if (buf21 >= 0) player.TPlayer.DelBuff(buf21);

			int buf57 = player.TPlayer.HasBuff(57);
			if (buf57 >= 0) player.TPlayer.DelBuff(buf57);

			NetMessage.SendData((int)PacketTypes.PlayerBuff, -1, player.Index, "", player.Index);

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

		private void OnHp_GodMode(object sender, TS.GetDataHandlers.PlayerHPEventArgs e) {
			// このハンドラが呼ばれるのは、GodModeの回復判定などが行われるときと同じタイミング。
			TS.TSPlayer player = TS.TShock.Players[e.PlayerId];

			if (player.GodMode) {
				// ここにGodModeの時の処理を書く
				player.SetBuff(1, bypass: true);
			}
		}

		public override Version Version {
			get { return new Version("1.2.2"); }
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
