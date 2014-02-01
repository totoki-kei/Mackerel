using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerPublisherCGI {
	public class ServerEntry {
		public class TextInformation {
			public string Name { get; set; }
			public string ShortDescription { get; set; }
			public string Description { get; set; }
			public string URL { get; set; }
			public string HostName { get; set; }
		}
		public TextInformation Description { get; set; }

		public class ServerSettingInformation {
			public IPAddress IP { get; set; }
			public ushort Port { get; set; }
			public WorldSize Size { get; set; }
			public int MaxUserCount { get; set; }
			public bool RequirePassword { get; set; }
		}
		public ServerSettingInformation Server { get; set; }

		public class StatusInformation {
			public DateTime RegisteredTime { get; set; }
			public DateTime LastHeartbeatTime { get; set; }
			public int UserCount { get; set; }

		}
		public StatusInformation Status { get; set; }
	}
}
