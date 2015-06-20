using System;
using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;

namespace MackerelPluginSet.RegionImport {
//*
	[ApiVersion(1, 16)]
	public class Plugin : TerrariaPlugin {

		public Plugin(Main game)
			: base(game) {	
		}


		public override void Initialize() {

			TS.Log.ConsoleInfo("Mackerel RegionImport Plugin is loaded.");

			Commands.InitializeCommands();
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
			get { return "Plugin suite"; }
		}


		public static void ResetSection(int x, int y, int x2, int y2) {
			int left = Netplay.GetSectionX(x);
			int right = Netplay.GetSectionX(x2);
			int top = Netplay.GetSectionY(y);
			int bottom = Netplay.GetSectionY(y2);
			foreach (ServerSock sock in Netplay.serverSock) {
				if (sock.active) {
					int lowX = Math.Max(left,  0);
					int highX = Math.Min(right, sock.tileSection.GetLength(0));
					int lowY = Math.Max(top, 0);
					int highY = Math.Min(bottom, sock.tileSection.GetLength(1));

					for (int i = lowX; i <= highX; i++) {
						for (int j = lowY; j <= highY; j++)
							sock.tileSection[i, j] = false;
					}
				}
			}
		}
	}
//*/
}
