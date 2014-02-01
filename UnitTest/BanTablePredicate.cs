using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MackerelPluginSet.BanTables;

namespace UnitTest {
	[TestClass]
	public class BanTablePredicate {
		[TestMethod]
		public void CoreFuncTest() {
			var ip = BanPredicates.GetCoreFunctionByName("ip");
			var name = BanPredicates.GetCoreFunctionByName("name");
			var host = BanPredicates.GetCoreFunctionByName("hostname");
			var other = BanPredicates.GetCoreFunctionByName("unused");

			Assert.IsNotNull(ip);
			Assert.IsNotNull(name);
			Assert.IsNotNull(host);
			Assert.IsNull(other);
		}
	}
}
