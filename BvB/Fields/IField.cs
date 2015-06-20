using System;
namespace MackerelPluginSet.RegionImport.Fields {
	interface IField {
		void Apply(int startX, int startY);
		int Height { get; }
		int Width { get; }
	}
}
