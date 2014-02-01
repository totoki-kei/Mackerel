using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MackerelPluginSet.ChatLog {
	class LogFile {
		StreamWriter dest;

		public LogFile() {
			string path = Settings.Instance.LogPath
				.Replace("%DATE%", DateTime.Now.ToString("yyyyMMdd"))
				.Replace("%TIME%", DateTime.Now.ToString("HHmmss"));
			dest = new StreamWriter(path, true, Encoding.UTF8);
			dest.AutoFlush = true;
		}

		public void Append(string str) {
			dest.WriteLine(str);
		}

		~LogFile() {
			dest.Close();
		}

	}
}
