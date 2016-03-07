using System;
using SQLite;
using System.Collections.Generic;
using Mono.Data.Sqlite;

namespace TimeTrackerMobile
{
	public class DatabaseHandler
	{	
	
	//	SQLiteConnection db = new SQLiteConnection (System.IO.Path.Combine (System.Environment.GetFolderPath (Environment.SpecialFolder.Personal), "krttm.sqlite"));
		private static string db_file = "krttm.sqlite";
		SQLiteConnection db;
		private static SQLiteConnection GetConnection ()
		{
			var dbPath = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), db_file);
			bool exists = System.IO.File.Exists(dbPath);

			if (!exists)
				var db = new SQLiteConnection(dbPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite);

			var conn = new SQLiteConnection ("Data Source=" + dbPath);

			if (!exists)
				CreateDatabase (conn);

			return conn;
		}

		private static void CreateDatabase (SQLiteConnection connection)
		{
			var sql = "CREATE TABLE User (UserId INTEGER PRIMARY KEY AUTOINCREMENT, FirstName ntext,LastName ntext, Username ntext,Password ntext);";

		//	connection.Close ();

			using (var cmd = connection.CreateCommand ()) {
				cmd.CommandText = sql;
				cmd.ExecuteNonQuery ();
			}

			// Create a sample note to get the user started
			sql = "INSERT INTO Users (FirstName, LastName,Username,Password) VALUES (@FirstName, @LastName,@Username,@Password);";

			using (var cmd = connection.CreateCommand ()) {
				cmd.CommandText = sql;
				cmd.Bind.AddWithValue ("@Body", "Sample Note");
				cmd.Parameters.AddWithValue ("@Modified", DateTime.Now);

				cmd.ExecuteNonQuery ();
			}

			connection.Close ();
		}



		public string AddUser(string username, string password,string firstname,string lastname)
		{
			try {
				User item = new User();
				item.Username=username;
				item.Password=password;
				item.FirstName=firstname;
				item.LastName=lastname;

				db.Insert(item);
				return "E dhena u ruajt me sukses";
			} 
			catch (Exception ex) {
				return "Error" +ex.Message;
			}

		}

	}
}

