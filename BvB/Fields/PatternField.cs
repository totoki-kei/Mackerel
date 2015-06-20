using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Terraria;

namespace MackerelPluginSet.RegionImport.Fields {
	public class PatternField : MackerelPluginSet.RegionImport.Fields.IField {
		#region static
		private static Dictionary<string, PatternField> cache;

		static PatternField() {
			cache = new Dictionary<string, PatternField>();
		}

		public static PatternField LoadOrGet(string filename) {
			PatternField ret;
			if (cache.TryGetValue(filename, out ret)) {
				if (!File.Exists(filename) || File.GetLastWriteTime(filename) <= ret.Timestamp) {
					return ret;
				}
				cache.Remove(filename);
			}
			try {
				ret = new PatternField();
				ret.Load(filename);
				cache.Add(filename, ret);
			}
			catch {
				//TShockAPI.TShock.Utils.SendLogs()
				ret = null;
			}
			return ret;

		}

		public static void ResetCache() {
			cache.Clear();
		}


		#endregion

		TilePattern[] tilePatterns;
		string[][] fields;
		public DateTime Timestamp { get; private set; }

		public PatternField() { }

		public void Load(string filename) {
			List<TilePattern> ptns = new List<TilePattern>();
			List<string[]> g = new List<string[]>();

			using (StreamReader reader = new StreamReader(filename, Encoding.GetEncoding(932), true)) {
				string line;
				bool dataReading = false;

				while (null != (line = reader.ReadLine())) {
					// 空行は無視する
					if (line.Length == 0) continue;

					// コメントは無視する
					if (line[0] == ';') continue;

					if (!dataReading) {
						if (line.StartsWith("#tile")) {
							// インクルード
							string includeFile = line.Substring("#tile".Length + 1).TrimStart(new[] { ' ', '\t' });

							ptns.Add(TilePattern.LoadOrGet(includeFile));
							continue;
						}
					}

					// 空行でもコメントでもヘッダでもない文字が来た時点でデータモード開始
					dataReading = true;

					g.Add(line.Split(' ', '\t', ','));
				} // end while;
			}

			tilePatterns = ptns.ToArray();
			fields = g.ToArray();
			Timestamp = File.GetLastWriteTime(filename);
		}


		int width = -1;
		int height = -1;
		public int Width {
			get {
				if (width == -1) width = this.fields.Select(a => a.Length).Max();
				return width;
			}
		}

		public int Height {
			get {
				if (height == -1) height = this.fields.Length;
				return height;
			}
		}

		public void Apply(int startX, int startY) {

			int x1 = Math.Max(0, startX);
			int y1 = Math.Max(0, startY);
			int x2 = Math.Min(startX + Width, Terraria.Main.maxTilesX);
			int y2 = Math.Min(startY + Height, Terraria.Main.maxTilesY);

			for (int y = y1; y < y2; y++) {
				string[] column = fields[y - startY];
				for (int x = x1; x < x2; x++) {
					if (x - startX >= column.Length) break;

					Tile tile = Terraria.Main.tile[x, y] ?? new Tile();
					string s = column[x - startX];
					int i = 0;

					while (i < s.Length) {
						char c = s[i++];
						Queue<int> q = new Queue<int>();
						while (i < s.Length && char.IsNumber(s[i])) {
							q.Enqueue(s[i++] - '0');
						}

						foreach (var p in tilePatterns) {
							if (p == null) continue;
							p.ExecuteTileModOrDefault(tile, c, q);
						}
					}
					Terraria.Main.tile[x, y] = tile;
				}
			}

			Plugin.ResetSection(x1, y1, x2 - 1, y2 - 1);

		}
	}
}
