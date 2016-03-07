
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
	[Activity (Label = "Archive ", Theme = "@android:style/Theme.Holo.Light")]			
	public class ArchiveActivity : Activity
	{
		string userId;
		DatabaseHelper data;
		Spinner sppiner;
		List<String> stocListCompany = new List<String> ();
		Bundle sIS;
		String kompania;
		List<Company> companyList;
		int companyId;
		TabHost tabHost;
		LocalActivityManager mlam;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_archive);

			userId = Intent.GetStringExtra ("UserId");

			data = new DatabaseHelper (ApplicationContext);
			sppiner = FindViewById <Spinner> (Resource.Id.spCompanyArchive);
			GetCompany (Convert.ToInt32 (userId));
			InitializeTabs (bundle);
			sIS = bundle;

			sppiner.ItemSelected += delegate(object sender, AdapterView.ItemSelectedEventArgs e) {
				InitializeTabs (sIS);
				kompania = sppiner.GetItemAtPosition (e.Position).ToString ();
				companyList = data.GetCompanyId (kompania);
				companyId = companyList [0].CompanyId;
				List<Evidence> tData = data.GetEvidenceUserCompanyList (Convert.ToInt32 (userId), companyId);
				if (tData.Count <= 0) {
					tabHost.ClearAllTabs ();
					return;
				} else {
					tabHost.ClearAllTabs ();
					LoadTabs ();
				}
			};
		}

		private void LoadTabs ()
		{
			tabHost.Setup (mlam);
			TabHost.TabSpec spec;
			Intent intent;
			String vlera = Convert.ToString (companyId);

			intent = new Intent (this, typeof(Tab1Activity));
			intent.PutExtra ("UserId", userId);
			intent.PutExtra ("CompanyId", vlera);
			spec = tabHost.NewTabSpec ("Sot").SetIndicator ("Sot").SetContent (intent);
			tabHost.AddTab (spec);

			intent = new Intent (this, typeof(Tab2Activity));
			intent.PutExtra ("UserId", userId);
			intent.PutExtra ("CompanyId", vlera);
			spec = tabHost.NewTabSpec ("Të kaluarat").SetIndicator ("Të kaluarat").SetContent (intent);
			tabHost.AddTab (spec);



		}

		public void GetCompany (int userId)
		{
			List<Company> getCompany = data.GetCompanyForUserList (userId);
			if (getCompany.Count > 0) {
				for (int i = 0; i < getCompany.Count; i++) {
					stocListCompany.Add (getCompany [i].CompanyName);
				}
				ISpinnerAdapter lista = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleDropDownItem1Line, stocListCompany);
				sppiner.Adapter = lista;


			}

		}

		private void InitializeTabs (Bundle savedInstanceState)
		{
			mlam = new LocalActivityManager (this, true);
			tabHost = FindViewById <TabHost> (Resource.Id.tabHost);
			mlam.DispatchCreate (savedInstanceState);
			tabHost.Setup (mlam);
		}
	}

}

