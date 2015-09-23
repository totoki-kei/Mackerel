using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
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
			TS.GetDataHandlers.PlayerDamage += OnDamage;
			TerrariaApi.Server.ServerApi.Hooks.NpcSetDefaultsString.Register(this, Server_NpcSetDefaultsString);
			TerrariaApi.Server.ServerApi.Hooks.NpcSetDefaultsInt.Register(this, Server_NpcSetDefaults);
			TerrariaApi.Server.ServerApi.Hooks.ProjectileSetDefaults.Register(this, Server_ProjectileSetDefaults);

			TS.TShock.Log.ConsoleInfo("Mackerel Carnival Plugin is loaded.");

		}

		private void Server_ProjectileSetDefaults(SetDefaultsEventArgs<Projectile, int> args) {
			//if (args.Info == Terraria.ID.ProjectileID.sho)
		}

		private void OnDamage(object sender, TS.GetDataHandlers.PlayerDamageEventArgs e) {
			var player = Terraria.Main.player[e.ID];

			var dmg = Terraria.Main.CalculatePlayerDamage( e.Damage, player.statDefense);
			
			while (dmg > 0) {
				int x = rand.Next(50) + 1;
				if (x < dmg) {

					int rnd = rand.Next(5);
					if (rnd <= 3) {
						int additionalDrop = nebulaIds[rand.Next(nebulaIds.Length)];
						Terraria.Item.NewItem((int)player.position.X, (int)player.position.Y, 3, 3, additionalDrop);
					}
				}
				dmg -= x;
			}
		}

		private void Server_NpcSetDefaultsString(SetDefaultsEventArgs<NPC, string> args) {
			ModifyNPC(args.Object);
		}

		private void Server_NpcSetDefaults(SetDefaultsEventArgs<NPC, int> args) {
			ModifyNPC(args.Object);
		}

		private static void ModifyNPC(NPC npc) {
			if (npc.realLife == -1) {
				npc.lifeMax *= 5;
				npc.lifeMax /= 2;
				npc.life *= 5;
				npc.life /= 2;
			}
			if(npc.lifeRegen == 0)
				npc.lifeRegen = 10;
			npc.value *= 10;
		}

		readonly int[] nebulaIds = {
				Terraria.ID.ItemID.NebulaPickup1,
				Terraria.ID.ItemID.NebulaPickup2,
				Terraria.ID.ItemID.NebulaPickup3,
			};

		private void Server_NpcStrike(NpcStrikeEventArgs args) {
			
		}

		private void Server_NpcLootDrop(NpcLootDropEventArgs args) {

			switch (args.ItemId) {
				case Terraria.ID.ItemID.PlatinumCoin:
				case Terraria.ID.ItemID.GoldCoin:
				case Terraria.ID.ItemID.SilverCoin:
				case Terraria.ID.ItemID.CopperCoin:

					int additionalDrop = nebulaIds[rand.Next(nebulaIds.Length)];
					Terraria.Item.NewItem((int)args.Position.X, (int)args.Position.Y, args.Width, args.Height, additionalDrop);

					if (rand.Next(50) == 0) {
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

				case Terraria.ID.ItemID.LunarOre:
				case Terraria.ID.ItemID.FragmentNebula:
				case Terraria.ID.ItemID.FragmentSolar:
				case Terraria.ID.ItemID.FragmentVortex:
				case Terraria.ID.ItemID.FragmentStardust:
					args.Stack *= 3;
					if (args.Stack > 999) args.Stack = 999;
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
