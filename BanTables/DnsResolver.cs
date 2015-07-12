using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using TS = TShockAPI;


namespace MackerelPluginSet.BanTables {
	public class DnsResolver {
		private class CacheEntry {
			public DateTime UpdateTime { get; private set; }
			public IPHostEntry HostEntry { get; private set; }

			public CacheEntry(IPHostEntry host) {
				Update(host);
			}

			public void Update(IPHostEntry newHost) {
				UpdateTime = DateTime.Now;
				HostEntry = newHost;
			}


			public bool IsExpired() {
				return UpdateTime + CacheDuration < DateTime.Now;
			}

		}

		private static int cleanupTimer;

		private static Dictionary<string, CacheEntry> cache = new Dictionary<string,CacheEntry>();
		private static readonly TimeSpan CacheDuration = new TimeSpan(0, 5, 0);

		public static string GetHostByIP(string ipAddress) {
			if (cleanupTimer++ > 100) {
				cleanupTimer = 0;
				// ToArrayしないと遅延評価になって削除に失敗する気がする
				var removeKeys = cache.Where(kv => kv.Value.IsExpired()).Select(kv => kv.Key).ToArray();

				foreach (string k in removeKeys) {
					cache.Remove(k);
				}
			}

			CacheEntry ca = null;
			if (cache.TryGetValue(ipAddress, out ca) && ca.IsExpired()) {
				// キャッシュが存在しかつ有効期限内
				return ca.HostEntry.HostName;
			}
			
			// IPアドレスからホスト名を取得する
			try {
				IPHostEntry hostInfo = Dns.GetHostEntry(ipAddress);

				// キャッシュの更新
				if (ca == null) {
					cache.Add(ipAddress, ca = new CacheEntry(hostInfo));
				}
				else {
					ca.Update(hostInfo);
				}

				return hostInfo.HostName;
			}
			catch (Exception e) {
				TS.TShock.Log.Info(string.Format(@"IP {0}'s hostname lookup failed : {1}", ipAddress, e.Message));
				return ipAddress;
			}
		}
	}
}
