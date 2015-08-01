using System;
using System.Diagnostics;
using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;
using System.Linq;

namespace MackerelPluginSet.Hypermarket {
//*
	[ApiVersion(1, 20)]
	public class Plugin : TerrariaPlugin {

		public Plugin(Main game)
			: base(game) {	
		}

		public override void Initialize() {

			TS.TShock.Log.ConsoleInfo("Mackerel Hypermarket Plugin is loaded.");

			TerrariaApi.Server.ServerApi.Hooks.NpcSpawn.Register(this, new HookHandler<NpcSpawnEventArgs>(onNpcSpawn));

			//TS.GetDataHandlers.TogglePvp.Register(new EventHandler<TS.GetDataHandlers.TogglePvpEventArgs>(h));
		}

		private void h(object sender, TS.GetDataHandlers.TogglePvpEventArgs e) {
			var p = TS.TShock.Players[e.PlayerId];

			for (int i = 0; i < 360; i++) {
				double rad = i * Math.PI / 180;
				float vx = 4 * (float)Math.Cos(rad);
				float vy = 4 * (float)Math.Sin(rad);
				Projectile.NewProjectile(p.X, p.Y, vx, vy, 110, 1, 9999, 0);
			}
		}

		private void onNpcSpawn(NpcSpawnEventArgs args) {
			var Npc = Main.npc[args.NpcId];
			Debug.WriteLine("{0} : {1}", Npc.type, Npc.name);
			if (Npc.type == 368 /* Traveling Merchant */) {
				Shuffle();

				NetMessage.SendTravelShop();

			}
		}

		private static void Shuffle() {
			for (int i = 0; i < Chest.maxItems; i++) {
				int id;
				do {
					id = Main.rand.Next(Terraria.ID.ItemID.PlatinumBow, Terraria.ID.ItemID.Count);
					var item = TS.TShock.Utils.GetItemById(id);
					if (TS.TShock.Itembans.ItemIsBanned(item.name)) continue;
				} while (Main.travelShop.Contains(id));
				Main.travelShop[i] = id;
			}
		}

		public override Version Version {
			get { return new Version("1.0.1"); }
		}
		public override string Name {
			get { return "Mackerel Hypermarket Plugin"; }
		}
		public override string Author {
			get { return "Totoki Kei"; }
		}
		public override string Description {
			get { return "Custom Travelling Merchant"; }
		}
	}
//*/
}
