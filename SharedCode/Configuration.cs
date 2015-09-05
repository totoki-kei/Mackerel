using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

namespace MackerelPluginSet.Common {
	static class Configuration {
		public static T Load<T>(string fileName, ConfigurationLoadMode mode) where T : new() {
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			try {
				using (var f = File.OpenRead(fileName)) {
					return (T)serializer.Deserialize(f);
				}
			}
			catch {
				switch (mode) {
					case ConfigurationLoadMode.ThrowIfFailed:
						throw;
					case ConfigurationLoadMode.NullIfFailed:
						return default(T);
					case ConfigurationLoadMode.DefaultIfFailed:
						return new T(); // 初期値
					case ConfigurationLoadMode.CreateIfFailed:
						T cfg = new T(); // 初期値
						try {
							// 初期値のファイルを作成
							using (var f = File.OpenWrite(fileName)) {
								serializer.Serialize(f, cfg);
							}
						}
						catch {
							/* 多重例外は無視する */
						}
						return cfg;
					default:
						throw new ArgumentException(nameof(mode));
				}
			}

		}

		public static void Save<T>(string fileName, T data, bool throwIfFailed) {
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			try {
				// ファイルを作成
				using (var f = File.OpenWrite(fileName)) {
					serializer.Serialize(f, data);
				}
			}
			catch {
				if (throwIfFailed) throw;
			}
		}

		#region 属性使用バージョン
		private static ConfigurationFileAttribute GetFileAttribute<T>() {
			var attr = Attribute.GetCustomAttribute(typeof(T), typeof(ConfigurationFileAttribute))
				as ConfigurationFileAttribute;
			if (attr == null) throw new ArgumentException();
			return attr;
		}

		public static T Load<T>(ConfigurationLoadMode mode) where T : new() {
			var attr = GetFileAttribute<T>();
			return Load<T>(attr.FileName, mode);
		}

		public static void Save<T>(T data, bool throwIfFailed) {
			var attr = GetFileAttribute<T>();
			Save<T>(attr.FileName, data, throwIfFailed);
		}
		#endregion

	}

}
