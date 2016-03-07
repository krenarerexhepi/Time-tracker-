using System;
using Mono;
using System.IO;
using Android.Content;
using Mono.Data.Sqlite;
using System.Collections.Generic;

namespace TimeTrackerMobile
{
	public class DatabaseHelper
	{
		private string dbPath = "";
		private string dbName = "krttm.sqlite";
		private Context context;
		private  string connectionDb = "";

		public DatabaseHelper (Context ctx)
		{
			dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), dbName);
			context = ctx;
			connectionDb =	"Data Source=" + dbPath;
			CheckIfDbExistsAndCreate ();
		}

		void CheckIfDbExistsAndCreate ()
		{
			if (!File.Exists (dbPath)) {
				var dbAssetStream = context.Assets.Open (dbName);
				var dbFileStream = new System.IO.FileStream (dbPath, System.IO.FileMode.OpenOrCreate);
				var buffer = new byte[1024];

				int b = buffer.Length;
				int length;

				while ((length = dbAssetStream.Read (buffer, 0, b)) > 0) {
					dbFileStream.Write (buffer, 0, length);
				}

				dbFileStream.Flush ();
				dbFileStream.Close ();
				dbAssetStream.Close ();
			}
		}

		public  List<Company> GetCompanies ()
		{

			return null;
		}

		public User GetUserDataForUserName (string username)
		{
			User con = new User ();
			using (var connection = new SqliteConnection (connectionDb)) {
				var cmd = connection.CreateCommand ();
				cmd.CommandText = " select * from Users where Username='" + username + "'";
				connection.Open ();
				var id1 = cmd.ExecuteReader ();
				while (id1.Read ()) {
					con.UserId = Convert.ToInt32 (id1 ["UserId"]);
					con.FirstName = id1 ["FirstName"].ToString ();
					con.LastName = (string)id1 ["LastName"].ToString ();
					con.Username = id1 ["Username"].ToString ();
					con.Password = (string)id1 ["Password"].ToString ();
				}
				connection.Close ();
				return con;

			}
		}


		public long RegisterUser (String name, String surname, String username, String password)
		{
			try {
				using (var connection = new SqliteConnection (connectionDb)) {
					var cmd = connection.CreateCommand ();
					cmd.CommandText = "insert into Users (FirstName, LastName, Username, Password)" +
					" Values (@FirstName, @LastName, @Username, @Password)";
					cmd.Parameters.AddWithValue ("FirstName", name);
					cmd.Parameters.AddWithValue ("LastName", surname);
					cmd.Parameters.AddWithValue ("Username", username);
					cmd.Parameters.AddWithValue ("Password", password);

					connection.Open ();
					var id1 = cmd.ExecuteNonQuery ();
					connection.Close ();
					var id = (long)id1;
					return id;
				}		
			} catch (Exception) {
				return (long)1;
			}
		}

		public long UpdateUser (int userId, string firstName, string lastName, string username, string password)
		{
			var connection = new SqliteConnection (connectionDb);
			string sql = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Username = @Username , Password=@Password WHERE UserId = @UserId";
			var cmd = connection.CreateCommand ();
			cmd.CommandText = sql;
			cmd.Parameters.AddWithValue ("UserId", userId);
			cmd.Parameters.AddWithValue ("FirstName", firstName);
			cmd.Parameters.AddWithValue ("LastName", lastName);
			cmd.Parameters.AddWithValue ("Username", username);
			cmd.Parameters.AddWithValue ("Password", password);
			connection.Open ();
			var id1 = cmd.ExecuteNonQuery ();
			connection.Close ();
			var id = (long)id1;
			return id;

		}

		public string SelectUsers ()
		{
			var output = "";
			using (var connection = new SqliteConnection (connectionDb)) {
				var cmd = connection.CreateCommand ();
				cmd.CommandText = " select * from Users";
				connection.Open ();
				var id1 = cmd.ExecuteReader ();
				while (id1.Read ()) {
					output += String.Format ("\n\tKey={0}; Value={1};" +
					"Value={2}, Value{3},Value{4}",
						id1 ["UserId"].ToString (),
						id1 ["FirstName"].ToString (),
						id1 ["LastName"].ToString (),
						id1 ["Username"].ToString (),
						id1 ["Password"].ToString ());
				}
				connection.Close ();
				return output;

			}
		}

		public  void DeleteUser (int userId)
		{
			var sql = string.Format ("DELETE FROM Users WHERE UserId = {0};", userId);

			using (var conn = new SqliteConnection (connectionDb)) {
				conn.Open ();

				using (var cmd = conn.CreateCommand ()) {
					cmd.CommandText = sql;
					cmd.ExecuteNonQuery ();
				}
			}
		}

		public User Login (String username, String password)
		{
			User con = new User ();
			using (var connection = new SqliteConnection (connectionDb)) {
				var cmd = connection.CreateCommand ();
				cmd.CommandText = "Select * from Users where Username ='" + username + "' and Password ='" + password + "'";
				connection.Open ();
				var id1 = cmd.ExecuteReader ();
				while (id1.Read ()) {
					con.UserId = Convert.ToInt32 (id1 ["UserId"]);
					con.FirstName = id1 ["FirstName"].ToString ();
					con.LastName = id1 ["LastName"].ToString ();
					con.Username = id1 ["Username"].ToString ();
					con.Password = id1 ["Password"].ToString ();
				}
				connection.Close ();
				return con;
			}
		}

		public long RegisterCompany (int userId, String companyName)
		{
			try {
				using (var connection = new SqliteConnection (connectionDb)) {
					var cmd = connection.CreateCommand ();
					cmd.CommandText = "insert into Companies (UserId, CompanyName)" +
					" Values (@UserId, @CompanyName)";
					cmd.Parameters.AddWithValue ("UserId", userId);
					cmd.Parameters.AddWithValue ("CompanyName", companyName);

					connection.Open ();
					var id1 = cmd.ExecuteNonQuery ();
					connection.Close ();
					var id = (long)id1;
					return id;
				}		
			} catch (Exception) {
				return (long)1;
			}
		}

		public long UpdateCompany (int companyId, int userId, String companyName)
		{
			var connection = new SqliteConnection (connectionDb);
			string sql = "UPDATE Users SET UserId = @UserId, CompanyName = @CompanyName WHERE CompanyId = @CompanyId";
			var cmd = connection.CreateCommand ();
			cmd.CommandText = sql;
			cmd.Parameters.AddWithValue ("UserId", userId);
			cmd.Parameters.AddWithValue ("CompanyName", companyName);
			connection.Open ();
			var id1 = cmd.ExecuteNonQuery ();
			connection.Close ();
			var id = (long)id1;
			return id;
		}

		public User GetUserDataForId (int userId)
		{
			User con = new User ();
			using (var connection = new SqliteConnection (connectionDb)) {
				var cmd = connection.CreateCommand ();
				cmd.CommandText = " select * from Users where UserId='" + userId + "'";
				connection.Open ();
				var id1 = cmd.ExecuteReader ();
				while (id1.Read ()) {
					con.UserId = Convert.ToInt32 (id1 ["UserId"]);
					con.FirstName = id1 ["FirstName"].ToString ();
					con.LastName = (string)id1 ["LastName"].ToString ();
					con.Username = id1 ["Username"].ToString ();
					con.Password = (string)id1 ["Password"].ToString ();
				}
				connection.Close ();
				return con;

			}
		}

		public List<Company> GetCompanyForUserList (int userId)
		{
			List<Company> lista = new List<Company> ();
			using (var connection = new SqliteConnection (connectionDb)) {
				var cmd = connection.CreateCommand ();
				cmd.CommandText = " select * from Companies where UserId=" + userId;
				connection.Open ();
				var id1 = cmd.ExecuteReader ();
				while (id1.Read ()) {
					Company con = new Company ();
					con.UserId = Convert.ToInt32 (id1 ["UserId"]);
					con.CompanyName = id1 ["CompanyName"].ToString ();
					con.CompanyId = Convert.ToInt32 (id1 ["CompanyId"]);
					lista.Add (con);
				}
				connection.Close ();
				return lista;

			}
		}

		public Company GetCompanyForUser (int userId)
		{
			Company com = new Company ();
			using (var connection = new SqliteConnection (connectionDb)) {
				var cmd = connection.CreateCommand ();
				cmd.CommandText = " select * from Companies where UserId=" + userId;
				connection.Open ();
				var id1 = cmd.ExecuteReader ();
				while (id1.Read ()) {
					com.UserId = Convert.ToInt32 (id1 ["UserId"]);
					com.CompanyName = id1 ["CompanyName"].ToString ();
					com.CompanyId = (int)id1 ["CompanyId"];

				}
				connection.Close ();
				return com;

			}
		}

		public List<Connections> GetConnectionsList (String name, int companyId)
		{
			List<Connections> lista = new List<Connections> ();
			using (var connection = new SqliteConnection (connectionDb)) {
				var cmd = connection.CreateCommand ();
				cmd.CommandText = " select * from Connections where ConnectionName='" + name + "'and CompanyId='" + companyId + "'"; 
				connection.Open ();
				var id1 = cmd.ExecuteReader ();
				while (id1.Read ()) {
					Connections con = new Connections ();
					con.ConnectionId = Convert.ToInt32 (id1 ["ConnectionId"]);
					con.ConnectionName = id1 ["ConnectionName"].ToString ();
					con.CompanyId = Convert.ToInt32 (id1 ["CompanyId"]);
					lista.Add (con);
				}
				connection.Close ();
				return lista;

			}
		}

		public Connections GetAllConnections ()
		{
			Connections con = new Connections ();
			using (var connection = new SqliteConnection (connectionDb)) {
				var cmd = connection.CreateCommand ();
				cmd.CommandText = " select * from Connections";
				connection.Open ();
				var id1 = cmd.ExecuteReader ();
				while (id1.Read ()) {
					con.ConnectionId = Convert.ToInt32 (id1 ["ConnectionId"]);
					con.CompanyId = Convert.ToInt32 (id1 ["CompanyId"]);
					con.ConnectionName = (string)id1 ["ConnectionName"];
				}
				connection.Close ();
				return con;

			}
		}

		public List<Connections> GetAllConnectionsList ()
		{
			List<Connections> lista = new List<Connections> ();
			using (var connection = new SqliteConnection (connectionDb)) {
				var cmd = connection.CreateCommand ();
				cmd.CommandText = " select * from Connections";
				connection.Open ();
				var id1 = cmd.ExecuteReader ();
				while (id1.Read ()) {
					Connections con = new Connections ();
					con.ConnectionId = Convert.ToInt32 (id1 ["ConnectionId"]);
					con.CompanyId = Convert.ToInt32 (id1 ["CompanyId"]);
					con.ConnectionName = (string)id1 ["ConnectionName"];
					lista.Add (con);
				}
				connection.Close ();
				return lista;

			}
		}

		public List<Connections> GetCompanyForConnection (int connectionId)
		{
			List<Connections> allEvide = new List<Connections> ();
			using (var connection = new SqliteConnection (connectionDb)) {
				var cmd = connection.CreateCommand ();
				cmd.CommandText = " select * from Connections where ConnectionId='" + connectionId + "'"; 
				connection.Open ();
				var id1 = cmd.ExecuteReader ();
				while (id1.Read ()) {
					Connections con = new Connections ();
					con.ConnectionId = Convert.ToInt32 (id1 ["ConnectionId"]);
					con.CompanyId = Convert.ToInt32 (id1 ["CompanyId"]);
					con.ConnectionName = (string)id1 ["ConnectionName"];
					allEvide.Add (con);
				}
				connection.Close ();
				return allEvide;
			}
		}

		public List<Company> GetCompanyId (String name)
		{
			List<Company> lista = new List<Company> ();
			using (var connection = new SqliteConnection (connectionDb)) {
				var cmd = connection.CreateCommand ();
				cmd.CommandText = "select * from Companies where CompanyName='" + name + "'";
				connection.Open ();
				var id1 = cmd.ExecuteReader ();
				while (id1.Read ()) {
					Company con = new Company ();
					con.UserId = Convert.ToInt32 (id1 ["UserId"]);
					con.CompanyName = id1 ["CompanyName"].ToString ();
					con.CompanyId = Convert.ToInt32 (id1 ["CompanyId"]);
					lista.Add (con);
				}
				connection.Close ();
				return lista;

			}
		}

		public void deleteCompanyForCompanyId (int companyId)
		{
			var sql = string.Format ("DELETE FROM Companies WHERE CompanyId = {0};", companyId);

			using (var conn = new SqliteConnection (connectionDb)) {
				conn.Open ();

				using (var cmd = conn.CreateCommand ()) {
					cmd.CommandText = sql;
					cmd.ExecuteNonQuery ();
				}
			}
		}

		public void deleteConnectionById (int connectionId)
		{
			var sql = "DELETE FROM Connections WHERE ConnectionId='" + connectionId + "'"; 

			using (var conn = new SqliteConnection (connectionDb)) {
				conn.Open ();

				using (var cmd = conn.CreateCommand ()) {
					cmd.CommandText = sql;
					cmd.ExecuteNonQuery ();
				}
			}
		}

		public  long RegisterConnection (int companyId, String connectionName)
		{
			try {
				using (var connection = new SqliteConnection (connectionDb)) {
					var cmd = connection.CreateCommand ();
					cmd.CommandText = "insert into Connections (CompanyId , ConnectionName)" +
					" Values (@CompanyId , @ConnectionName)";
					cmd.Parameters.AddWithValue ("CompanyId", companyId);
					cmd.Parameters.AddWithValue ("ConnectionName", connectionName);

					connection.Open ();
					var id1 = cmd.ExecuteNonQuery ();
					connection.Close ();
					var id = (long)id1;
					return id;
				}		
			} catch (Exception) {
				return (long)1;
			}
		}
		//todo: get connection per companyId
		public List<Connections> GetConnectionForCompany (int companyId)
		{
			List<Connections> lista = new List<Connections> ();
			using (var connection = new SqliteConnection (connectionDb)) {
				var cmd = connection.CreateCommand ();
				cmd.CommandText = " select * from Connections where CompanyId=" + companyId;
				connection.Open ();
				var id1 = cmd.ExecuteReader ();
				while (id1.Read ()) {
					Connections con = new Connections ();
					con.ConnectionId = Convert.ToInt32 (id1 ["ConnectionId"]);
					con.CompanyId = Convert.ToInt32 (id1 ["CompanyId"]);
					con.ConnectionName = (string)id1 ["ConnectionName"].ToString ();
					lista.Add (con);
				}
				connection.Close ();
				return lista;
			}
		}
		//todo: delete connection
		public void deleteConnections ()
		{
			var sql = string.Format ("DELETE FROM Connections");

			using (var conn = new SqliteConnection (connectionDb)) {
				conn.Open ();

				using (var cmd = conn.CreateCommand ()) {
					cmd.CommandText = sql;
					cmd.ExecuteNonQuery ();
				}
			}
		}
		//evidence
		//todo: register an evidence
		public long RegisterEvidence (string checkInTime, string checkOutTime, int connectionId, int companyId, int userId)
		{
			try {
				using (var connection = new SqliteConnection (connectionDb)) {
					var cmd = connection.CreateCommand ();
					cmd.CommandText = "insert into Evidence (CheckInTime, CheckOutTime, ConnectionId, CompanyId, UserId)" +
					" Values (@CheckInTime, @CheckOutTime, @ConnectionId, @CompanyId, @UserId)";
					cmd.Parameters.AddWithValue ("CheckInTime", checkInTime);
					cmd.Parameters.AddWithValue ("CheckOutTime", checkOutTime);
					cmd.Parameters.AddWithValue ("ConnectionId", connectionId);
					cmd.Parameters.AddWithValue ("CompanyId", companyId);
					cmd.Parameters.AddWithValue ("UserId", userId);

					connection.Open ();
					var id1 = cmd.ExecuteNonQuery ();
					connection.Close ();
					var id = (long)id1;
					return id;
				}		
			} catch (Exception) {
				return (long)1;
			}

		}
		//todo: update evidence for checkout

		public  long UpdateEvidence (int evidenceId, String checkInTime, String checkOutTime, int connectionId, int companyId, int userId)
		{
			var connection = new SqliteConnection (connectionDb);
			string sql = "UPDATE Evidence SET CheckInTime = @CheckInTime, CheckOutTime = @CheckOutTime," +
			             " ConnectionId = @ConnectionId, @CompanyId=CompanyId , UserId= @UserId WHERE EvidenceId = @EvidenceId";

			var cmd = connection.CreateCommand ();
			cmd.CommandText = sql;
			cmd.Parameters.AddWithValue ("CheckInTime", checkInTime);
			cmd.Parameters.AddWithValue ("CheckOutTime", checkOutTime);
			cmd.Parameters.AddWithValue ("ConnectionId", connectionId);
			cmd.Parameters.AddWithValue ("CompanyId", companyId);
			cmd.Parameters.AddWithValue ("UserId", userId);
			connection.Open ();
			var id1 = cmd.ExecuteNonQuery ();
			connection.Close ();
			var id = (long)id1;
			return id;

		}
		//todo: get all evidence per user & company
		public List<Evidence> GetEvidenceUserCompanyList (int userId, int companyId)
		{
			List<Evidence> allEvide = new List<Evidence> ();
			using (var connection = new SqliteConnection (connectionDb)) {
				var cmd = connection.CreateCommand ();
				cmd.CommandText = " select * from Evidence where UserId='" + userId + "'and CompanyId='" + companyId + "'"; 
				connection.Open ();
				var id1 = cmd.ExecuteReader ();
				while (id1.Read ()) {
					Evidence con = new Evidence ();
					con.EvidenceId = Convert.ToInt32 (id1 ["EvidenceId"]);
					con.CheckInTime = id1 ["CheckInTime"].ToString ();
					con.CheckOutTime = id1 ["CheckOutTime"].ToString ();
					con.CompanyId = Convert.ToInt32 (id1 ["CompanyId"]);
					con.UserId = Convert.ToInt32 (id1 ["UserId"]);
					allEvide.Add (con);
				}
				connection.Close ();
				return allEvide;
			}
		}

		public List<Evidence> GetLastEvidence (int userId)
		{
			List<Evidence> allEvide = new List<Evidence> ();
			using (var connection = new SqliteConnection (connectionDb)) {
				var cmd = connection.CreateCommand ();
				cmd.CommandText = "select * from Evidence where UserId='" + userId + "' ORDER BY EvidenceId DESC"; 
				connection.Open ();
				var id1 = cmd.ExecuteReader ();
				while (id1.Read ()) {
					Evidence con = new Evidence ();
					con.EvidenceId = Convert.ToInt32 (id1 ["EvidenceId"]);
					con.CheckInTime = id1 ["CheckInTime"].ToString ();
					con.CheckOutTime = id1 ["CheckOutTime"].ToString ();
					con.ConnectionId = Convert.ToInt32 (id1 ["ConnectionId"]);
					con.CompanyId = Convert.ToInt32 (id1 ["CompanyId"]);
					con.UserId = Convert.ToInt32 (id1 ["UserId"]);
					allEvide.Add (con);
				}
				connection.Close ();
				return allEvide;
			}
		}
	
	}

}

