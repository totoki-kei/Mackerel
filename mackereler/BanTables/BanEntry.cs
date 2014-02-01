
namespace MackerelPluginSet.BanTables {
	public class BanEntry {
		public int Priority { get; private set; }
		public bool AllowJudge { get; private set; }
		public string PredicateName { get; private set; }
		public BanPredicates.Function Predicate { get; private set; }
		public string Parameter { get; private set; }
		public string Reason { get; private set; }

		public BanEntry(int priority, string judge, string predicate, string parameter, string reason) {
			this.Priority = priority;
			this.AllowJudge = string.Compare(judge, "Allow", true) == 0;
			this.PredicateName = predicate;
			this.Predicate = BanPredicates.GetFunctionByName(predicate);
			this.Parameter = parameter;
			this.Reason = reason;
		}

		public bool IsMatches(TShockAPI.TSPlayer pl) {
			return Predicate != null ? Predicate(pl, this.Parameter) : false;
		}

	}
}
