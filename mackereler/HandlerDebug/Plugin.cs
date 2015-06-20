using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;

using System.Linq;

namespace MackerelPluginSet.HandlerDebug {
#if DEBUG
	[ApiVersion(1, 17)]
	public class Plugin : TerrariaPlugin {

		bool enabled = false;

		public Plugin(Main game)
			: base(game) {	
		}

		public override void Initialize() {
			
			SetHandler(ref TS.GetDataHandlers.ChestItemChange);
			SetHandler(ref TS.GetDataHandlers.ChestOpen);
			SetHandler(ref TS.GetDataHandlers.ItemDrop);
			SetHandler(ref TS.GetDataHandlers.KillMe);
			SetHandler(ref TS.GetDataHandlers.LiquidSet);
			SetHandler(ref TS.GetDataHandlers.NewProjectile);
			SetHandler(ref TS.GetDataHandlers.NPCHome);
			SetHandler(ref TS.GetDataHandlers.NPCSpecial);
			SetHandler(ref TS.GetDataHandlers.NPCStrike);
			SetHandler(ref TS.GetDataHandlers.PlayerAnimation);
			SetHandler(ref TS.GetDataHandlers.PlayerBuff);
			SetHandler(ref TS.GetDataHandlers.PlayerBuffUpdate);
			SetHandler(ref TS.GetDataHandlers.PlayerDamage);
			SetHandler(ref TS.GetDataHandlers.PlayerHP);
			SetHandler(ref TS.GetDataHandlers.PlayerInfo);
			SetHandler(ref TS.GetDataHandlers.PlayerMana);
			SetHandler(ref TS.GetDataHandlers.PlayerSlot);
			SetHandler(ref TS.GetDataHandlers.PlayerSpawn);
			SetHandler(ref TS.GetDataHandlers.PlayerTeam);
			SetHandler(ref TS.GetDataHandlers.PlayerUpdate);
			SetHandler(ref TS.GetDataHandlers.SendTileSquare);
			SetHandler(ref TS.GetDataHandlers.Sign);
			SetHandler(ref TS.GetDataHandlers.TileEdit);
			SetHandler(ref TS.GetDataHandlers.TileKill);
			SetHandler(ref TS.GetDataHandlers.TogglePvp);

			// リフレクションを使ってこれを簡略化したいが
			// 思っていた以上に複雑になりそうなので一旦保留する

			//var fields = typeof(TS.GetDataHandlers).GetFields(System.Reflection.BindingFlags.Static);
			//foreach (var field in fields) {
			//	if (field.FieldType == typeof(TS.HandlerList<>)) {					
			//		// リフレクションで取ったフィールドをメソッドのref引数として渡すには・・・？
			//		SetHandler(...);
			//	}
			//}


			TS.Hooks.GeneralHooks.ReloadEvent += e => Callback<TS.Hooks.ReloadEventArgs>(e, "GeneralHooks.ReloadEvent");
			TS.Hooks.PlayerHooks.PlayerChat += e => Callback<TS.Hooks.PlayerChatEventArgs>(e, "PlayerHooks.PlayerChat");
			TS.Hooks.PlayerHooks.PlayerCommand += e => Callback<TS.Hooks.PlayerCommandEventArgs>(e, "PlayerHooks.PlayerCommand");
			TS.Hooks.PlayerHooks.PlayerPostLogin += e => Callback<TS.Hooks.PlayerPostLoginEventArgs>(e, "PlayerHooks.PlayerPostLogin");
			TS.Hooks.PlayerHooks.PlayerPreLogin += e => Callback<TS.Hooks.PlayerPreLoginEventArgs>(e, "PlayerHooks.PlayerPreLogin");


			SetCallback(TerrariaApi.Server.ServerApi.Hooks.ClientChat);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.ClientChatReceived);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.GameGetKeyState);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.GameHardmodeTileUpdate);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.GameInitialize);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.GamePostInitialize);
			//SetCallback(TerrariaApi.Server.ServerApi.Hooks.GamePostUpdate); // 間隔が短すぎるため一旦コメントアウトする
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.GameStatueSpawn);
			//SetCallback(TerrariaApi.Server.ServerApi.Hooks.GameUpdate); // 間隔が短すぎるため一旦コメントアウトする
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.GameWorldConnect);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.GameWorldDisconnect);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.ItemNetDefaults);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.ItemSetDefaultsInt);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.ItemSetDefaultsString);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.NetGetData);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.NetGreetPlayer);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.NetNameCollision);
			//SetCallback(TerrariaApi.Server.ServerApi.Hooks.NetSendBytes); // NetSendDataでほぼ賄えるためコメントアウトする
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.NetSendData);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.NpcLootDrop);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.NpcNetDefaults);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.NpcSetDefaultsInt);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.NpcSetDefaultsString);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.NpcSpawn);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.NpcStrike);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.NpcTriggerPressurePlate);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.PlayerUpdatePhysics);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.ProjectileSetDefaults);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.ProjectileTriggerPressurePlate);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.ServerChat);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.ServerCommand);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.ServerConnect);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.ServerJoin);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.ServerLeave);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.ServerSocketReset);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.WorldChristmasCheck);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.WorldHalloweenCheck);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.WorldMeteorDrop);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.WorldSave);
			SetCallback(TerrariaApi.Server.ServerApi.Hooks.WorldStartHardMode);
			// リフレクション以下略

			TS.Commands.ChatCommands.Add(new TS.Command(new TS.CommandDelegate(cmd), "debugmsg"));
		}

		private void cmd(TS.CommandArgs args) {
			if (args.Parameters.Count > 0) {
				if (args.Parameters[0].Equals("on", StringComparison.CurrentCultureIgnoreCase)) {
					enabled = true;
				}
				else if (args.Parameters[0].Equals("off", StringComparison.CurrentCultureIgnoreCase)) {
					enabled = false;
				}
				else {
					enabled = !enabled;
				}
			}
			else {
				enabled = !enabled;
			}

			args.Player.SendMessage("DebugMsg " + (enabled ? "Enabled" : "Disabled"), Color.Green);
		}

		private void PrintMessage(string str) {
			if (!enabled) return;
			System.Diagnostics.StackFrame sf = new StackFrame(1);
			string time = DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss.fff");
			Debug.WriteLine(string.Format("{0} {1} : {2}", time, sf.GetMethod().Name , str));
		}

		private string CreateMessage<T>(T e){
			Type t = typeof(T);
			var ps = t.GetProperties();

			var pstrs = ps
				.Where(p => p.Name != "Handled")
				.Select(p => string.Format("{0}={1}", p.Name, p.GetValue(e, null)));
			return string.Join(", ", pstrs);
		}

		private void Callback<T>(T e, string text) {
			PrintMessage("[" + text + "] " + CreateMessage(e));
		}
		private void SetCallback<T>(HandlerCollection<T> handlerCollection) where T: EventArgs {
			handlerCollection.Register(this, n => Callback<T>(n, handlerCollection.hookName));
		}

		private void Handler<T>(object sender, T e)  {
			PrintMessage("[" + typeof(T).Name + "] " + CreateMessage(e));
		}
		private void SetHandler<T>(ref TS.HandlerList<T> handlerList) where T : EventArgs {
			var h = new TS.HandlerList<T>.HandlerItem() {
				Handler = Handler<T>,
				Priority = TS.HandlerPriority.Highest,
			};
			handlerList += h;
		}


		public override Version Version {
			get { return new Version("1.0"); }
		}
		public override string Name {
			get { return "Mackerel HandlerDebug Plugin"; }
		}
		public override string Author {
			get { return "Totoki Kei"; }
		}
		public override string Description {
			get { return "Show Handlers to debug output. (test plugin)"; }
		}
	}
#endif
}
