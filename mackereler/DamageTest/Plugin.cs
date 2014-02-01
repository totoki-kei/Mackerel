using System;

using TS = TShockAPI;
using Terraria;
using System.Collections.Generic;

namespace MackerelPluginSet.DamageTest {
#if false
	[APIVersion(1, 12)]
	public class Plugin : TerrariaPlugin {

		public Plugin(Main game)
			: base(game) {	
		}

		public override void Initialize() {

			{
				var hnd = new TS.HandlerList<TS.GetDataHandlers.PlayerDamageEventArgs>.HandlerItem();
				hnd.Handler = OnKillMe;
				hnd.Priority = TS.HandlerPriority.High;
				if (TS.GetDataHandlers.KillMe == null) {
					TS.GetDataHandlers.KillMe = new TS.HandlerList<TS.GetDataHandlers.KillMeEventArgs>();
				}
				TS.GetDataHandlers.PlayerDamage.Register(hnd);
			}

			TS.Log.ConsoleInfo("Mackerel KillScore Plugin is loaded.");

		}

		private void OnKillMe(object sender, TS.GetDataHandlers.PlayerDamageEventArgs e) {
			//TShockAPI.Utils.Instance.SendLogs("you got " + e.Damage.ToString() + " damage!", Color.AliceBlue);
			//var p = new TS.TSPlayer(e.ID);
			
			// TODO: Projectileを発生させる方法を確認する
		}

		public override Version Version {
			get { return new Version("1.0"); }
		}
		public override string Name {
			get { return "Mackerel KillScore Plugin"; }
		}
		public override string Author {
			get { return "Totoki Kei"; }
		}
		public override string Description {
			get { return "Plugin suite"; }
		}
	}
#endif
}
