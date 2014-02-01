using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Terraria;

namespace MackerelPluginSet.BvB.Fields {
	/// <summary>
	/// WorldEditプラグイン (by MarioE) のクリップボードデータを使用するフィールド
	/// </summary>
	class WorldEditField : IField {
		Tile[,] tile;

		private static class Helper {
			public static Tile[,] LoadWorldData(string path, out int width, out int height) {
				Tile[,] tile;
				// GZipStream is already buffered, but it's much faster to have a 1 MB buffer.
				using (var reader =
					new BinaryReader(
						new BufferedStream(
							new GZipStream(File.Open(path, FileMode.Open), CompressionMode.Decompress), 1048576))) {
					reader.ReadInt32();
					reader.ReadInt32();
					width = reader.ReadInt32();
					height = reader.ReadInt32();
					tile = new Tile[width, height];

					for (int i = 0; i < width; i++) {
						for (int j = 0; j < height; j++)
							tile[i, j] = ReadTile(reader);
					}
					return tile;
				}
			}

			public static Tile ReadTile(BinaryReader reader) {
				Tile tile = new Tile();
				byte flags = reader.ReadByte();
				byte flags2 = reader.ReadByte();
				tile.tileHeader = (byte)(flags & 0xf1);
				tile.tileHeader2 = (byte)(flags2 & 0x03);
				tile.tileHeader3 = (byte)(flags2 & 0x30);

				// Color
				if ((flags2 & 4) != 0)
					tile.color(reader.ReadByte());
				// Wall color
				if ((flags2 & 8) != 0)
					tile.wallColor(reader.ReadByte());
				// Tile type
				if ((flags & 1) != 0) {
					tile.type = reader.ReadByte();
					if (Main.tileFrameImportant[tile.type]) {
						tile.frameX = reader.ReadInt16();
						tile.frameY = reader.ReadInt16();
					}
				}
				// Wall type
				if ((flags & 4) != 0)
					tile.wall = reader.ReadByte();
				// Liquid
				if ((flags & 8) != 0) {
					tile.liquid = reader.ReadByte();
					tile.liquidType(reader.ReadByte());
				}
				return tile;
			}
		}

		public WorldEditField() { }

		public void Load(string filename) {
			this.tile = Helper.LoadWorldData(filename, out width, out height);
		}

		int width;
		int height;

		#region IField メンバー

		public void Apply(int startX, int startY) {
			
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					Terraria.Main.tile[startX + x, startY + y] = this.tile[x, y];
				}
			}
			Plugin.ResetSection(startX, startY, startX + width, startY + height);
		}

		public int Height { get { return height; } }
		public int Width { get { return width; } }

		#endregion
	}
}
