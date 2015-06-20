//#define TESTCOMMANDS

using MackerelPluginSet.RegionImport.Fields;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;


namespace MackerelPluginSet.RegionImport {
	

	public class Commands {

		public static void InitializeCommands() {
			{
				var cmd = new TS.Command(OnImport, "import") {
					AllowServer = true,
					DoLog = true,
					Permissions = { }
				};
				TS.Commands.ChatCommands.Add(cmd);
			}
			//{
			//	var cmd = new TS.Command(OnTestFillWorldEdit, "bvbwe") {
			//		AllowServer = true,
			//		DoLog = true,
			//		Permissions = { }
			//	};
			//	TS.Commands.ChatCommands.Add(cmd);
			//}
		}

		private static void OnImport(TS.CommandArgs args) {
			try {
				if (args.Parameters.Count != 1) {
					args.Player.SendErrorMessage("argument (filename) is required !");
					return;
				}

				string filename = args.Parameters[0];

				if (filename.EndsWith(".field", StringComparison.CurrentCultureIgnoreCase)) {
					PatternField f = PatternField.LoadOrGet(filename);

					f.Apply((int)(args.Player.X / 16), (int)(args.Player.Y / 16));
				}
				else /* if(filename.EndsWith(".dat", StringComparison.CurrentCultureIgnoreCase)) */ {
					WorldEditField f = new WorldEditField();
					f.Load(filename);

					f.Apply((int)(args.Player.X / 16), (int)(args.Player.Y / 16));

				}
			}
			catch (Exception e) {
				TS.Log.Error(e.ToString());
				args.Player.SendErrorMessage("Caught {0}, please see logs for detail.", e.GetType().Name);
			}
		}
	}
}
