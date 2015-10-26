using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TerrariaApi.Server;
using TS = TShockAPI;

namespace MackerelPluginSet.ItemJack {
	[ApiVersion(1, 22)]
	public class Plugin : TerrariaPlugin {
		TS.Command originalItemCommand;
		TS.Command originalGiveCommand;

		public Plugin(Main game)
			: base(game) {
		}

		public const string Permission = "mackerel.itemjack.spawn";

		public override void Initialize() {
			originalItemCommand = TS.Commands.ChatCommands.Find(c => c.Name == "item");
			originalGiveCommand = TS.Commands.ChatCommands.Find(c => c.Name == "give");

			List<string> itemPerm = new List<string>(originalItemCommand.Permissions);
			itemPerm.Add(Permission);

			TS.Commands.ChatCommands.Remove(originalItemCommand);
			TS.Commands.ChatCommands.Add(
				new TS.Command(itemPerm, OnItem, originalItemCommand.Names.ToArray()) {
					HelpText = originalItemCommand.HelpText,
				}
				);

			TS.Commands.ChatCommands.Remove(originalGiveCommand);
			TS.Commands.ChatCommands.Add(
				new TS.Command(itemPerm, OnGive, originalGiveCommand.Names.ToArray()) {
					HelpText = originalGiveCommand.HelpText,
				}
				);

			TS.TShock.Log.ConsoleInfo("Mackerel ItemJack Plugin is loaded.");
		}

		private void OnGive(TS.CommandArgs args) {
			// おおもとのGiveコマンドの処理を全コピー


			if (args.Parameters.Count < 2) {

				args.Player.SendErrorMessage(
					"Invalid syntax! Proper syntax: {0}give <item type/id> <player> [item amount] [prefix id/name]", TS.Commands.Specifier);
				return;
			}
			if (args.Parameters[0].Length == 0) {

				args.Player.SendErrorMessage("Missing item name/id.");
				return;
			}
			if (args.Parameters[1].Length == 0) {

				args.Player.SendErrorMessage("Missing player name.");
				return;
			}
			int itemAmount = 0;
			int prefix = 0;
			//var items = TS.Utils.Instance.GetItemByIdOrName(args.Parameters[0]);
			var items = TS.Utils.Instance.GetItemByIdOrName(args.Parameters[0]).Where(i => IsAllowed(i, args.Player)).ToList();
			args.Parameters.RemoveAt(0);
			string plStr = args.Parameters[0];
			args.Parameters.RemoveAt(0);
			if (args.Parameters.Count == 1)
				int.TryParse(args.Parameters[0], out itemAmount);
			else if (args.Parameters.Count == 2) {
				int.TryParse(args.Parameters[0], out itemAmount);
				var prefixIds = TS.Utils.Instance.GetPrefixByIdOrName(args.Parameters[1]);
				if (items[0].accessory && prefixIds.Contains(42)) {
					prefixIds.Remove(42);
					prefixIds.Remove(76);
					prefixIds.Add(76);
				}
				else if (!items[0].accessory && prefixIds.Contains(42))
					prefixIds.Remove(76);
				if (prefixIds.Count == 1)
					prefix = prefixIds[0];
			}


			if (items.Count == 0) {

				args.Player.SendErrorMessage("Invalid item type!");
			}
			else if (items.Count > 1) {

				TS.Utils.Instance.SendMultipleMatchError(args.Player, items.Select(i => i.name));
			}
			else {

				var item = items[0];
				if (item.type >= 1 && item.type < Main.maxItemTypes) {

					var players = TS.Utils.Instance.FindPlayer(plStr);
					if (players.Count == 0) {

						args.Player.SendErrorMessage("Invalid player!");
					}
					else if (players.Count > 1) {

						TS.Utils.Instance.SendMultipleMatchError(args.Player, players.Select(p => p.Name));
					}
					else {

						var plr = players[0];
						if (plr.InventorySlotAvailable || (item.name.Contains("Coin") && item.type != 905) || item.type == 58 || item.type == 184) {

							if (itemAmount == 0 || itemAmount > item.maxStack)
								itemAmount = item.maxStack;
							if (plr.GiveItemCheck(item.type, item.name, item.width, item.height, itemAmount, prefix)) {

								args.Player.SendSuccessMessage(string.Format("Gave {0} {1} {2}(s).", plr.Name, itemAmount, item.name));
								plr.SendSuccessMessage(string.Format("{0} gave you {1} {2}(s).", args.Player.Name, itemAmount, item.name));
							}
							else {


								args.Player.SendErrorMessage("You cannot spawn banned items.");
							}


						}
						else {


							args.Player.SendErrorMessage("Player does not have free slots!");
						}
					}
				}
				else {

					args.Player.SendErrorMessage("Invalid item type!");
				}
			}
		}

