using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;

namespace MackerelPluginSet.BvB {
	class Scheduler {
		Thread thread;

		public enum GameState {
			Stop,
			Initialize,
			
			PlayerEntry,
			StartCountdown,
			Playing,
			Gameset,

			Terminate,
			Error,
		}

		TS.TSPlayer[] plr;

		public GameState State { get; private set; }

		public Scheduler() {
			State = GameState.Stop;
		}

		public void Start() {
			// ここで初期状態を作成
			this.State = GameState.Initialize;
			plr = new TS.TSPlayer[4];


			TS.GetDataHandlers.PlayerInfo.Register(OnPlayerInfo, TS.HandlerPriority.High);
			TS.GetDataHandlers.TogglePvp.Register(OnTogglePvP, TS.HandlerPriority.High);
		}

		private void OnTogglePvP(object sender, TS.GetDataHandlers.TogglePvpEventArgs e) {
		}

		// プレイヤー情報変更のハンドル
		private void OnPlayerInfo(object sender, TS.GetDataHandlers.PlayerInfoEventArgs e) {
		}


		private void SetPlayerToMember(TS.TSPlayer player) {

		}




	}
}
