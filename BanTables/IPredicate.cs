using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MackerelPluginSet.BanTables {

	public interface IPredicate {
		bool IsPublic { get; }
		string Name { get; }
		bool Judge(TShockAPI.TSPlayer player, string needle);
		bool Test(string testString, string needle);

	}
}
