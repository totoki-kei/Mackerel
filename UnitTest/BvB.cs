using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MackerelPluginSet.RegionImport.Fields;
using System.Collections.Generic;
using System.IO;

namespace UnitTest {
	[TestClass]
	public class RegionImport {
		[TestMethod]
		public void TestTilePattern() {

			Assert.IsTrue(File.Exists("baseField.tiles"));

			TilePattern tp = TilePattern.LoadOrGet(@"baseField.tiles");


			string[] testgrid = new[] {
				"b",
				"*",
				"C",
				"p",
				"L00",
				"L01",
				"L10",
				"L11",
				"L01-",
				"T0-",
				"T2-",
				"b-",
				"A",
			};

			foreach(var g in testgrid){
				Terraria.Tile tile = new Terraria.Tile();
				
				int i = 0;
				while (i < g.Length) {
					char c = g[i++];
					Queue<int> q = new Queue<int>();
					while (i < g.Length && char.IsNumber(g[i])) {
						q.Enqueue(g[i++] - '0');
					}

					tp.ExecuteTileModOrDefault(tile, c, q);
				}
			}
		}



		[TestMethod]
		public void TestField() {
			PatternField fld = new PatternField();

			fld.Load(@"baseField.field");


		}
	}
}
