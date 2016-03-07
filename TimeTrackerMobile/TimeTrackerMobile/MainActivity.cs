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
	[Activity (Label = "Main ", Theme = "@android:style/Theme.Holo.Light")]
	public class MainActivity : Activity
	{
		DatabaseHelper dbHelper;
		TextView txt;
		string userId;
		string logIn;
		ListView lastEvidenceList;
		static int INTERVAL = 1000 * 30 * 1;
		string nameWifi;
		int company;
		WifiManager wifi;
		string sharedName;
		bool sharedStatus = false;
		List<string> stockList = new List<string> ();
		Handler handler = new Handler ();
		//	Runnable runnable;
		Action runnable;
		ISharedPreferences settings;
		List<Connections> connName;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_main);
			dbHelper = new DatabaseHelper (ApplicationContext);

			DateTime dateWithoutTime = DateTime.Now;
			txt = FindViewById<TextView> (Resource.Id.txtNameAndSurname);

			userId = Intent.GetStringExtra ("UserId");
			logIn = Intent.GetStringExtra ("LogIn");
			lastEvidenceList = FindViewById <ListView> (Resource.Id.lsvLastrecords);
			string us = dbHelper.SelectUsers ();
			User user = dbHelper.GetUserDataForId (Convert.ToInt32 (userId));
			txt.Text = user.FirstName + "  " + user.LastName;
			runnable = () => AddEvidence (ApplicationContext, dateWithoutTime);
			//Action	runnable = new Action (true);
			//	runnable.Run ();
			//	runnable.BeginInvoke (() => AddEvidence (ApplicationContext, dateWithoutTime));
			runnable.Invoke ();
			GetLastEvidence ();
		
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

		public void GetLastEvidence ()
		{
			stockList.Clear ();

			List<Evidence> evidence = dbHelper.GetLastEvidence (Convert.ToInt32 (userId));
			if (evidence != null &&	evidence.Count != 0) {
				stockList.Add ("Hyrja : " + evidence [0].CheckInTime);
				if (evidence [0].CheckOutTime != Convert.ToDateTime (null)) {
					stockList.Add ("Dalja : " + evidence [0].CheckOutTime);

				}
			}

			IListAdapter lista = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleListItem1, stockList);
			lastEvidenceList.Adapter = lista;
			User user = dbHelper.GetUserDataForId (Convert.ToInt32 (userId));
			txt.Text = user.FirstName + "  " + user.LastName;
		}

		void txt_Click (object sender, EventArgs e)
		{
			Intent sendToWifi = new Intent (this, typeof(RegisterActivity));
			sendToWifi.PutExtra ("UserId", (userId));
			sendToWifi.PutExtra ("LogIn", logIn);
			StartActivityForResult (sendToWifi, 0);
		}

		public override void OnBackPressed ()
		{
			Intent intent = new Intent (Intent.ActionMain);
			intent.AddCategory ("android.intent.category.HOME");
			intent.SetFlags (ActivityFlags.NewTask);
			StartActivity (intent);
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Layout.settings, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			User user = dbHelper.GetUserDataForId (Convert.ToInt32 (userId));
			txt.Text = user.FirstName + "  " + user.LastName;
			GetLastEvidence ();
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			User user = dbHelper.GetUserDataForId (Convert.ToInt32 (userId));
			switch (item.ItemId) {
			case Resource.Id.action_reset:
				string mesazhi = "Nëse jeni të sigurtë që dëshironi ta ndryshoni fjalëkalimin. Shënoni fjalëlalimin e ri !! ";
				AlertDialog.Builder answer = new AlertDialog.Builder (this);
				AlertDialog alert = answer.Create ();
				alert.SetTitle ("reset password?");
				alert.SetIcon (Resource.Drawable.Icon);
				alert.SetMessage (mesazhi);
				EditText input = new EditText (this);
				alert.SetView (input);
				alert.SetButton ("OK", (s, ev) => {

					string password = input.Text.ToString ();
					long updateUser = dbHelper.UpdateUser (user.UserId, user.FirstName, user.LastName, user.Username, password);
					Toast.MakeText (ApplicationContext, "Fjalëkalimi është ndryshuar ", ToastLength.Long).Show ();
			

				});
				alert.SetButton2 ("Cancel", (s, ev) => {
					return;
				});
				alert.Show ();
				return true;			
			case Resource.Id.action_LogOut:
				settings = BaseContext.GetSharedPreferences ("myPrefs", FileCreationMode.Private);
				settings.Edit ().Clear ().Commit ();
				DateTime dateWithoutTime = DateTime.Now;
				UpdateEvidence ((dateWithoutTime));
				Intent sendToLogIn = new Intent (this, typeof(LogInActivity));
				sendToLogIn.AddFlags (ActivityFlags.ClearTop);
				StartActivityForResult (sendToLogIn, 0);
				return true;
			}
			return base.OnOptionsItemSelected (item);
		}

		public void AddEvidence (Context context, DateTime mydate)
		{ 
			mydate = DateTime.Now;

			handler.PostDelayed (runnable, INTERVAL);


			wifi = (WifiManager)GetSystemService (Context.WifiService);

			WifiInfo info = wifi.ConnectionInfo;
			if (info.SSID != null && !info.SSID.Equals ("") && info.LinkSpeed >= 0) {
				string rrjeti = info.SSID.Replace ("\"", ""); 
				if (!rrjeti.Equals (GetSharedPreferenceName ()) && GetSharedPreferenceName ().Equals ("")) {
					List<Connections> tData = dbHelper.GetAllConnectionsList ();
					if (tData.Count > 0) {
						string fooString1 = "";
						string fooString2 = rrjeti;
						foreach (Connections data1 in tData) {
							nameWifi = data1.ConnectionName;
							fooString1 = nameWifi;
							List<Connections> companyData = dbHelper.GetCompanyForConnection (Convert.ToInt32 (data1.ConnectionId));
							company = Convert.ToInt32 (companyData [0].CompanyId);
							if (fooString1.Equals (fooString2.ToString ())) {
								List<Company> userCompany = dbHelper.GetCompanyForUserList (Convert.ToInt32 (userId));
								foreach (Company com in userCompany) {
									if (com.CompanyId == company) {
										ShtoEvidence (mydate, company, Convert.ToInt32 (companyData [0].ConnectionId));
										GetLastEvidence ();
										return;
									}
								}
							}
						}
					}
				} else if (!rrjeti.Equals (GetSharedPreferenceName ())) {
					UpdateEvidence (mydate);

				}

			} else {
				UpdateEvidence (mydate);
			}
		}

	
		private void UpdateEvidence (DateTime mydate)
		{
			List<Evidence> evidence = dbHelper.GetLastEvidence (Convert.ToInt32 (userId));
			if (evidence != null) {
				List<Connections> conn = dbHelper.GetCompanyForConnection (Convert.ToInt32 (evidence [0].ConnectionId));
				if (conn != null) {
					if (conn [0].ConnectionName.Equals (GetSharedPreferenceName ())) {
						dbHelper.UpdateEvidence (evidence [0].EvidenceId, evidence [0].CheckInTime, mydate, evidence [0].ConnectionId, evidence [0].CompanyId, evidence [0].UserId);
						Toast.MakeText (BaseContext, "Evidencimi ka perfunduar ! ", ToastLength.Long).Show ();
						DeleteSharedPreference ();
						GetLastEvidence ();
						return;
					}
					// ketu fshihen te dhenat ne shared preference sepse ne raste kur nuk ka conneksion as te dhena nuk ka mirepo evidenca ka mbetur disi

				} else if (conn == null && evidence [0].CheckOutTime.Equals ("")) {
					dbHelper.UpdateEvidence (evidence [0].EvidenceId, evidence [0].CheckInTime, mydate, evidence [0].ConnectionId, evidence [0].CompanyId, evidence [0].UserId);
					DeleteSharedPreference ();
					GetLastEvidence ();
					return;
				}

			}

		}

		private void ShtoEvidence (DateTime mydate, int company, int connectionId)
		{
			if (company != 0) {
			
				connName = dbHelper.GetCompanyForConnection (connectionId);
				List<Evidence> dataForupdate = dbHelper.GetEvidenceUserCompanyList (Convert.ToInt32 (userId), company);
				if (dataForupdate != null && dataForupdate.Count > 0) {
					foreach (Evidence data1 in dataForupdate) {
						if (data1.CheckOutTime.Equals (Convert.ToDateTime (null)) && GetSharedPreferenceName ().Equals (connName) && GetSharedPreferenceStatus ().Equals (false)) {
							dbHelper.UpdateEvidence (data1.EvidenceId, data1.CheckInTime, mydate, connectionId, company, Convert.ToInt32 (userId));
							DeleteSharedPreference ();
							GetLastEvidence ();
							return;
						} else if (data1.CheckOutTime.Equals (Convert.ToDateTime (null)) && GetSharedPreferenceName () != (Convert.ToString (connName [0].ConnectionName)) && GetSharedPreferenceStatus ().Equals (true)) {
							dbHelper.RegisterEvidence (mydate, Convert.ToDateTime (null), connectionId, company, Convert.ToInt32 (userId));
							DeleteSharedPreference ();
							SaveSharePreference (Convert.ToString (connName [0].ConnectionName), true);
							GetLastEvidence ();
							Toast.MakeText (ApplicationContext, "Evidencimi ka filluar !!", ToastLength.Long).Show ();

							return;

						} else if (!data1.CheckOutTime.Equals (Convert.ToDateTime (null)) && GetSharedPreferenceName () != (Convert.ToString (connName [0].ConnectionName)) && GetSharedPreferenceStatus ().Equals (true)) {
							dbHelper.RegisterEvidence (mydate, Convert.ToDateTime (null), connectionId, company, Convert.ToInt32 (userId));
							DeleteSharedPreference ();
							SaveSharePreference (Convert.ToString (connName [0].ConnectionName), true);
							GetLastEvidence ();
							Toast.MakeText (ApplicationContext, "Evidencimi ka filluar!! ", ToastLength.Long).Show ();
							return;

						}
					}
				} else {
					dbHelper.RegisterEvidence (mydate, Convert.ToDateTime (null), connectionId, company, Convert.ToInt32 (userId));
					SaveSharePreference (Convert.ToString (connName [0].ConnectionName), true);
					GetLastEvidence ();
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
			if (!StoredValueStatus.Equals (true)) {
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



