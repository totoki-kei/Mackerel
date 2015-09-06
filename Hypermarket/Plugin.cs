using System;
using System.Diagnostics;
using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;
using System.Linq;

using Common = MackerelPluginSet.Common;

namespace MackerelPluginSet.Hypermarket {
//*
	[ApiVersion(1, 21)]
	public class Plugin : TerrariaPlugin {

		public Plugin(Main game)
			: base(game) {	
		}

		public override void Initialize() {

			TS.TShock.Log.ConsoleInfo("Mackerel Hypermarket Plugin is loaded.");

			TerrariaApi.Server.ServerApi.Hooks.GameUpdate.Register(this, new HookHandler<EventArgs>(onGameUpdate));

		}

		double nextShuffleTime = 0.0;

		private readonly Tuple<bool, double> night2000 = Common.Utility.ConvertTimeInHourToGameTime(20);

		private void onGameUpdate(EventArgs args) {
			//if (!NPC.travelNPC) {
			//	npcIndex = -1;
			//	nextShuffleTime = 0;
			//	return;
			//}


			if (Main.dayTime) {
				if (Main.time >= nextShuffleTime) {
					// ゲーム内時間1800秒(30分)、実時間30秒ごとにシャッフル
					nextShuffleTime = Math.Floor(Main.time / 1800) * 1800 + 1800;
					Shuffle();
					NetMessage.SendTravelShop();
				}
			}
			else {
				int npcIndex = NPC.FindFirstNPC(Terraria.ID.NPCID.TravellingMerchant);
				if (npcIndex != -1) {
					TS.TSPlayer.Server.StrikeNPC(npcIndex, 99999, 0, 0);
				}
			}
		}

		private static void Shuffle() {
			int itemNum = 5 + Main.rand.Next(0, Terraria.Chest.maxItems - 5);
			for (int i = 0; i < Terraria.Chest.maxItems; i++) {
				int id = 0;
				if (i < itemNum) {
					while(true) {
						// Phasesaber系6種類はマイナスのインデックスが割り当てられている
						id = Main.rand.Next(-6, Terraria.ID.ItemID.Count);
						if (id < 0) id -= 18;

						var item = TS.TShock.Utils.GetItemById(id);
						if (item.type == 0) continue;
						if (item.value == 0) continue; // 無料のものは並べない
						if (TS.TShock.Itembans.ItemIsBanned(item.name)) continue;
						if (Main.travelShop.Contains(id)) continue;

						break;
					}
				}
				Main.travelShop[i] = id;
			}
		}

		public override Version Version {
			get { return new Version("1.1.0"); }
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
