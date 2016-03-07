using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Net.Wifi;
using System.Collections.Generic;
using System.Threading.Tasks;
using Java.Lang;

namespace TimeTrackerMobile
{
	[Activity (Label = "MainActivity")]
	public class MainActivity : Activity
	{
		DatabaseHelper dbHelper;
		TextView txt;
		string userId;
		string logIn;
		ListView lastEvidenceList;
		static int INTERVAL = 1000 * 60 * 1;


		//	DatabaseHandler db = new DatabaseHandler();
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_main);
			dbHelper = new DatabaseHelper (ApplicationContext);


			txt = FindViewById<TextView> (Resource.Id.txtNameAndSurname);

			userId = Intent.GetStringExtra ("UserId");
			logIn = Intent.GetStringExtra ("LogIn");
			lastEvidenceList = FindViewById <ListView> (Resource.Id.lsvLastrecords);
			string us = dbHelper.SelectUsers ();
			User user = dbHelper.GetUserDataForId (Convert.ToInt32 (userId));
			txt.Text = user.FirstName + "  " + user.LastName;

			DateTime dateWithoutTime = DateTime.Now;
			Runner (ApplicationContext, Convert.ToString (dateWithoutTime));
		

		
			//Task task = new Task (() => 
			//		AddEvidence (ApplicationContext, dateWithoutTime)
			//            );
			//task.Start ();
			//task.Wait (100);
			/*

			//runnable.run();
			GetLastEvidence();*/
		
			txt.Click += txt_Click;
				
			Button btnConnections = FindViewById<Button> (Resource.Id.btnConnections);
			btnConnections.Click += delegate {
				Intent sendToWifi = new Intent (this, typeof(ConnectionsListActivity));
				sendToWifi.PutExtra ("UserId", (userId));
				StartActivityForResult (sendToWifi, 0);
			};

