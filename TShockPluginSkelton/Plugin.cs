using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaApi.Server;
using TS = TShockAPI;

namespace TShockPluginSkelton {
	[ApiVersion(1, 21)]
	public class Plugin : TerrariaPlugin {

		public Plugin(Terraria.Main game)
			: base(game) {
		}

		public override void Initialize() {

			// TODO: Input your plugin initialization code here.
			TS.TShock.Log.ConsoleInfo("Plugin is loaded.");

		}

		public override Version Version {
			get { return new Version("1.0"); }
		}
		public override string Name {
			get { return ""; }
		}
		public override string Author {
			get { return ""; }
		}
		public override string Description {
			get { return ""; }
		}
	}
}
