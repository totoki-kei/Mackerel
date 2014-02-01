using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;

namespace ServerPublisherCGI {
	class Program {
		const bool TestMode = true;

		static readonly string LF = "\n";

		static void Main(string[] args) {
			PrintHTTPHeader();


		}

		private static void PrintHTTPHeader() {

			Console.Write(@"Content-type:text/html" + LF + LF);
		}
	}
}
