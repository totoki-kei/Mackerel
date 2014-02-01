using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using TShockAPI;

namespace MackerelPluginSet.Cooltime {
	[Serializable]
	public class Settings {
		public enum Scope {
			User,
			World,
		}

		public class Entry {
			public int ItemID { get; set; }
			public Scope Scope { get; set; }
			public int Count { get; set; }
			public int IntervalSeconds { get; set; }
		}

		public bool Enabled { get; set; }
		public Entry[] CooltimeEntries { get; set; }
	}
}
