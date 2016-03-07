
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TimeTrackerMobile
{
	[Activity (Label = "LogInActivity", MainLauncher = true, Icon = "@drawable/icon")]
	public class LogInActivity : Activity
	{
		string sharedId;
		EditText uname;
		EditText passwd;
		DatabaseHelper data;
		String username;
		string useri;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			if (GetSharedPreference ()) {
				Intent sentToMain = new Intent (BaseContext, typeof(MainActivity));
				sentToMain.PutExtra ("UserId", sharedId);
				sentToMain.PutExtra ("LogIn", "LogIn");
				StartActivity (sentToMain);

			} else {
				SetContentView (Resource.Layout.activity_login);

				uname = FindViewById <EditText> (Resource.Id.edtUsername);
				passwd = FindViewById <EditText> (Resource.Id.edtPassword);
				data = new DatabaseHelper (ApplicationContext);
				username = uname.Text;

				Button btnSign = FindViewById<Button> (Resource.Id.btnSignUp);

				btnSign.Click += delegate {
					Intent sentToPersonalData = new Intent (BaseContext, typeof(RegisterActivity));
					sentToPersonalData.PutExtra ("LogIn", "");
					StartActivity (sentToPersonalData);
				};

				Button btnLogIn = FindViewById<Button> (Resource.Id.btnLogIn);
				btnLogIn.Click += delegate {
					if (uname.Text.Equals ("") || passwd.Text.Equals ("")) {
						Toast.MakeText (BaseContext, "Plotësoni të dhënat! ", ToastLength.Long).Show ();
						return;
					} else {
						string us = data.SelectUsers ();
						User user = data.Login (uname.Text, passwd.Text);
						if (user != null) {
							useri = Convert.ToString (user.UserId); 
							Intent sentToPersonalData = new Intent (BaseContext, typeof(MainActivity));
							sentToPersonalData.PutExtra ("LogIn", "LogIn");
							sentToPersonalData.PutExtra ("UserId", useri);
							sharedId = (useri);
							StartActivity (sentToPersonalData);
							SaveSharePreference (user.UserId);
						} else {
							Toast.MakeText (BaseContext, "Ky user nuk ekziston! ", ToastLength.Long).Show ();

						}
					}
				};
			
			}
		}

		private void SaveSharePreference (long userId)
		{
			ISharedPreferences myPrefs = GetSharedPreferences ("myPrefs", FileCreationMode.WorldWriteable);
			ISharedPreferencesEditor prefsEditor;
			prefsEditor = myPrefs.Edit ();
			prefsEditor.PutString ("STOREDVALUE", useri);
			prefsEditor.Commit ();
		}

		private bool GetSharedPreference ()
		{
			ISharedPreferences myPrefs;
			myPrefs = GetSharedPreferences ("myPrefs", FileCreationMode.WorldWriteable);
			String StoredValue = myPrefs.GetString ("STOREDVALUE", "");
			if (!StoredValue.Equals ("") && StoredValue != null && StoredValue != "") {
				sharedId = myPrefs.GetString ("STOREDVALUE", "");
				return true;
			} else {
				return false;
			}
		}

	}
}


