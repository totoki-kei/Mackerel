using System;
using System.Net;

using TS = TShockAPI;


namespace MackerelPluginSet.BanTables {
	public class DnsResolver {
		public static string GetHostByIP(string ipAddress) {
			// IPアドレスからホスト名を取得する
			try {
				IPHostEntry hostInfo = Dns.GetHostEntry(ipAddress);
				return hostInfo.HostName;
			}
			catch (Exception e) {
				TS.Log.Info(string.Format(@"IP {0}'s hostname lookup failed : {1}", ipAddress, e.Message));
				return ipAddress;
			}
		}
	}
}
