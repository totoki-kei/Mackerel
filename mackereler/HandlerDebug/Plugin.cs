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
	[ApiVersion(1, 14)]
	public class Plugin : TerrariaPlugin {

		public Plugin(Main game)
			: base(game) {	
		}

		public override void Initialize() {

			SetHandler(ref TS.GetDataHandlers.ChestItemChange, OnChestItemChange);
			SetHandler(ref TS.GetDataHandlers.ChestOpen, OnChestOpen);
			SetHandler(ref TS.GetDataHandlers.ItemDrop, OnItemDrop);
			SetHandler(ref TS.GetDataHandlers.KillMe, OnKillMe);
			SetHandler(ref TS.GetDataHandlers.LiquidSet, OnLiquidSet);
			SetHandler(ref TS.GetDataHandlers.NewProjectile, OnNewProjectile);
			SetHandler(ref TS.GetDataHandlers.NPCHome, OnNPCHome);
			SetHandler(ref TS.GetDataHandlers.NPCSpecial, OnNPCSpecial);
			SetHandler(ref TS.GetDataHandlers.NPCStrike, OnNPCStrike);
			SetHandler(ref TS.GetDataHandlers.PlayerAnimation, OnPlayerAnimation);
			SetHandler(ref TS.GetDataHandlers.PlayerBuff, OnPlayerBuff);
			SetHandler(ref TS.GetDataHandlers.PlayerBuffUpdate, OnPlayerBuffUpdate);
			SetHandler(ref TS.GetDataHandlers.PlayerDamage, OnPlayerDamage);
			SetHandler(ref TS.GetDataHandlers.PlayerHP, OnPlayerHP);
			SetHandler(ref TS.GetDataHandlers.PlayerInfo, OnPlayerInfo);
			SetHandler(ref TS.GetDataHandlers.PlayerMana, OnPlayerMana);
			SetHandler(ref TS.GetDataHandlers.PlayerSlot, OnPlayerSlot);
			SetHandler(ref TS.GetDataHandlers.PlayerSpawn, OnPlayerSpawn);
			SetHandler(ref TS.GetDataHandlers.PlayerTeam, OnPlayerTeam);
			SetHandler(ref TS.GetDataHandlers.PlayerUpdate, OnPlayerUpdate);
			SetHandler(ref TS.GetDataHandlers.SendTileSquare, OnSendTileSquare);
			SetHandler(ref TS.GetDataHandlers.Sign, OnSign);
			SetHandler(ref TS.GetDataHandlers.TileEdit, OnTileEdit);
			SetHandler(ref TS.GetDataHandlers.TileKill, OnTileKill);
			SetHandler(ref TS.GetDataHandlers.TogglePvp, OnTogglePvp);

			TS.Hooks.GeneralHooks.ReloadEvent += GeneralHooks_ReloadEvent;
			TS.Hooks.PlayerHooks.PlayerCommand += PlayerHooks_PlayerCommand;
			TS.Hooks.PlayerHooks.PlayerPostLogin += PlayerHooks_PlayerPostLogin;
			TS.Hooks.PlayerHooks.PlayerPreLogin += PlayerHooks_PlayerPreLogin;
		}

		void PlayerHooks_PlayerPreLogin(TS.Hooks.PlayerPreLoginEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		void PlayerHooks_PlayerPostLogin(TS.Hooks.PlayerPostLoginEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		void PlayerHooks_PlayerCommand(TS.Hooks.PlayerCommandEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		void GeneralHooks_ReloadEvent(TS.Hooks.ReloadEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private static void PrintMessage(string str) {
			System.Diagnostics.StackFrame sf = new StackFrame(1);
			string time = DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss.fff");
			Debug.WriteLine(string.Format("{0} {1} : {2}", time, sf.GetMethod().Name , str));
		}

		private string CreateMessage<T>(T e){
			Type t = typeof(T);
			var ps = t.GetProperties();
			StringBuilder sb = new StringBuilder();

			var pstrs = ps
				.Where(p => p.Name != "Handled")
				//.Select(p => new { p.Name, Type = p.GetType(), Value = p.GetValue(e, null)})
				//;
				.Select(p => string.Format("{0}={1}", p.Name, p.GetValue(e, null)));
			return string.Join(", ", pstrs);
		}

		private void OnTogglePvp(object sender, TS.GetDataHandlers.TogglePvpEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnTileKill(object sender, TS.GetDataHandlers.TileKillEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnTileEdit(object sender, TS.GetDataHandlers.TileEditEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnSign(object sender, TS.GetDataHandlers.SignEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnSendTileSquare(object sender, TS.GetDataHandlers.SendTileSquareEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnPlayerUpdate(object sender, TS.GetDataHandlers.PlayerUpdateEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnPlayerSpawn(object sender, TS.GetDataHandlers.SpawnEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnPlayerSlot(object sender, TS.GetDataHandlers.PlayerSlotEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnPlayerMana(object sender, TS.GetDataHandlers.PlayerManaEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnPlayerInfo(object sender, TS.GetDataHandlers.PlayerInfoEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnPlayerHP(object sender, TS.GetDataHandlers.PlayerHPEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnPlayerDamage(object sender, TS.GetDataHandlers.PlayerDamageEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnPlayerBuffUpdate(object sender, TS.GetDataHandlers.PlayerBuffUpdateEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnPlayerBuff(object sender, TS.GetDataHandlers.PlayerBuffEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnPlayerAnimation(object sender, TS.GetDataHandlers.PlayerAnimationEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnNPCStrike(object sender, TS.GetDataHandlers.NPCStrikeEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnNPCSpecial(object sender, TS.GetDataHandlers.NPCSpecialEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnNPCHome(object sender, TS.GetDataHandlers.NPCHomeChangeEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnNewProjectile(object sender, TS.GetDataHandlers.NewProjectileEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnLiquidSet(object sender, TS.GetDataHandlers.LiquidSetEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnKillMe(object sender, TS.GetDataHandlers.KillMeEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnItemDrop(object sender, TS.GetDataHandlers.ItemDropEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnChestOpen(object sender, TS.GetDataHandlers.ChestOpenEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnChestItemChange(object sender, TS.GetDataHandlers.ChestItemEventArgs e) {
			PrintMessage(CreateMessage(e));
		}

		private void OnPlayerTeam(object sender, TS.GetDataHandlers.PlayerTeamEventArgs e) {
			PrintMessage(CreateMessage(e));
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
