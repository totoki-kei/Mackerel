using System;
using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;

namespace MackerelPluginSet {
//*
	[ApiVersion(1, 20)]
	public class Plugin : TerrariaPlugin {

		public Plugin(Main game)
			: base(game) {	
		}

		public override void Initialize() {

			TS.TShock.Log.ConsoleInfo("Mackerel Misc Plugin is loaded.");

			TS.GetDataHandlers.NewProjectile.Register(handler); ;
			TerrariaApi.Server.ServerApi.Hooks.ProjectileSetDefaults.Register(this, new HookHandler<SetDefaultsEventArgs<Projectile, int>>(hook));

		}

		private void handler(object sender, TS.GetDataHandlers.NewProjectileEventArgs e) {
			var ply = TS.TShock.Players[e.Owner];
			if (e.Type == 242) {
//				ply.RemoveProjectile(e.Identity, e.Owner);
				Projectile.NewProjectile(e.Position.X, e.Position.Y, e.Velocity.X/7, e.Velocity.Y/7, e.Type,e.Damage, e.Knockback);
			}
		}

		private void hook(SetDefaultsEventArgs<Projectile, int> args) {
			if (args.Info == 242) {
//				args.Object.velocity = args.Object.velocity.Multiply(1 / 7f);
			}
		}


		public override Version Version {
			get { return new Version("1.0.1"); }
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

	}
//*/
}
