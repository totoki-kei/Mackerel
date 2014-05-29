using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MackerelPluginSet.ItemBroadcast {
	public class Configuration : IReadOnlyDictionary<int, string> {
		public static readonly string ConfigurationFileName = "ItemBroadcast.config";

		Dictionary<int, string> entries = new Dictionary<int, string>();

		public static Configuration Load() {

			if (!File.Exists(ConfigurationFileName)) {
				TShockAPI.Log.ConsoleInfo("File " + ConfigurationFileName + " dose not exists.");
				return null;
			}

			Configuration ret = new Configuration();

			try {
				using (StreamReader src = new StreamReader(ConfigurationFileName, Encoding.Default)) {
					string line;

					while ((line = src.ReadLine()) != null) {
						if (line.StartsWith("#")) continue;

						string[] splited = line.Split(new[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
						if (splited.Length != 2) continue;

						int id = -1;
						if (!int.TryParse(splited[0], out id)) continue;

						ret.entries.Add(id, splited[1]);
					}
				}
			}
			catch (Exception e) {
				TShockAPI.Log.ConsoleError(string.Format("File " + ConfigurationFileName + " open failed : {0}", e));
			}
			return ret;
		}


		#region IReadOnlyDictionary<int,string> メンバー

		public bool ContainsKey(int key) {
			return entries.ContainsKey(key);
		}

		public IEnumerable<int> Keys {
			get { return entries.Keys; }
		}

		public bool TryGetValue(int key, out string value) {
			return entries.TryGetValue(key, out value);
		}

		public IEnumerable<string> Values {
			get { return entries.Values; }
		}

		public string this[int key] {
			get { return entries[key]; }
		}

		#endregion


		#region IReadOnlyCollection<KeyValuePair<int,string>> メンバー

		public int Count {
			get { return entries.Count; }
		}

		#endregion

		#region IEnumerable<KeyValuePair<int,string>> メンバー

		public IEnumerator<KeyValuePair<int, string>> GetEnumerator() {
			return entries.GetEnumerator();
		}

		#endregion

		#region IEnumerable メンバー

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return ((System.Collections.IEnumerable)entries).GetEnumerator();
		}

		#endregion
	}
}
