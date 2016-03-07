
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
using SQLite;

namespace TimeTracker
{
	[Activity (Label = "PersonalDataActivity")]			
	public class PersonalDataActivity : Activity
	{
		EditText txtCompany;
		EditText name;
		EditText surname;
		List<String> items;

		ListView lv;
		String _username;
		String _login;
		LogInActivity l;
		string folder;
		SQLiteConnection dbb ;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.activity_personal_data);
			l = new LogInActivity();
			items = new List<string> ();
			 folder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			 dbb = new SQLiteConnection (System.IO.Path.Combine (folder, "TimeTracker.db"));

			_username = Intent.GetStringExtra ("UserName") ?? "Data not available";
			_login = Intent.GetStringExtra("Kycje");

			txtCompany = FindViewById<EditText> (Resource.Id.txtUsername);
			name = FindViewById<EditText>(Resource.Id.txtEmri);
			surname = FindViewById<EditText>(Resource.Id.txtMbiemri);
			lv = FindViewById<ListView>(Resource.Id.listView);

			if (_login != null)
			{
				var table = dbb.Table<PersonalData>();
				var apple = from s in table
						where s.Username.Equals(_username)
					select s;

				foreach (var s in apple)
				{
					name.Text = s.Name;
					surname.Text = s.Surname;
					items.Add(s.Company);

				}
				MbusheListen ();
			}
			Button button = FindViewById<Button>(Resource.Id.btnRuaj);
			Button btnShtoKompani = FindViewById<Button> (Resource.Id.btnShtoKompani);
			button.Click +=  HandleClick;
			btnShtoKompani.Click += btnShtoClick;
		}

		void HandleClick (object sender, EventArgs e)
		{
			int countt = ad.Count;
			if (name.Text.Equals ("") || surname.Text.Equals ("") || countt == 0)
			{
				l.DialogAlert ("Së paku një kompani dhe të dhënat tjera duhet të jenë të shënuara! ");
				return;
			} 
			else
			{
				if (lv != null) {
					int count = ad.Count;
					for (int i = 0; i < count; ++i)
					{
						dbb.CreateTable<PersonalData> ();
						var table = dbb.Table<PersonalData>();
						var apple = from s in table
							where s.Username.Equals(_username)
							select s;
						var person = new PersonalData();
						foreach (var item in apple)
						{
							person.ID = item.ID;
							person.Username = item.Username;
							person.Name = item.Name;
							person.Surname = item.Surname;
							person.Company = item.Company;
						}
					dbb.Delete(person);
				}
						for (int i = 0; i < count; ++i) 
						{	
							string item = ad.GetItem (i).ToString ();
							dbb.CreateTable<PersonalData> ();
							var person = new PersonalData { Name = name.Text, Surname = surname.Text, Company = item, Username = _username };
							dbb.Insert(person);
						}
					}
				} 
			 //	l.DialogAlert("Të dhënat u ruajtën me sukses");
			Intent sendToWifi = new Intent (this, typeof(WiFiActivity));
			sendToWifi.PutExtra ("UserName", _username);
			StartActivity (sendToWifi);
		}

		void btnShtoClick (object sender, EventArgs e)
		{
			string res = txtCompany.Text;
				if (res.Length > 0)
				{
				    items.Add(res);
					MbusheListen();
					txtCompany.Text = "";
				}
				else
				{
				//l.DialogAlert ("Shto kompaninë");
					return;
				}
		}

		ArrayAdapter<String> ad;
		private void MbusheListen()
		{ 
			ad = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
			lv.Adapter = ad;
		}
	}
}

