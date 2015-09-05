using System;
using System.Collections.Generic;
using System.Text;

namespace MackerelPluginSet.Common
{
	[System.AttributeUsage(System.AttributeTargets.Class | AttributeTargets.Struct)]
	class ConfigurationFileAttribute : Attribute
    {
		public string FileName { get; }
		public ConfigurationFileAttribute(string filename) {
			FileName = filename;
		}
    }
}
