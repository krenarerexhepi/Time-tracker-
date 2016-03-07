
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
	[Activity (Label = "SettingsActivity")]			
	public class SettingsActivity : Activity
	{
		string folder;
		SQLiteConnection dbb;
		Spinner spinner;
		List<String> items;
		String _username;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_settings);

			folder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			dbb = new SQLiteConnection (System.IO.Path.Combine (folder, "TimeTracker.db"));

			_username = Intent.GetStringExtra ("UserName") ?? "Data not available";
			GetCompany ();

		}

		public void GetCompany()
		{
			items=new List<String>();
			var table = dbb.Table<PersonalData>();
			var apple = from s in table
					where s.Username.Equals(_username)
				select s;
			spinner = (Spinner) FindViewById(Resource.Id.spCompanySettings);

			if (!table.Equals(null)) {
				foreach (var item in apple) {
					items.Add (item.Company);
				}

				var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, items);
				adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
				spinner.Adapter = adapter;

			}
		}
	}
}

