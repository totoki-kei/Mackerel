using System;
using System.Linq;

using TS = TShockAPI;

namespace MackerelPluginSet.BanTables {
	static class Commands {
		static TS.Command[] cmds;
		static DBTable db;

		public static void Initialize(DBTable db) {
			Commands.db = db;
			cmds = new TS.Command[] {
				new TS.Command(TS.Permissions.ban, OnCommand, "banex"),
			};

			TS.Commands.ChatCommands.AddRange(cmds);
		}

		static void OnCommand(TS.CommandArgs args) {
			if (args.Player.RealPlayer) {
				// サーバからしか使えないようにする
				args.Player.SendWarningMessage("This command can use in server console.");
				return;
			}

			string mode = null;
			if (args.Parameters.Count != 0) {
				mode = args.Parameters[0].ToLower();
			}

			if (mode == "list") {
				// BanTableの全エントリを表示
				var es = db.GetBanTables();
				if (es == null) {
					args.Player.SendErrorMessage("Failed to read entries. Please check log message.");
				}
				else {
					args.Player.SendInfoMessage("BanTable entries");
					int count = 0;
					foreach (var e in es) {
						args.Player.SendInfoMessage(string.Format("{0} {1}({2}) => {3}", e.Priority, e.PredicateName, e.Parameter, e.AllowJudge ? "Allow" : "Deny"));
						args.Player.SendInfoMessage(string.Format("     Reason : {0}", e.Reason));
						count++;
					}
					args.Player.SendInfoMessage("Total " + count.ToString() + " entries.");
				}
				return;
			}
			else if(mode == "add") {
				// 引数
				// 1. 優先度(Priority) : 必須
				// 2. 判定方法(Predicate) : 必須、選択式
				// 3. パラメータ(Parameter) : 必須
				// 4. 判定結果(Judge) : 任意(省略時は"Deny")
				// 5. 理由(Reason) : 任意(省略時は表示なし)
				int priority = 0;
				string pred = null;
				string param = null;
				string judge = "deny";
				string reason = "";

				switch (args.Parameters.Count) {
					case 4:
						if (!int.TryParse(args.Parameters[1], out priority)) {
							// 数値パースに失敗したらヘルプ表示へ
							goto PrintHelp;
						}
						pred = args.Parameters[2].ToLower();
						param = args.Parameters[3];
						break;
					case 5:
						judge = args.Parameters[4];
						goto case 4;
					case 6:
						reason = args.Parameters[5];
						goto case 5;
					default:
						goto PrintHelp;
				}

				// predが有効かどうか確認する
				if(!BanPredicates.FunctionNames.Contains(pred)){
					goto PrintHelp;
				}

				var e = new BanEntry(priority, judge, pred, param, reason);

				try {
					db.AddEntry(e);
					args.Player.SendSuccessMessage("BanTable entry added.");
				}
				catch (Exception ex) {
					TS.Log.Error(ex.ToString());
					Console.WriteLine(ex.StackTrace);
					args.Player.SendErrorMessage("Failed to add entry. please check log message.");
				}
				return;
			}
			else if (mode == "del") {
				// 引数
				// 1. 優先度(Priority) : 必須
				int priority = 0;
				if (args.Parameters.Count < 2)
					goto PrintHelp;

				if (!int.TryParse(args.Parameters[1], out priority))
					goto PrintHelp;

				try {
					db.DelEntry(priority);
					args.Player.SendSuccessMessage("BanTable entry deleted.");
				}
				catch (Exception ex) {
					TS.Log.Error(ex.ToString());
					Console.WriteLine(ex.StackTrace);
					args.Player.SendErrorMessage("Failed to delete entry. please check log message.");
				}
				return;
			}
			else if (mode == "move") {
				// 引数
				// 1. 優先度(Priority) : 必須
				// 2. 移動先優先度(NewPriority) : 必須
				int priority = 0;
				int newPriority = 0;
				if (args.Parameters.Count < 3)
					goto PrintHelp;

				if (!int.TryParse(args.Parameters[1], out priority))
					goto PrintHelp;
				if (!int.TryParse(args.Parameters[2], out newPriority))
					goto PrintHelp;

				try {
					db.MoveEntry(priority, newPriority);
					args.Player.SendSuccessMessage("BanTable entry moved.");
				}
				catch (Exception ex) {
					TS.Log.Error(ex.ToString());
					Console.WriteLine(ex.StackTrace);
					args.Player.SendErrorMessage("Failed to move entry. please check log message.");
				}
				return;
			}
			else if (mode == "test") {
				// 引数
				// 1. 判定方法(Predicate) : 必須
				// 2. パラメータ(Parameter) : 必須
				string pred = null;
				string param = null;

				if (args.Parameters.Count < 3)
					goto PrintHelp;
				pred = args.Parameters[1].ToLower();
				param = args.Parameters[2];


				var banList = db.GetBanTables();
				var fn = BanPredicates.GetCoreFunctionByName(pred);
				if (fn == null) {
					args.Player.SendErrorMessage(string.Format("Bad function type \"{0}\"", pred));
					return;
				}

				foreach (var b in banList) {
					if (b.PredicateName == pred) {
						var matched = fn(param, b.Parameter);
						string msg = string.Format("#{0} : test by \"{1}\" => {2}", b.Priority, b.Parameter, matched ? b.AllowJudge ? "Allow" : "Deny" : "Unmatched");
						args.Player.SendInfoMessage(msg);
						if (matched) break;
					}
				}

				return;
			}
			
			PrintHelp:
			args.Player.SendInfoMessage("banex commands:");
			args.Player.SendInfoMessage(" /banex list");
			args.Player.SendInfoMessage(" /banex add <priority> <function> <parameter> [allow|deny [<Reason>]] ");
			args.Player.SendInfoMessage(" /banex del <priority>");
			args.Player.SendInfoMessage(" /banex move <priority> <new_priority>");
			args.Player.SendInfoMessage(" /banex test <function> <value>");
			args.Player.SendInfoMessage("available functions : " + string.Join(", ", BanPredicates.FunctionNames));
		}
	}
}
