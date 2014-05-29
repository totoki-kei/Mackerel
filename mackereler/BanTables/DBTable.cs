using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using TShockAPI;
using TShockAPI.DB;

namespace MackerelPluginSet.BanTables {
	public class DBTable {
		IDbConnection database;
		SqlTable table;
		public const string TableName = "BanEx";


		public DBTable(System.Data.IDbConnection database) {
			this.database = database;
			try {
				EnsureExistTable(database);
			}
			catch (DllNotFoundException) {
				System.Console.WriteLine("Possible problem with your database - is Sqlite3.dll present?");
				throw new Exception("Could not find a database library (probably Sqlite3.dll)");
			}
		}

		private void EnsureExistTable(System.Data.IDbConnection database) {
			table = new SqlTable(TableName,
				new SqlColumn("Priority", MySqlDbType.Int32) { NotNull = true, Unique = true, Primary = true },
				new SqlColumn("Judge", MySqlDbType.String, 8) { NotNull = true, DefaultValue = "Deny" },
				new SqlColumn("Predicate", MySqlDbType.String, 64) { NotNull = true },
				new SqlColumn("Parameter", MySqlDbType.Text),
				new SqlColumn("Reason", MySqlDbType.Text)
			);
			var creator = new SqlTableCreator(database,
											  database.GetSqlType() == SqlType.Sqlite
												? (IQueryBuilder)new SqliteQueryCreator()
												: new MysqlQueryCreator());
			creator.EnsureExists(table);
		}

		public IEnumerable<BanEntry> GetBanTables() {
			List<BanEntry> list = new List<BanEntry>();
			try {
				using (var reader = database.QueryReader("SELECT Priority, Judge, Predicate, Parameter, Reason FROM " + table.Name + " ORDER BY Priority;")) {
					while (reader.Read()) {
						var priority = reader.Get<int>("Priority");
						var judge = reader.Get<string>("Judge");
						var predicate = reader.Get<string>("Predicate");
						var parameter = reader.Get<string>("Parameter");
						var reason = reader.Get<string>("Reason");

						var banentry = new BanEntry(priority, judge, predicate, parameter, reason);
						list.Add(banentry);
					}
					//list.Sort((e1, e2) => e1.Priority.CompareTo(e2.Priority));
					return list;
				}
			}
			catch (Exception ex) {
				Log.Error(ex.ToString());
				Console.WriteLine(ex.StackTrace);
			}
			return null;
		}

		public void AddEntry(BanEntry entry) {
			database.Query("INSERT INTO " + table.Name + @" VALUES (@0, @1, @2, @3, @4);",
				entry.Priority, entry.AllowJudge ? "Allow" : "Deny", entry.PredicateName, entry.Parameter, entry.Reason);
		}

		public void DelEntry(int priority) {
			try {
				var cmd = database.Query("DELETE FROM " + table.Name + " WHERE Priority=@0;", priority);
			}
			catch (Exception ex) {
				Log.Error(ex.ToString());
				Console.WriteLine(ex.StackTrace);
			}
		}

		public void MoveEntry(int priority, int new_priority) {
			try {
				var cmd = database.Query("UPDATE " + table.Name + " SET Priority=@0 WHERE Priority=@1;", new_priority, priority);
			}
			catch (Exception ex) {
				Log.Error(ex.ToString());
				Console.WriteLine(ex.StackTrace);
			}
		}
	}
}
