using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace MackerelPluginSet.BvB {
	static class ExtMethods {
		public delegate bool TryParseMethod<T>(string s, out T t);
		public static T? To<T>(this string s, TryParseMethod<T> m) where T:struct {
			T ret;
			if (m(s, out ret)) return ret;
			else return null;
		}

		public static T DequeueOrDefault<T>(this Queue<T> queue) {
			try {
				return queue.Count == 0 ? default(T) : queue.Dequeue();
			}
			catch {
				return default(T);
			}
		}
	}
}
