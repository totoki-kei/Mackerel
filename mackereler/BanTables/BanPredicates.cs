using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MackerelPluginSet.BanTables {
	public static class BanPredicates {
		public delegate bool Function(TShockAPI.TSPlayer player, string parameter);
		public delegate bool CoreFunction(string val, string parameter);

		static Dictionary<string, Function> funcTable;
		static Dictionary<string, CoreFunction> coreTable;

		public static Function GetFunctionByName(string predicateName) {
			if (funcTable == null) {
				CreateFuncTable();
			}

			Function f;
			if (!funcTable.TryGetValue(predicateName.ToLower(), out f))
				f = null;
			return f;
		}

		public static CoreFunction GetCoreFunctionByName(string predicateName) {
			if (coreTable == null) {
				CreateCoreTable();
			}

			CoreFunction f;
			if (!coreTable.TryGetValue(predicateName.ToLower(), out f))
				f = null;
			return f;
		}


		private static void CreateFuncTable() {
			funcTable = new Dictionary<string, Function>();
			funcTable.Add("name", JudgeName);
			funcTable.Add("ip", JudgeIP);
			funcTable.Add("hostname", JudgeHostname);
		}

		private static void CreateCoreTable() {
			coreTable = new Dictionary<string, CoreFunction>();
			coreTable.Add("name", JudgeName_Core);
			coreTable.Add("ip", JudgeIP_Core);
			coreTable.Add("hostname", JudgeHostname_Core);
		}

		static bool JudgeName(TShockAPI.TSPlayer p, string arg) {
			string s = p.Name;
			return JudgeName_Core(s, arg);
		}

		private static bool JudgeName_Core(string s, string arg) {
			Regex r = new Regex(arg);
			return r.IsMatch(s);
		}

		static bool JudgeIP(TShockAPI.TSPlayer p, string arg) {
			string s = p.IP;
			return JudgeIP_Core(s, arg);
		}

		private static bool JudgeIP_Core(string s, string arg) {
			// normalize IP address
			try {
				string[] ipAndNw = arg.Split('/');
				byte[] fb = ipAndNw[0].Split('.').Select(str => byte.Parse(str)).ToArray();
				byte[] pb = s.Split('.').Select(str => byte.Parse(str)).ToArray();

				uint fi = 0;
				uint pi = 0;
				for (int i = 0; i < 4; i++) {
					fi = (uint)(fi << 8) + (i < fb.Length ? fb[i] : 0U);
					pi = (uint)(pi << 8) + (i < pb.Length ? pb[i] : 0U);
				}

				int nw;
				if (ipAndNw.Length < 2 || !int.TryParse(ipAndNw[1], out nw)) {
					nw = 32;
				}

				uint mask = (uint)((1UL << nw) - 1U) << (32 - nw);

				return (fi & mask) == (pi & mask);
			}
			catch (FormatException) {
				return false;
			}
		}

		static bool JudgeHostname(TShockAPI.TSPlayer p, string arg) {
			string s = DnsResolver.GetHostByIP(p.IP);
			return JudgeHostname_Core(s, arg);
		}

		private static bool JudgeHostname_Core(string s, string arg) {
			Regex r = new Regex(arg);
			return r.IsMatch(s);
		}


		public static IEnumerable<string> FunctionNames {
			get {
				if (funcTable == null)
					CreateFuncTable();

				return funcTable.Keys;
			}
		}
	}
}
