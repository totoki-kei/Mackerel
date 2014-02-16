using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;

namespace MackerelPluginSet.KagaG {

	[ApiVersion(1, 15)]
	public class Plugin : TerrariaPlugin {

		public Plugin(Main game)
			: base(game) {	
		}

		public override void Initialize() {

			var handler = new TS.HandlerList<TS.GetDataHandlers.PlayerHPEventArgs>.HandlerItem();
			handler.Handler = OnHp;
			handler.Priority = 0;

			if (TS.GetDataHandlers.PlayerHP == null) {
				TS.GetDataHandlers.PlayerHP = new TS.HandlerList<TS.GetDataHandlers.PlayerHPEventArgs>();
			}
			TS.GetDataHandlers.PlayerHP.Register(handler);

			TS.Log.ConsoleInfo("Mackerel KakaG Plugin is loaded.");

		}

		private void OnHp(object sender, TS.GetDataHandlers.PlayerHPEventArgs e) {
			// このハンドラが呼ばれるのは、GodModeの回復判定などが行われるときと同じタイミング。
			TS.TSPlayer player = TS.TShock.Players[e.PlayerId];

			if (player.GodMode) {
				// ここにGodModeの時の処理を書く
				player.SetBuff(1, bypass:true);
			}
		}

		public override Version Version {
			get { return new Version("1.0.1"); }
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
