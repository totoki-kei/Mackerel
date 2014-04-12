using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MackerelPluginSet.KakaG {
	[Serializable]
	public class Configuration {

		[Serializable]
		public class GodModeConfiguration {
			public bool ObsidianSkinEnabled { get; set; }
			public bool PvPfragileEnabled { get; set; }
		}
		[Serializable]
		public class HPRequirementConfiguration {
			public int RequiredHP { get; set; }
			public string KickMessageFormat { get; set; }
		}

		public GodModeConfiguration GodMode;
		public HPRequirementConfiguration HPRequirement;


		[XmlIgnore]
		public static readonly string ConfigurationFileName = "KakaG.xml";

		public Configuration() {
			// デフォルト値の設定
			GodMode = new GodModeConfiguration() { ObsidianSkinEnabled = true, PvPfragileEnabled = true };
			HPRequirement = new HPRequirementConfiguration() { RequiredHP = 400, KickMessageFormat = "Need HP {0} over. your HP is {1}." };
		}

		public static Configuration Load() {
			XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
			Configuration cfg = null;
			if (!File.Exists(ConfigurationFileName)) {
				// ファイルがない

				cfg = new Configuration(); // 初期値
				try {
					// 初期値のファイルを作成
					using (var f = File.OpenWrite(ConfigurationFileName)) {
						serializer.Serialize(f, cfg);
					}
				}
				catch (IOException) {
					// にぎりつぶす
				}
			}
			else {
				try {
					using (var f = File.OpenRead(ConfigurationFileName)) {
						cfg = serializer.Deserialize(f) as Configuration;
					}
				}
				catch (IOException) {
					// デフォルト値を返す
					cfg = new Configuration();
				}

			}

			return cfg;

		}
	}
}
