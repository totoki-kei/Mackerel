using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
			funcTable.Add("uuid", JudgeUUID);
		}

		private static void CreateCoreTable() {
			coreTable = new Dictionary<string, CoreFunction>();
			coreTable.Add("name", JudgeName_Core);
			coreTable.Add("ip", JudgeIP_Core);
			coreTable.Add("hostname", JudgeHostname_Core);
			coreTable.Add("uuid", JudgeUUID_Core);
		}

		private static uint IPAddressToUint(IPAddress nwip) {
			uint mask;
			byte[] bs = nwip.GetAddressBytes();
			mask = ((uint)bs[0] << 24) | ((uint)bs[1] << 16) | ((uint)bs[2] << 8) | (uint)bs[3];
			return mask;
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
				IPAddress ip = IPAddress.Parse(ipAndNw[0]);
				IPAddress playerIp = IPAddress.Parse(s);

				IPAddress nwip;
				int nw;
				uint mask;
				if (ipAndNw.Length < 2 ){
					mask = 0xFFFFFFFFU;
				}
				else if (IPAddress.TryParse(ipAndNw[1], out nwip)) {
					mask = IPAddressToUint(nwip);
				}
				else if (int.TryParse(ipAndNw[1], out nw)) {
					mask = (uint)((0xFFFFFFFF00000000UL >> nw) & 0xFFFFFFFFU);
				}
				else {
					mask = 0xFFFFFFFFU;
				}

				uint fi = IPAddressToUint(ip);
				uint pi = IPAddressToUint(playerIp);
				
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

		static bool JudgeUUID(TShockAPI.TSPlayer p, string arg) {
			string s = p.UUID;
			return JudgeUUID_Core(s, arg);
		}

		private static bool JudgeUUID_Core(string s, string arg) {
			return s == arg;
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