			Button btnCompany = FindViewById<Button> (Resource.Id.btnCompanies);
			btnCompany.Click += delegate {
				Intent sendToWifi = new Intent (this, typeof(CompanyActivity));
				sendToWifi.PutExtra ("UserId", (userId));
				StartActivityForResult (sendToWifi, 0);
			};
			Button btnArchive = FindViewById<Button> (Resource.Id.btnArchive);
			btnArchive.Click += delegate {
				Intent sendToWifi = new Intent (this, typeof(ArchiveActivity));
				sendToWifi.PutExtra ("UserId", (userId));
				StartActivityForResult (sendToWifi, 0);
			};


		}

		void Runner (Context con, string date)
		{
			Thread t = new Thread (() => AddEvidence (con, date));
			t.Start ();
			//t.Wait (5 * 60 * 1000);

		}

		void txt_Click (object sender, EventArgs e)
		{
			Intent sendToWifi = new Intent (this, typeof(LogInActivity));
			sendToWifi.PutExtra ("UserId", (userId));
			sendToWifi.PutExtra ("LogIn", logIn);
			StartActivityForResult (sendToWifi, 0);
		}

		public override void OnBackPressed ()
		{
			Intent a = new Intent (this, typeof(MainActivity));
			a.AddCategory (Intent.CategoryHome);
			a.SetFlags (Intent.Flags);
			StartActivity (a);
		}



		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Layout.settings, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		ISharedPreferences settings;

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.action_about:
				return true;			
			case Resource.Id.action_LogOut:
				settings = BaseContext.GetSharedPreferences ("myPrefs", FileCreationMode.Private);
				settings.Edit ().Clear ().Commit ();
				DateTime dateWithoutTime = DateTime.Now;
				UpdateEvidence (Convert.ToString (dateWithoutTime));
				Intent sendToLogIn = new Intent (this, typeof(LogInActivity));
				sendToLogIn.AddFlags (Intent.Flags);
				StartActivityForResult (sendToLogIn, 0);
				return true;
			}
			return base.OnOptionsItemSelected (item);
		}

		//dbHelper.GetConnectionForCompany (1) [0].ConnectionName,
		//dbHelper.GetConnectionsList ("Connection11", 2) [0].ConnectionName
		//
		//dbHelper.RegisterUser ("krenki", "krenki", "krenki", "krenki"),
		//dbHelper.Login ("krenki11", "krenki")
		//
		//	dbHelper.UpdateUser (1, "krenare", "krenare", "krenare", "krenare");
		//dbHelper.DeleteUser (2);
		//dbHelper.SelectUsers () + dbHelper.GetUserDataForId (1).FirstName
		// dbHelper.RegisterUser ("betim", "drenica", "betim11", "krenare123") +
		//dbHelper.GetConnections ("Connection1", 1)

		string nameWifi;
		int company;
		WifiManager wifi;

		void UpdateEvidence (string mydate)
		{
			List<Evidence> evidence = dbHelper.GetLastEvidence (Convert.ToInt32 (userId));
			if (evidence != null) {
				List<Connections> conn = dbHelper.GetCompanyForConnection (Convert.ToInt32 (evidence [0].ConnectionId));
				if (conn [0].ConnectionName.Equals (GetSharedPreferenceName ())) {
					dbHelper.UpdateEvidence (evidence [0].EvidenceId, evidence [0].CheckInTime, mydate, evidence [0].ConnectionId, evidence [0].CompanyId, evidence [0].UserId);
					DeleteSharedPreference ();
					return;
				}
			}
		}

		public void AddEvidence (Context context, string mydate)
		{
			wifi = (WifiManager)GetSystemService (Context.WifiService);
			WifiInfo info = wifi.ConnectionInfo;
			if (info.SSID != null) {
				List<Connections> tData = dbHelper.GetAllConnectionsList ();
				if (tData.Count > 0) {
					string fooString1 = " ";
					string fooString2 = info.SSID;
					foreach (Connections data1 in tData) {
						nameWifi = data1.ConnectionName;
						fooString1 = nameWifi;
						List<Connections> companyData = dbHelper.GetCompanyForConnection (Convert.ToInt32 (data1.ConnectionId));
						company = Convert.ToInt32 (companyData [0].CompanyId);
						if (fooString1.Equals (fooString2)) {
							List<Company> userCompany = dbHelper.GetCompanyForUserList (Convert.ToInt32 (userId));
							foreach (Company com in userCompany) {
								if (com.CompanyId == company) {
									ShtoEvidence (mydate, company, Convert.ToInt32 (companyData [0].ConnectionId));
									return;
								}
							}
						}
					}
				}
			} else {
				UpdateEvidence (mydate);
			}
		}

		List<Connections> connName;

		private void ShtoEvidence (string mydate, int company, int connectionId)
		{
			if (company != 0) {
				connName = dbHelper.GetCompanyForConnection (connectionId);
				List<Evidence> dataForupdate = dbHelper.GetEvidenceUserCompanyList (Convert.ToInt32 (userId), company);
				if (dataForupdate != null && dataForupdate.Count > 0) {
					foreach (Evidence data1 in dataForupdate) {
						if (data1.CheckOutTime.Equals ("") && GetSharedPreferenceName ().Equals (connName) && GetSharedPreferenceStatus () == false) {
							dbHelper.UpdateEvidence (data1.EvidenceId, data1.CheckInTime, mydate, connectionId, company, Convert.ToInt32 (userId));
							DeleteSharedPreference ();
							return;
						} else if (data1.CheckOutTime.Equals ("") && GetSharedPreferenceName () != (Convert.ToString (connName [0].ConnectionName)) && GetSharedPreferenceStatus ().Equals (true)) {
							dbHelper.RegisterEvidence (mydate, "", connectionId, company, Convert.ToInt32 (userId));
							DeleteSharedPreference ();
							SaveSharePreference (Convert.ToString (connName [0].ConnectionName), true);
							return;

						}
					}

				} else {
					dbHelper.RegisterEvidence (mydate, "", connectionId, company, Convert.ToInt32 (userId));
					SaveSharePreference (Convert.ToString (connName [0].ConnectionName), true);
					return;
				}

			}
		}

		private void SaveSharePreference (string connnectionName, bool status)
		{
			ISharedPreferences myPrefs = GetSharedPreferences ("ConnectionData", FileCreationMode.WorldWriteable);
			ISharedPreferencesEditor prefsEditor;
			prefsEditor = myPrefs.Edit ();
			prefsEditor.PutString ("ConnectionName", connnectionName);
			prefsEditor.PutBoolean ("ConnectionStatus", status);
			prefsEditor.Commit ();

		}

		string sharedName;
		bool sharedStatus = false;

		private string GetSharedPreferenceName ()
		{
			ISharedPreferences myPrefs;
			myPrefs = GetSharedPreferences ("ConnectionData", FileCreationMode.WorldWriteable);
			string StoredValueName = myPrefs.GetString ("ConnectionName", "");
			if (!StoredValueName.Equals ("") && StoredValueName != null && StoredValueName != "") {
				sharedName = myPrefs.GetString ("ConnectionName", "");
				return sharedName;
			} else {
				return "";
			}
		}

		private bool GetSharedPreferenceStatus ()
		{
			ISharedPreferences myPrefs;
			myPrefs = GetSharedPreferences ("ConnectionData", FileCreationMode.WorldWriteable);
			bool StoredValueStatus = myPrefs.GetBoolean ("ConnectionStatus", false);
			if (!StoredValueStatus.Equals (true) && StoredValueStatus != null) {
				sharedStatus = myPrefs.GetBoolean ("ConnectionStatus", true);
				return true;
			} else {
				return false;
			}
		}

		private  void DeleteSharedPreference ()
		{
			settings = ApplicationContext.GetSharedPreferences ("ConnectionData", FileCreationMode.Private);
			settings.Edit ().Clear ().Commit ();

		}

	}
}



