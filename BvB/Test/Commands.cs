//#define TESTCOMMANDS

using MackerelPluginSet.BvB.Fields;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;


namespace MackerelPluginSet.BvB.Test {
	

	class Commands {
#if TESTCOMMANDS
		public static void InitializeCommands() {
			{
				var cmd = new TS.Command(OnTestCmd, "bvbtest") {
					AllowServer = true,
					DoLog = true,
					Permissions = { }
				};
				TS.Commands.ChatCommands.Add(cmd);
			}
			{
				var cmd = new TS.Command(OnTestFill, "bvbfill") {
					AllowServer = true,
					DoLog = true,
					Permissions = { }
				};
				TS.Commands.ChatCommands.Add(cmd);
			}
					{
				var cmd = new TS.Command(OnTestFillWorldEdit, "bvbwe") {
					AllowServer = true,
					DoLog = true,
					Permissions = { }
				};
				TS.Commands.ChatCommands.Add(cmd);
			}
}
#else
		public static void InitializeCommands() { /* nop */ }
#endif

		private static void OnTestFillWorldEdit(TS.CommandArgs args) {
			try {
				if (args.Parameters.Count != 1) {
					args.Player.SendErrorMessage("argument (filename) is needed.");
					return;
				}

				string filename = args.Parameters[0];

				WorldEditField f = new WorldEditField();
				f.Load(filename);

				f.Apply((int)(args.Player.X / 16), (int)(args.Player.Y / 16));

			}
			catch (Exception e) {
				Debug.WriteLine(e.StackTrace);
			}
		}

		private static void OnTestFill(TS.CommandArgs args) {
			try {
				if (args.Parameters.Count != 1) {
					args.Player.SendErrorMessage("argument (filename) is needed.");
					return;
				}

				string filename = args.Parameters[0];

				PatternField f = PatternField.LoadOrGet(filename);

				f.Apply((int)(args.Player.X / 16), (int)(args.Player.Y / 16));

			}
			catch (Exception e) {
				Debug.WriteLine(e.StackTrace);
			}
		}

		private static void OnTestCmd(TS.CommandArgs args) {

			byte tileType;
			if (args.Parameters.Count != 1 || byte.TryParse(args.Parameters[0], out tileType)) {
				tileType = 46; // silver brick;
			}

			var cx = Terraria.Main.maxTilesX / 2;
			var cy = Terraria.Main.maxTilesY / 2;
			var size = 30;

			for (int x = cx - size; x <= cx + size; x++) {
				for (int y = cy - size; y <= cy + size; y++) {
					Terraria.Tile tile = Terraria.Main.tile[x, y] ?? (Terraria.Main.tile[x, y] = new Terraria.Tile());
					tile.type = tileType;
					tile.bTileHeader |= 0x01;
					tile.wire(x == cx - size || y == cy - size || x == cx + size || y == cy + size);
	//				Terraria.Main.tile[x, y] = new Tile();
				}
			}
			{
				var t1 = Terraria.Main.tile[cx - 1, cy];
				var t2 = Terraria.Main.tile[cx, cy];
				var t3 = Terraria.Main.tile[cx + 1, cy];

				t1.type = 235;
				t1.frameX = 0; t1.frameY = 0;

				t2.type = 235;
				t2.frameX = 18; t2.frameY = 0;
				
				t3.type = 235;
				t3.frameX = 36; t3.frameY = 0;
			}

			Plugin.ResetSection(cx - size, cy - size, cx + size, cy + size);


			
			{
				int index = Terraria.Item.NewItem((int)cx, (int)cy, 1, 1, 1);
				var item = Terraria.Main.item[index];

				item.SetDefaults(1);
				item.position = new Vector2(cx * 16, cy * 16);

				NetMessage.SendData((int)PacketTypes.ItemDrop, -1, -1, "", 1, 0f, 0f, 0f);

			}

			var p = TShockAPI.TShock.Players[0];
			TShockAPI.PlayerData pd = new TS.PlayerData(p);
			pd.CopyCharacter(p);
			pd.health = 20;
			p.PlayerData = pd;
			p.SendServerCharacter();
			
			


		}
	}
}
