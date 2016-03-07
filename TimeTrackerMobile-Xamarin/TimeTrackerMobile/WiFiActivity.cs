
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
using Android.Net.Wifi;

using System.Threading;

namespace TimeTrackerMobile
{
	[Activity (Label = "WiFiActivity")]			
	public class WiFiActivity : Activity
	{
		string folder;
		Spinner spinner;
		List<String> items;
		String _username;

		WifiManager mainWifiObj;
		ListView list;
		List<String> wifis;
		String SSID = "";
		ArrayAdapter<String> ad;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_add_wi_fi);

			folder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			dbb = new SQLiteConnection (System.IO.Path.Combine (folder, "TimeTracker.db"));

			list = (ListView)FindViewById (Resource.Id.listViewWifi);
			wifis = new List<String> ();

			_username = Intent.GetStringExtra ("UserName") ?? "Data not available";
			GetCompany ();

			mainWifiObj = (WifiManager)GetSystemService (Context.WifiService);
			IList<ScanResult> scanwifinetworks = mainWifiObj.ScanResults;
			foreach (ScanResult wifinetwork in scanwifinetworks) {
				wifis.Add (wifinetwork.Ssid.ToString ());
			}
		
			GetWiFiData ();
		}

		public void GetCompany ()
		{
			items = new List<String> ();
			var table = dbb.Table<PersonalData> ();
			var apple = from s in table
			            where s.Username.Equals (_username)
			            select s;
			spinner = (Spinner)FindViewById (Resource.Id.spCompanywifi);

			if (!table.Equals (null)) {
				foreach (var item in apple) {
					items.Add (item.Company);
				}

				var adapter = new ArrayAdapter<string> (this, Android.Resource.Layout.SimpleSpinnerItem, items);
				adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
				spinner.Adapter = adapter;

			}
		
		}

		private void GetWiFiData ()
		{

			ad = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1, wifis);
			list.Adapter = ad;
			
			//	View view = super.getView(position, convertView, parent);
			//	TextView text = (TextView) view.findViewById(android.R.id.text1);
			//	text.setTextColor(Color.BLACK);
			//	return view;
		}
	}
}

