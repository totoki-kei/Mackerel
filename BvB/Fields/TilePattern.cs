using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MackerelPluginSet.BvB.Fields {

	public enum PaintColors : byte {
		None,
		Red,
		Orange,
		Yellow,
		Lime,
		Green,
		Teal,
		Cyan,
		SkyBlue,
		Blue,
		Purple,
		Violet,
		Pink,
		DeepRed,
		DeepOrange,
		DeepYellow,
		DeepLime,
		DeepGreen,
		DeepTeal,
		DeepCyan,
		DeepSkyBlue,
		DeepBlue,
		DeepPurple,
		DeepViolet,
		DeepPink,
		Black,
		White,
		Gray,
	};

	public class TilePattern {
		// static

		private static Dictionary<string, TilePattern> cache;

		public static TilePattern LoadOrGet(string filename) {
			TilePattern ret;
			if (cache == null) cache = new Dictionary<string, TilePattern>();
			if (cache.TryGetValue(filename, out ret)) {
				if (!File.Exists(filename) || File.GetLastWriteTime(filename) <= ret.Timestamp) {
					// ファイルが更新されていないときはキャッシュを返す
					return ret;
				}
				// 無効にする
				cache.Remove(filename);
			}
			try {
				ret = new TilePattern();
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

		// /static

		delegate void TileMod(Terraria.Tile tile, Queue<int> leadingNums);

		Dictionary<char, TileMod> tileModDic;
		public DateTime Timestamp { get; private set; }

		private TilePattern() { }

		enum Columns {
			mark,
			parent,
			tile,
			tileFrameX,
			tileFrameY,
			tileColor,
			tileActive,
			tileSmashed,
			wall,
			wallFrameX,
			wallFrameY,
			wallColor,
			liquid,
			liquidAmount,
			wire
		}

		int callCount = 0;
		public void ExecuteTileModOrDefault(Terraria.Tile tile, char c, Queue<int> nums) {
			if (callCount > 100) {
				// 再帰してる？
				return;
			}

			TileMod d;
			if (!tileModDic.TryGetValue(c, out d) || d == null) { d = (t, p) => { }; }

			callCount++;
			d(tile, nums);
			callCount--;

		}

		private void Load(string filename) {
			using (StreamReader reader = new StreamReader(filename, Encoding.GetEncoding(932), true)) {
				string line;
				bool dataReading = false;

				while (null != (line = reader.ReadLine())) {
					// 空行は無視する
					if (line.Length == 0) continue;

					// コメントは無視する
					if (line[0] == ';') continue;

					if (!dataReading) {
						if (line.StartsWith("#include")) {
							// インクルード
							string includeFile = line.Substring("#include".Length + 1).TrimStart(new[] { ' ', '\t' });
							Load(filename);
							continue;
						}
					}

					// 空行でもコメントでもヘッダでもない文字が来た時点でデータモード開始
					dataReading = true;

					ParseData(line);

				} // end while;
			}
			Timestamp = File.GetLastWriteTime(filename);
		}

		private void ParseData(string line) {

			string[] cols = line.Split(' ', '\t', ','); // スペース、タブ、カンマで区切る

			if (cols.Length < Enum.GetNames(typeof(Columns)).Length) {
				Array.Resize(ref cols, Enum.GetNames(typeof(Columns)).Length);
			}

			if (string.IsNullOrEmpty(cols[(int)Columns.mark])) {
				// charのないデータ行はエラー、無視する
				return;
			}
			char c = cols[(int)Columns.mark][0];

			// 設定対象のデリゲートを準備
			if (tileModDic == null) {
				tileModDic = new Dictionary<char, TileMod>();
			}
			if (!tileModDic.ContainsKey(c)) {
				tileModDic.Add(c, null);
			}

			string col;

			// parent
			col = cols[(int)Columns.parent];
			if (!string.IsNullOrEmpty(col)) {
				var parent_c = col[0];
				tileModDic[c] += ((t, p) =>
					ExecuteTileModOrDefault(t, parent_c, p)); // 遅延バインド
			}

			// tile,
			col = cols[(int)Columns.tile];
			if (!string.IsNullOrEmpty(col)) {
				int type;
				if (int.TryParse(col, out type)) {
					tileModDic[c] += ((t, p) =>
 {
						if (type == -1) {
							t.tileHeader &= (byte.MaxValue - 1);
						}
						else {
							t.type = (byte)type;
							t.tileHeader |= 1;
						}
					});
				}
			}

			// tileFrameX,
			col = cols[(int)Columns.tileFrameX];
			if (!string.IsNullOrEmpty(col)) {
				string[] colcols = col.Split('*');

				int a = colcols[0].To<int>(int.TryParse) ?? 0;
				bool aster = colcols.Length >= 2;

				if (aster) {
					int b = colcols.Length >= 2 ? colcols[1].To<int>(int.TryParse) ?? 1 : 1;

					tileModDic[c] += (t, p) =>
						t.frameX = (short)Math.Min(short.MaxValue, a + p.DequeueOrDefault() * b);
				}
				else {
					tileModDic[c] += (t, p) =>
						t.frameX = (short)Math.Min(short.MaxValue, a);
				}
			}

			// tileFrameY,
			col = cols[(int)Columns.tileFrameY];
			if (!string.IsNullOrEmpty(col)) {
				string[] colcols = col.Split('*');

				int a = colcols[0].To<int>(int.TryParse) ?? 0;
				bool aster = colcols.Length >= 2;

				if (aster) {
					int b = colcols.Length >= 2 ? colcols[1].To<int>(int.TryParse) ?? 1 : 1;

					tileModDic[c] += (t, p) =>
						t.frameY = (short)Math.Min(short.MaxValue, a + p.DequeueOrDefault() * b);
				}
				else {
					tileModDic[c] += (t, p) =>
						t.frameY = (short)Math.Min(short.MaxValue, a);
				}
			}

			// tileColor,
			col = cols[(int)Columns.tileColor];
			if (!string.IsNullOrEmpty(col)) {
				PaintColors clr = col.To<PaintColors>(Enum.TryParse<PaintColors>) ?? PaintColors.None;

				tileModDic[c] += (t, p) =>
				   t.color((byte)clr);
			}

			// tileActive,
			col = cols[(int)Columns.tileActive];
			if (!string.IsNullOrEmpty(col)) {
				bool? val = new bool?[] { null, true, false, true, false }["YNTF".IndexOf(col[0]) + 1];

				if (val.HasValue) {
					//tileModDic[c] += (t, p) =>
					//	t.active(val.Value);
					tileModDic[c] += (t, p) =>
						t.inActive(!val.Value);
				}
			}

			// tileSmashed,
			col = cols[(int)Columns.tileSmashed];
			if (!string.IsNullOrEmpty(col)) {
			//	int val = new int[] { -1, 0, 1, 2, 3, }["NRLH".IndexOf(col[0]) + 1];
				char val = col[0];


				switch (val) {
					//case 0:
					case 'N':
						tileModDic[c] += (t, p) => {
							t.halfBrick(false);
							t.slope(0);
						};
						break;
					//case 1:
					case 'R':
						tileModDic[c] += (t, p) => {
							t.halfBrick(false);
							t.slope(1);
						};
						break;
					//case 2:
					case 'L':
						tileModDic[c] += (t, p) => {
							t.halfBrick(false);
							t.slope(2);
						};
						break;
					//case 3:
					case 'H':
						tileModDic[c] += (t, p) => {
							t.halfBrick(true);
						};
						break;
				}

			}

			// wall,
			col = cols[(int)Columns.wall];
			if (!string.IsNullOrEmpty(col)) {
				byte b;
				if (byte.TryParse(col, out b)) {
					tileModDic[c] += (t, p) =>
						t.wall = b;
				}
			}

			// wallFrameX,
			col = cols[(int)Columns.wallFrameX];
			if (!string.IsNullOrEmpty(col)) {
				string[] colcols = col.Split('*');

				int a = colcols[0].To<int>(int.TryParse) ?? 0;
				bool aster = colcols.Length >= 2;

				if (aster) {
					int b = colcols.Length >= 2 ? colcols[1].To<int>(int.TryParse) ?? 1 : 1;

					tileModDic[c] += (t, p) =>
						t.wallFrameX(a + p.DequeueOrDefault() * b);
				}
				else {
					tileModDic[c] += (t, p) =>
						t.wallFrameX(a);

				}
			}

			// wallFrameY,
			col = cols[(int)Columns.wallFrameY];
			if (!string.IsNullOrEmpty(col)) {
				string[] colcols = col.Split('*');

				int a = colcols[0].To<int>(int.TryParse) ?? 0;
				bool aster = colcols.Length >= 2;

				if (aster) {
					int b = colcols.Length >= 2 ? colcols[1].To<int>(int.TryParse) ?? 1 : 1;

					tileModDic[c] += (t, p) =>
						t.wallFrameY(a + p.DequeueOrDefault() * b);
				}
				else {
					tileModDic[c] += (t, p) =>
						t.wallFrameY(a);
				}
			}

			// wallColor,
			col = cols[(int)Columns.wallColor];
			if (!string.IsNullOrEmpty(col)) {
				PaintColors clr = col.To<PaintColors>(Enum.TryParse<PaintColors>) ?? PaintColors.None;
				tileModDic[c] += (t, p) =>
					t.wallColor((byte)clr);
			}

			// liquid,
			col = cols[(int)Columns.liquid];
			if (!string.IsNullOrEmpty(col)) {
					int val = new int [] { -1, -1, 0, 1, 2 }["NWLH".IndexOf(col[0]) + 1];


					if (val == -1) {
						tileModDic[c] += (t, p) =>
							t.liquid = 0;
					}
					else {
						tileModDic[c] += (t, p) =>
							t.liquidType(val);
					}
			}

			// liquidAmount,
			col = cols[(int)Columns.liquidAmount];
			if (!string.IsNullOrEmpty(col)) {
				string[] colcols = col.Split('*');

				int a = colcols[0].To<int>(int.TryParse) ?? 0;
				bool aster = colcols.Length >= 2;

				if (aster) {
					int b = colcols.Length >= 2 ? colcols[1].To<int>(int.TryParse) ?? 1 : 1;

					tileModDic[c] += (t, p) =>
						t.liquid = (byte)(a + p.DequeueOrDefault() * b);
				}
				else {
					tileModDic[c] += (t, p) =>
						t.liquid = (byte)(a);

				}
			}

			// wire
			col = cols[(int)Columns.wire];
			if (!string.IsNullOrEmpty(col)) {
				if (col.IndexOf('N') == -1) {
					bool? red = null, green = null, blue = null;
					foreach (var w in col) {
						switch (w) {
							case 'R':
								red = true;
								break;
							case 'r':
								red = false;
								break;
							case 'G':
								green = true;
								break;
							case 'g':
								green = false;
								break;
							case 'B':
								blue = true;
								break;
							case 'b':
								blue = false;
								break;
						}
					}

					tileModDic[c] += (t, p) => {
						if (red.HasValue) t.wire(red.Value);
						if (green.HasValue) t.wire2(green.Value);
						if (blue.HasValue) t.wire3(blue.Value);
					};
				}
			}
		}
	}
}
