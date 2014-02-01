using System;
namespace MackerelPluginSet.BvB.Fields {
	interface IField {
		void Apply(int startX, int startY);
		int Height { get; }
		int Width { get; }
	}
}
