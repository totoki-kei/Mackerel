using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MackerelPluginSet.ChatLog {
	[Serializable]
	public class Settings {
		static Settings instance;
		public static Settings Instance { get { return instance ?? (instance = Load()); } }

		public bool Enabled { get; set; }
		public string LogPath { get; set; }
		public string Header { get; set; }
		public string Footer { get; set; }
		public string ChatFormat { get; set; }
		public string JoinFormat { get; set; }
		public string LeaveFormat { get; set; }
		public bool LogCommand { get; set; }

		private Settings() { }

		const string Filename = "Mackereler.ChatLog.config";

		private static Settings Load() {
			XmlSerializer serializer = new XmlSerializer(typeof(Settings));
			if (File.Exists(Filename)) {
				using (Stream stream = new FileStream(Filename, FileMode.OpenOrCreate, FileAccess.Read)) {
					return serializer.Deserialize(stream) as Settings;
				}
			}
			else {
				var ret = new Settings() {
					Enabled = false ,
					LogPath = "path/to/log/file-%DATE%-%TIME%.log",
					Header = "log start at %DATE% %TIME%",
					Footer = "log end at %DATE% %TIME%",
					ChatFormat = "%DATE% %TIME% CHAT [%IP%] %NAME%(%ID%) : %TEXT%",
					JoinFormat = "%DATE% %TIME% JOIN [%IP%] %NAME% joined.",
					LeaveFormat = "%DATE% %TIME% LEAV [%IP%] %NAME% leaved.",
					LogCommand = true,
				}; 
				using (Stream stream = new FileStream(Filename, FileMode.OpenOrCreate, FileAccess.Write)) {
					serializer.Serialize(stream, ret);
				}
				return ret;
			}
		}

	}
}
