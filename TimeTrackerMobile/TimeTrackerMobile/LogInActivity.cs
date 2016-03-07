
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
	[Activity (Label = "Time Tracker", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Holo.Light")]
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
					try {
						if (uname.Text.Equals ("") || passwd.Text.Equals ("")) {
							Toast.MakeText (BaseContext, "Plotësoni të dhënat! ", ToastLength.Long).Show ();
							return;
						} else {
							User userExist = data.GetUserDataForUserName (uname.Text);
							User user = data.Login (uname.Text, passwd.Text);
							if (!user.UserId.Equals (0)) {
								useri = Convert.ToString (user.UserId); 
								Intent sentToPersonalData = new Intent (BaseContext, typeof(MainActivity));
								sentToPersonalData.PutExtra ("LogIn", "LogIn");
								sentToPersonalData.PutExtra ("UserId", useri);
								sharedId = (useri);
								StartActivity (sentToPersonalData);
								SaveSharePreference (user.UserId);
							} else if (userExist.UserId.Equals (0)) {
								Toast.MakeText (BaseContext, "Ky user nuk ekziston! ", ToastLength.Long).Show ();

							} else {
								Toast.MakeText (BaseContext, "Passwordi gabim! ", ToastLength.Long).Show ();

							}
						}
					} catch (Exception) {
						Toast.MakeText (BaseContext, "Gabim në të dhëna! ", ToastLength.Long).Show ();
					}
				};

				CheckBox checkbox = FindViewById<CheckBox> (Resource.Id.checkbox);

				checkbox.Click += (o, e) => {
					if (checkbox.Checked) {
						string mesazhi = " Shënoni username dhe fjalëkalimin e ri !! ";
						AlertDialog.Builder answer = new AlertDialog.Builder (this);
						AlertDialog alert = answer.Create ();
						alert.SetTitle ("Reset password?");
						alert.SetMessage (mesazhi);
						EditText input = new EditText (this);
						EditText input2 = new EditText (this);
						LinearLayout ll = new LinearLayout (this);
						ll.Orientation = Orientation.Vertical;
						ll.AddView (input);
						ll.AddView (input2);
						alert.SetView (ll);
						alert.SetButton ("Ok", (s, ev) => {

							string password = input2.Text.ToString ();
							string usernametext = input.Text.ToString ();

							User user = data.GetUserDataForUserName (usernametext);
							if (user.UserId != 0) {
								data.UpdateUser (user.UserId, user.FirstName, user.LastName, usernametext, password);
								Toast.MakeText (ApplicationContext, "Fjalëkalimi është ndryshuar ", ToastLength.Long).Show ();
							} else {
								Toast.MakeText (ApplicationContext, "Gabim në username ", ToastLength.Long).Show ();
							}

						});
						alert.SetButton2 ("Cancel", (s, ev) => {
							return;
						});
						alert.Show ();

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


