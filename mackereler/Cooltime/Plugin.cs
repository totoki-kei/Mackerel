using System;
using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;

namespace MackerelPluginSet.Cooltime {
	//*
		[ApiVersion(1, 21)]
		public class Plugin : TerrariaPlugin {

			public Plugin(Main game) : base(game) {

				// TShockに優先してフックする
				this.Order = -1;
			}

			public override void Initialize() {
				TS.TShock.Log.ConsoleInfo("Mackerel Cooltime Plugin is loaded.");

				ServerApi.Hooks.NetGetData.Register(this, OnGetData);
			}

			protected override void Dispose(bool disposing) {
				if (disposing) ServerApi.Hooks.NetGetData.Deregister(this, OnGetData);
				base.Dispose(disposing);
			}

			private void OnGetData(GetDataEventArgs e) {
				// バイナリ解析部分はTShockソースから拝借
				if (e.Handled)
					return;

				PacketTypes type = e.MsgID;
				var player = TS.TShock.Players[e.Msg.whoAmI];

				if (player == null || !player.ConnectionAlive) {
					return;
				}
				if (player.RequiresPassword && type != PacketTypes.PasswordSend) {
					return;
				}
				if ((player.State < 10 || player.Dead) 
					&& (int)type > 12 
					&& (int)type != 16 
					&& (int)type != 42 
					&& (int)type != 50 
					&& (int)type != 38 
					&& (int)type != 21
					&& (int)type != 22) {
					return;
				}

				if (type == PacketTypes.SpawnBossorInvasion) {
					var spawnboss = false;
					var invasion = false;
					var plr = e.Data.ReadInt16();
					var Type = e.Data.ReadInt16();
					NPC npc = new NPC();
					npc.SetDefaults(Type);
					spawnboss = npc.boss;

				}
			}

			public override Version Version {
				get { return new Version("1.0"); }
			}
			public override string Name {
				get { return "Mackerel Plugin"; }
			}
			public override string Author {
				get { return "Totoki Kei"; }
			}
			public override string Description {
				get { return "Apply summon cooltime"; }
			}
		}
	//*/
}
