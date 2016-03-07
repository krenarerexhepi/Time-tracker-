
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
	[Activity (Label = "RegisterActivity")]			
	public class RegisterActivity : Activity
	{
		EditText uname;
		EditText passwd;
		EditText firstName;
		EditText lastName;
		EditText company;
		string userId;
		string logIn;
		User logInuserData;
		Company companyForuser;
		DatabaseHelper data;
		int lonInUserDataUserId;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.activity_register);
			// Create your application here

			userId = Intent.GetStringExtra ("UserId");
			logIn = Intent.GetStringExtra ("LogIn");


			uname = FindViewById<EditText> (Resource.Id.edtUsername);
			passwd = FindViewById<EditText> (Resource.Id.edtPassword);
			firstName = FindViewById<EditText> (Resource.Id.edtFirstName);
			lastName = FindViewById<EditText> (Resource.Id.edtLastName);
			company = FindViewById<EditText> (Resource.Id.edtCompany);

			data = new DatabaseHelper (ApplicationContext);

			if (logIn.Equals ("LogIn")) {
				logInuserData = data.GetUserDataForId (Convert.ToInt32 (userId));
				lonInUserDataUserId = Convert.ToInt32 (logInuserData.UserId);
				companyForuser = data.GetCompanyForUser (lonInUserDataUserId);

				if (companyForuser != null) {
					company.Text = (companyForuser.CompanyName);
				}

				firstName.Text = (logInuserData.FirstName);
				lastName.Text = (logInuserData.LastName);
				uname.Text = (logInuserData.Username);
				passwd.Text = (logInuserData.Password);

			}

			Button btnSave = FindViewById<Button> (Resource.Id.btnSaveData);
			btnSave.Click += delegate {
				User id = new User ();
				long id1;
				if (uname.Text.Equals ("") || passwd.Text.Equals ("") || firstName.Text.Equals ("") || lastName.Text.Equals ("") || company.Text.Equals ("")) {
					Toast.MakeText (BaseContext, "Plotësoni të dhënat! ", ToastLength.Long).Show ();
					return;
				} else {
					try {
						if (logIn.Equals ("LogIn")) {
							id1 = data.UpdateUser (lonInUserDataUserId, firstName.Text, lastName.Text, uname.Text, passwd.Text);
							data.UpdateCompany (companyForuser.CompanyId, lonInUserDataUserId, company.Text);
						} else {   
							id = data.GetUserDataForUserName (uname.Text);
						
						}

						if (id.UserId == 0) {
							data.RegisterUser (firstName.Text, lastName.Text, uname.Text, passwd.Text);
							id = data.GetUserDataForUserName (uname.Text);
							data.RegisterCompany (id.UserId, company.Text);

							Toast.MakeText (BaseContext, "Te dhenat u ruajten! ", ToastLength.Long).Show ();

							Button save = FindViewById<Button> (Resource.Id.btnSaveData);
							save.Enabled = (false);

							Intent sentToPersonalData = new Intent (BaseContext, typeof(CompanyActivity));
							sentToPersonalData.PutExtra ("UserId", Convert.ToString (id.UserId));
							StartActivity (sentToPersonalData);
						} else {
							Toast.MakeText (BaseContext, "Username " + uname.Text + " ekziston. Shenoni nje tjeter !!", ToastLength.Long).Show ();
							return;
						}

					} catch (Exception) {

						Toast.MakeText (BaseContext, "Gabim ne te dhena", ToastLength.Long).Show ();

					}
					;
				}

			};
		}

	}
}


