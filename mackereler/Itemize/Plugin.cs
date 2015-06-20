using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TS = TShockAPI;
using Terraria;
using TerrariaApi.Server;


namespace MackerelPluginSet.Itemize {

	/// <summary>
	/// ダメージを受けたときにアイテムを発生させる
	/// </summary>
	[ApiVersion(1, 17)]
	class Plugin : TerrariaPlugin {

		public Plugin(Main game)
			: base(game) {
		}

		public override void Initialize() {
			// コマンドの登録
			TS.Command cmd = new TS.Command(TS.Permissions.godmode, OnCommand, "itemize", "[on/off/toggle]");


			TS.Commands.ChatCommands.Add(cmd);
			TS.GetDataHandlers.PlayerDamage += OnDamage;
			
		}

		static void OnCommand(TS.CommandArgs args) {
			string mode = args.Parameters.Count == 0 ? "toggle" : args.Parameters[0].ToLower();
			if (mode == "toggle") mode = (ItemEnabled ? "off" : "on");

			switch (mode) {
				case "help":
					args.Player.SendMessage("Usage: /itemize [on/off/toggle/help]", Color.Yellow);
					break;
				case "off":
					ItemEnabled = false;
					args.Player.SendMessage("Itemize mode is disabled.", Color.Green);
					break;
				case "on":
					ItemEnabled = true;
					args.Player.SendMessage("Itemize mode is enabled.", Color.Green);
					break;
				case "all_prefix":
					var target = TS.Utils.Instance.GetItemByIdOrName(args.Parameters[1]);
					if(target.Count == 0) break;
					for (int i = 0; i < 82; i++) {
						args.Player.GiveItem(target[0].netID, "???", 0, 0, 1, i);
					}
					break;
				default:
					break;
			}
		}

		static void OnDamage(object o, TS.GetDataHandlers.PlayerDamageEventArgs e) {
			if(ItemEnabled) 
				TS.TShock.Players[e.ID].GiveItem(TS.Utils.Instance.Random.Next(602) + 1, "???", 0, 0, 1, TS.Utils.Instance.Random.Next(82) + 1);
		}

		public static bool ItemEnabled {
			get;
			private set;
		}

		public override Version Version {
			get { return new Version("1.1"); }
		}
		public override string Name {
			get { return "Mackerel Itemize Plugin"; }
		}
		public override string Author {
			get { return "Totoki Kei"; }
		}
		public override string Description {
			get { return "Generate Item when player is damaged."; }
		}
	}
}
