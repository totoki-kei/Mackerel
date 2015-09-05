using System;
using System.Collections.Generic;
using System.Text;

namespace MackerelPluginSet.Common
{
    static class Utility
    {
		/// <summary>
		/// Terrariaゲーム内時間から、24時間制時間への変換
		/// </summary>
		/// <param name="daytime">Main.daytimeの値</param>
		/// <param name="gameTime">Main.timeの値</param>
		/// <returns>[0, 24)の、「時」単位の時間</returns>
		public static double ConvertGameTimeToTimeInHour(bool daytime, double gameTime) {
			double time = gameTime / 3600.0;
			time += 4.5;
			if (!daytime)
				time += 15.0;
			time = time % 24.0;
			return time;
		}

		/// <summary>
		/// Terrariaゲーム内時間から、24時間制時間への変換
		/// </summary>
		/// <param name="daytimeAndTime">Main.daytimeとMain.timeの値の組</param>
		/// <returns>[0, 24)の、「時」単位の時間</returns>
		public static double ConvertGameTimeToTimeInHour(Tuple<bool, double> daytimeAndTime) {
			return ConvertGameTimeToTimeInHour(daytimeAndTime.Item1, daytimeAndTime.Item2);
		}

		/// <summary>
		/// 24時間制時間から、Terrariaゲーム内時間への変換
		/// </summary>
		/// <param name="timeInHour">[0, 24)の、「時」単位の時間</param>
		/// <returns>Main.daytimeとMain.timeの値の組</returns>
		public static Tuple<bool, double> ConvertTimeInHourToGameTime(double timeInHour) {
			decimal time = (decimal)timeInHour;
			time -= 4.50m;
			if (time < 0.00m)
				time += 24.00m;

			if (time >= 15.00m) {
				return Tuple.Create(false, (double)((time - 15.00m) * 3600.0m));
			}
			else {
				return Tuple.Create(true, (double)(time * 3600.0m));
			}
		}
	}
}
