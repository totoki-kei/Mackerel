using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;

namespace MackerelPluginSet.ItemBroadcast {

	[ApiVersion(1, 20)]
	public class Plugin : TerrariaPlugin {

		public Plugin(Main game)
			: base(game) {	
		}

		Configuration config;

		public override void Initialize() {

			config = Configuration.Load();

			if (config == null) {
				TS.Log.ConsoleInfo("Mackerel ItemBroadcast Plugin is disabled.");
				return;
			}

			if (TS.GetDataHandlers.PlayerUpdate == null) {
				TS.GetDataHandlers.PlayerUpdate = new TS.HandlerList<TS.GetDataHandlers.PlayerUpdateEventArgs>();
			}
			TS.GetDataHandlers.PlayerUpdate.Register(
				new TS.HandlerList<TS.GetDataHandlers.PlayerUpdateEventArgs>.HandlerItem() { 
					Handler = new EventHandler<TS.GetDataHandlers.PlayerUpdateEventArgs>(OnPlayerUpdate)
				});


			TS.Log.ConsoleInfo("Mackerel ItemBroadcast Plugin is loaded.");

		}

		class Info {
			public DateTime time;
			public int lastItem;
		}

		Dictionary<byte, Info> cooltimes = new Dictionary<byte,Info>();

		private void OnPlayerUpdate(object sender, TS.GetDataHandlers.PlayerUpdateEventArgs e) {
			var player = TS.TShock.Players[e.PlayerId];
			if (player == null || player.PlayerData == null) return;

			var itemId = player.PlayerData.inventory[e.Item].netID;
			if (itemId == 0) return;

			// メッセージフォーマットを取得
			// もし取得できなければ対象アイテムではないので抜ける
			string msgfmt = null;
			if (!config.TryGetValue(itemId, out msgfmt)) return;

			if ((e.Control & 0x20) != 0) {
				DateTime time, now = DateTime.Now;
				Info info;
				if (cooltimes.TryGetValue(e.PlayerId, out info)) {
					time = info.time;
				}
				else {
					time = DateTime.MinValue;
				}

				if (time > now && (info == null ? false : info.lastItem == itemId)) {
					return;
				}
				cooltimes[e.PlayerId] = new Info() { time = now + TimeSpan.FromSeconds(5), lastItem = itemId };

				var item = TS.Utils.Instance.GetItemById(itemId);

				string msg = FormatMessage(msgfmt, player, item);

				TS.TSPlayer.All.SendInfoMessage(msg);
			}
			
		}

		private static class Tokens {
			public const string Time = "TIME";
			public const string Date = "DATE";

			public const string Name = "NAME";

			public const string Item = "ITEM";

		}

		private string FormatMessage(string format, TS.TSPlayer player, Item item) {
			char escape = '\x3000';
			while (format.IndexOf(escape) >= 0) escape++;

			var spf = format.Split(new[] { "%%" }, StringSplitOptions.None);

			for (int i = 0; i < spf.Length; i++) {
				StringBuilder sb = new StringBuilder(spf[i]);
				sb.Replace('%', escape);
				
				sb.Replace(escape.ToString() + Tokens.Date + escape.ToString(), DateTime.Now.ToString("yyyy/MM/dd"));
				sb.Replace(escape.ToString() + Tokens.Time + escape.ToString(), DateTime.Now.ToString("HH:mm:ss"));
				sb.Replace(escape.ToString() + Tokens.Name + escape.ToString(), player.Name);
				sb.Replace(escape.ToString() + Tokens.Item + escape.ToString(), item.name);

				sb.Replace(escape, '%');
				spf[i] = sb.ToString();
			}

			return string.Join("%", spf);
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

}