		private void OnItem(TS.CommandArgs args) {
			// おおもとのItemコマンドの処理を全コピー

			if (args.Parameters.Count < 1) {

				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}item <item name/id> [item amount] [prefix id/name]", TS.Commands.Specifier);
				return;
			}


			int amountParamIndex = -1;
			int itemAmount = 0;
			for (int i = 1; i < args.Parameters.Count; i++) {

				if (int.TryParse(args.Parameters[i], out itemAmount)) {

					amountParamIndex = i;
					break;
				}
			}


			string itemNameOrId;
			if (amountParamIndex == -1)
				itemNameOrId = string.Join(" ", args.Parameters);
			else
				itemNameOrId = string.Join(" ", args.Parameters.Take(amountParamIndex));


			Item item;
			//List<Item> matchedItems = TS.Utils.Instance.GetItemByIdOrName(itemNameOrId);
			List<Item> matchedItems = TS.Utils.Instance.GetItemByIdOrName(itemNameOrId).Where(i => IsAllowed(i, args.Player)).ToList();

			if (matchedItems.Count == 0) {

				args.Player.SendErrorMessage("Invalid item type!");
				return;
			}
			else if (matchedItems.Count > 1) {

				TS.Utils.Instance.SendMultipleMatchError(args.Player, matchedItems.Select(i => i.name));
				return;
			}
			else {

				item = matchedItems[0];
			}
			if (item.type < 1 && item.type >= Main.maxItemTypes) {

				args.Player.SendErrorMessage("The item type {0} is invalid.", itemNameOrId);
				return;
			}


			int prefixId = 0;
			if (amountParamIndex != -1 && args.Parameters.Count > amountParamIndex + 1) {

				string prefixidOrName = args.Parameters[amountParamIndex + 1];
				var matchedPrefixIds = TS.Utils.Instance.GetPrefixByIdOrName(prefixidOrName);


				if (item.accessory && matchedPrefixIds.Contains(42)) {
					matchedPrefixIds.Remove(42);
					matchedPrefixIds.Remove(76);
					matchedPrefixIds.Add(76);
				}
				else if (!item.accessory && matchedPrefixIds.Contains(42))
					matchedPrefixIds.Remove(76);

				if (matchedPrefixIds.Count > 1) {


					TS.Utils.Instance.SendMultipleMatchError(args.Player, matchedPrefixIds.Select(p => p.ToString()));
					return;
				}
				else if (matchedPrefixIds.Count == 0) {

					args.Player.SendErrorMessage("No prefix matched \"{0}\".", prefixidOrName);
					return;
				}
				else {

					prefixId = matchedPrefixIds[0];
				}
			}


			if (args.Player.InventorySlotAvailable || (item.type > 70 && item.type < 75) || item.ammo > 0 || item.type == 58 || item.type == 184)

				if (itemAmount == 0 || itemAmount > item.maxStack)
					itemAmount = item.maxStack;


			if (args.Player.GiveItemCheck(item.type, item.name, item.width, item.height, itemAmount, prefixId)) {

				item.prefix = (byte)prefixId;
				args.Player.SendSuccessMessage("Gave {0} {1}(s).", itemAmount, item.AffixName());
			}
			else {

				args.Player.SendErrorMessage("You cannot spawn banned items.");
			}
		}

		private bool IsAllowed(int i, TS.TSPlayer player) {
			return IsAllowed(TS.Utils.Instance.GetItemById(i), player);
		}

		private bool IsAllowed(Item i, TS.TSPlayer player) {
			// item権限がある場合は全アイテムOK
			return player.Group.Name.Equals("superadmin", StringComparison.CurrentCultureIgnoreCase)
				|| player.Group.permissions.Intersect(originalItemCommand.Permissions).Count() > 0
				|| (i.rare == 0 && !(new []{71, 72, 73, 74}.Contains(i.netID))); // お金以外のレア度0
		}
		
		public override Version Version {
			get { return new Version("1.1.2"); }
		}
		public override string Name {
			get { return "Mackerel ItemJack Plugin"; }
		}
		public override string Author {
			get { return "Totoki Kei"; }
		}
		public override string Description {
			get { return "Item/Give Command Wrapper."; }
		}
	}
}
