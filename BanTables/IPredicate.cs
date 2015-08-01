using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MackerelPluginSet.BanTables {
	public class PredicateAttribute : Attribute {
		public PredicateAttribute() {

		}
	}


	public interface IPredicate {
		/// <summary>
		/// この条件は外部から利用可能
		/// (自分で作っておいてアレだけどなんでこんなbool値作ったんだろう)
		/// </summary>
		bool IsPublic { get; }
		/// <summary>
		/// 名前
		/// </summary>
		string Name { get; }
		/// <summary>
		/// 条件判定を行う
		/// </summary>
		/// <param name="player">判定対象のプレイヤー情報</param>
		/// <param name="param"></param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">paramが有効なパラメータでは有りません</exception>
		bool Judge(TShockAPI.TSPlayer player, object param);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="testString"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		bool Test(string testString, object param);
	}
}
