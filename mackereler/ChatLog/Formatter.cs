using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TS = TShockAPI;
using Terraria;

namespace MackerelPluginSet.ChatLog {
	class Formatter {
		private string header;
		public string HeaderFormat { get { return header; } set { header = value; } }
		private string footer;
		public string FooterFormat { get { return footer; } set { footer = value; } }
		private string chat;
		public string ChatFormat { get { return chat; } set { chat = value; } }
		private string join;
		public string JoinFormat { get { return join; } set { join = value; } }
		private string leave;
		public string LeaveFormat { get { return leave; } set { leave = value; } }


		public Formatter() {
			header = Settings.Instance.Header;
			footer = Settings.Instance.Footer;
			chat = Settings.Instance.ChatFormat;
			join = Settings.Instance.JoinFormat;
			leave = Settings.Instance.LeaveFormat;
		}

		private static class Tokens {
			public const string Time = "TIME";
			public const string Date = "DATE";

			public const string Name = "NAME";
			public const string IP = "IP";
			public const string ID = "ID";
			public const string Text = "TEXT";

			public const string PositionX = "X";
			public const string PositionY = "Y";

		}

		private string Format(string format, TS.TSPlayer player, messageBuffer msgBuffer, int id, string text) {
			char escape = '\x3000';
			while (format.Contains(escape)) escape++;

			var spf = format.Split(new[] { "%%" }, StringSplitOptions.None);

			for (int i = 0; i < spf.Length; i++) {
				StringBuilder sb = new StringBuilder(spf[i]);
				sb.Replace('%', escape);
				{ // always
					sb.Replace(escape.ToString() + Tokens.Date + escape.ToString(), DateTime.Now.ToString("yyyy/MM/dd"));
					sb.Replace(escape.ToString() + Tokens.Time + escape.ToString(), DateTime.Now.ToString("HH:mm:ss"));
				}
				if (player != null) {
					sb.Replace(escape.ToString() + Tokens.Name + escape.ToString(), player.Name);
					sb.Replace(escape.ToString() + Tokens.IP + escape.ToString(), player.IP.PadRight(15));
					sb.Replace(escape.ToString() + Tokens.PositionX + escape.ToString(), player.X.ToString("F2").PadLeft(8));
					sb.Replace(escape.ToString() + Tokens.PositionY + escape.ToString(), player.Y.ToString("F2").PadLeft(8));
				}
				if (text != null) {
					sb.Replace(escape.ToString() + Tokens.ID + escape.ToString(), "#" + id.ToString("X6"));
					sb.Replace(escape.ToString() + Tokens.Text + escape.ToString(), text);
				}


				sb.Replace(escape, '%');
				spf[i] = sb.ToString();
			}

			return string.Join("%", spf);
			
		}

		internal string FormatChat(TS.TSPlayer pl, messageBuffer arg1, int arg2, string arg3) {
			return Format(chat, pl, arg1, arg2, arg3);
		}

		internal string FormatJoin(TS.TSPlayer pl) {
			return Format(join, pl, null, 0, null);
		}

		internal string FormatLeave(TS.TSPlayer pl) {
			return Format(leave, pl, null, 0, null);
		}
	}
}
