using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace ServerListJp
{
	public class Properties : Dictionary<string, string>
	{
		private string name;
		private ArrayList properties = new ArrayList();
		public Properties(string name)
		{
			this.name = name;
		}
		public void registerProperty(string propertyName, string defaultValue = "false")
		{
			this.properties.Add(propertyName + Environment.NewLine + defaultValue);
		}
		public int getInteger(string propertyName)
		{
			int result = 0;
			if (!int.TryParse(base[propertyName], out result))
			{
				result = -1;
			}
			return result;
		}
		public bool getBoolean(string propertyName)
		{
			bool result = false;
			if (!bool.TryParse(base[propertyName], out result))
			{
				result = false;
			}
			return result;
		}
		public double getDouble(string propertyName)
		{
			double result = 0.0;
			if (!double.TryParse(base[propertyName], out result))
			{
				result = -1.0;
			}
			return result;
		}
		public void loadProperties()
		{
			string[] array = File.ReadAllLines(this.name + ".properties");
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (text.IndexOf('=') > -1 && text.Substring(0, 1) != "#")
				{
					string[] array2 = text.Split(new char[]
					{
						'='
					});
					array2[1] = array2[1].Split(new char[]
					{
						'#'
					})[0];
					if (array2[0] != "" && array2[1] != "")
					{
						base.Add(array2[0], array2[1]);
					}
					else
					{
						if (array2[0] != "")
						{
							base.Add(array2[0], "");
						}
					}
				}
			}
			foreach (string text2 in this.properties)
			{
				string[] array3 = text2.Split(Environment.NewLine.ToCharArray());
				Console.WriteLine(string.Concat(new string[]
				{
					"Property: ",
					array3[0],
					" missing in file ",
					this.name,
					".properties! Default set!"
				}));
				base.Add(array3[0], array3[1]);
			}
		}
	}
}
