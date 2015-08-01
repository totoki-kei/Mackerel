using System;

using TS = TShockAPI;
using Terraria;
using System.Collections.Generic;

namespace MackerelPluginSet {
#if false
	[APIVersion(1, 12)]
	public class Plugin : TerrariaPlugin {

		public Plugin(Main game)
			: base(game) {	
		}

		public override void Initialize() {

			TS.TShock.Log.ConsoleInfo("Mackerel Cooltime Plugin is loaded.");

		}

		public override Version Version {
			get { return new Version("1.0"); }
		}
		public override string Name {
			get { return "Mackerel Cooltime Plugin"; }
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
