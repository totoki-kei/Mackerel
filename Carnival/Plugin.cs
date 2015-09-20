using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaApi.Server;
using TS = TShockAPI;

namespace MackerelPluginSet.Carnival {
	[ApiVersion(1, 21)]
	public class Plugin : TerrariaPlugin {

		public Plugin(Terraria.Main game)
			: base(game) {
		}

		Random rand = new Random();

		public override void Initialize() {
			TerrariaApi.Server.ServerApi.Hooks.NpcLootDrop.Register(this, Server_NpcLootDrop);
			TerrariaApi.Server.ServerApi.Hooks.NpcStrike.Register(this, Server_NpcStrike);

			TS.TShock.Log.ConsoleInfo("Mackerel Carnival Plugin is loaded.");

		}

		readonly int[] nebulaIds = {
				Terraria.ID.ItemID.NebulaPickup1,
				Terraria.ID.ItemID.NebulaPickup2,
				Terraria.ID.ItemID.NebulaPickup3,
			};

		private void Server_NpcStrike(NpcStrikeEventArgs args) {
			int rnd = rand.Next(10);
			if (rnd <= 3) {
				int additionalDrop = nebulaIds[rand.Next(nebulaIds.Length)];
				Terraria.Item.NewItem((int)args.Npc.position.X, (int)args.Npc.position.Y, 3, 3, additionalDrop);
			}
		}

		private void Server_NpcLootDrop(NpcLootDropEventArgs args) {

			switch (args.ItemId) {
				case Terraria.ID.ItemID.PlatinumCoin:
				case Terraria.ID.ItemID.GoldCoin:
				case Terraria.ID.ItemID.SilverCoin:
				case Terraria.ID.ItemID.CopperCoin:
					int additionalDrop = nebulaIds[rand.Next(nebulaIds.Length)];
					Terraria.Item.NewItem((int)args.Position.X, (int)args.Position.Y, args.Width, args.Height, additionalDrop);

					if (rand.Next(20) == 0) {
						int num3 = Terraria.Item.NPCtoBanner(Terraria.Main.npc[args.NpcArrayIndex].BannerID());
						if (num3 <= 0) break;

						int num6 = 1615 + num3 - 1;
						if (num3 >= 249) {
							num6 = 3593 + num3 - 249;
						}
						else if (num3 >= 186) {
							num6 = 3390 + num3 - 186;
						}
						else if (num3 >= 88) {
							num6 = 2897 + num3 - 88;
						}
						Terraria.Item.NewItem((int)args.Position.X, (int)args.Position.Y, args.Width, args.Height, num6);
					}
					break;
				case Terraria.ID.ItemID.Heart:
					Terraria.Item.NewItem((int)args.Position.X, (int)args.Position.Y, args.Width, args.Height, args.ItemId);
					Terraria.Item.NewItem((int)args.Position.X, (int)args.Position.Y, args.Width, args.Height, args.ItemId);
					Terraria.Item.NewItem((int)args.Position.X, (int)args.Position.Y, args.Width, args.Height, args.ItemId);
					Terraria.Item.NewItem((int)args.Position.X, (int)args.Position.Y, args.Width, args.Height, args.ItemId);
					break;

			}
		}
		

		public override Version Version {
			get { return new Version("1.0"); }
		}
		public override string Name {
			get { return "Mackerel Carnival"; }
		}
		public override string Author {
			get { return "Totoki kei"; }
		}
		public override string Description {
			get { return "4th Anniversary"; }
		}
	}
}
