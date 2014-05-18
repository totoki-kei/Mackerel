using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;

using System.Linq;

namespace MackerelPluginSet.SafetyRocket {

	[ApiVersion(1, 16)]
	public class Plugin : TerrariaPlugin {

		public Plugin(Main game)
			: base(game) {
		}

		public override void Initialize() {
			SetHandler(ref TS.GetDataHandlers.NewProjectile, OnNewProjectile);
			TS.Log.ConsoleInfo("Mackerel SafetyRocket Plugin is loaded.");
		}


		private void OnNewProjectile(object sender, TS.GetDataHandlers.NewProjectileEventArgs e) {

			var ply = TS.TShock.Players[e.Owner];

			if (ply.Group.HasPermission(TS.Permissions.usebanneditem)) return;
			if (string.Compare(ply.Group.Name, "superadmin", true) == 0) return;

			if (TS.TShock.Itembans.ItemIsBanned("Rocket II")) {
				if (new[] { 136, 137, 138, 339 }.Contains(e.Type)) {
					ply.RemoveProjectile(e.Identity, e.Owner);
					ply.SendErrorMessage("You do not have permission to perform this action.");
					ply.Disable("Using Rocket II without permissions");
					e.Handled = true;
				}
			}
			if (TS.TShock.Itembans.ItemIsBanned("Rocket IV")) {
				if (new[] { 142, 143, 144, 341 }.Contains(e.Type)) {
					ply.RemoveProjectile(e.Identity, e.Owner);
					ply.SendErrorMessage("You do not have permission to perform this action.");
					ply.Disable("Using Rocket IV without permissions");
					e.Handled = true;
				}
			}
		}

		private void SetHandler<T>(ref TS.HandlerList<T> handlerList, EventHandler<T> handler) where T : EventArgs {
			if (handlerList == null)
				handlerList = new TS.HandlerList<T>();
			var h = new TS.HandlerList<T>.HandlerItem() {
				Handler = handler,
				Priority = TS.HandlerPriority.Highest,
			};
			handlerList.Register(h);
		}

		public override Version Version {
			get { return new Version("1.0.2"); }
		}
		public override string Name {
			get { return "Mackerel SafetyRocket Plugin"; }
		}
		public override string Author {
			get { return "Totoki Kei"; }
		}
		public override string Description {
			get { return "Bans Rocket II / IV"; }
		}
	}

}
