  
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

namespace TimeTrackerMobile
{
	[Activity (Label = "Connections ", Theme = "@android:style/Theme.Holo.Light")]			
	public class ConnectionsActivity : Activity
	{
		Spinner spinner;
		ListView lsWifil;
		string userId;
		DatabaseHelper data;
		string[] stocList;
		int companyId;
		String kompania_e_zgjedhur;

		WifiManager mainWifiObj;
		List<String> wifis;
		String SSID = "";
		ArrayAdapter<String> ad;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_add_wi_fi);

			userId = Intent.GetStringExtra ("UserId");

			data = new DatabaseHelper (ApplicationContext);

			spinner = FindViewById <Spinner> (Resource.Id.spCompanyWifi);
			lsWifil = FindViewById<ListView> (Resource.Id.listView);

			GetCompany (Convert.ToInt32 (userId));
			spinner.ItemSelected += spinner_ItemSelected;

			ImageButton refresh = FindViewById <ImageButton> (Resource.Id.refreshNetwork);

			refresh.Click += delegate {

				GetWiFiData ();

			};  


			GetWiFiData ();
		
			lsWifil.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs e) {   
				ListView lista = (ListView)sender;
				SSID = lsWifil.GetItemAtPosition (e.Position).ToString ();
				String mesazhi = "A jeni të sigurtë që dëshironi ta shtoni këtë wifi " + SSID + " për kompaninë e zgjedhur !! ";
				AlertDialog.Builder answer = new AlertDialog.Builder (this);
				AlertDialog alert = answer.Create ();
				alert.SetTitle ("Question?");
				alert.SetIcon (Resource.Drawable.Icon);
				alert.SetMessage (mesazhi);
				alert.SetButton ("Yes", (s, ev) => {

					if (!CheckSSID (SSID)) {
						try {
							RuajTeDhenat ();
							return;
						} catch (Exception ex) {
							Toast.MakeText (ApplicationContext, "Gabim në të dhëna !!", ToastLength.Long).Show ();
							return;
						}

					} else {
						Toast.MakeText (ApplicationContext, "Ky wifi ekziston.  Zgjedh një tjetër !!", ToastLength.Long).Show ();
						return;
					} 


				});
				alert.SetButton2 ("No", (s, ev) => {
					return;
				});
				alert.Show ();
			};

		}

		private void GetWiFiData ()
		{
			wifis = new List<String> ();
			mainWifiObj = (WifiManager)GetSystemService (Context.WifiService);

			IList<ScanResult> scanwifinetworks = mainWifiObj.ScanResults;
			if (scanwifinetworks != null) {
				foreach (ScanResult wifinetwork in scanwifinetworks) {
					wifis.Add (wifinetwork.Ssid.ToString ());
				}
			}
			ad = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1, wifis);
			lsWifil.Adapter = ad;
		}

		public void GetCompany (int userId)
		{
			List<Company> getCompany = data.GetCompanyForUserList (userId);
			if (getCompany.Count > 0) {
				stocList = new String[getCompany.Count];

				for (int i = 0; i < getCompany.Count; i++) {
					stocList [i] = (getCompany [i].CompanyName);
				}
				ISpinnerAdapter lista = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleDropDownItem1Line, stocList);
				spinner.Adapter = lista;

			}
		}

		private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;

			kompania_e_zgjedhur = spinner.GetItemAtPosition (e.Position).ToString ();
			List<Company> CcompanyId = data.GetCompanyId (kompania_e_zgjedhur);
			companyId = Convert.ToInt32 (CcompanyId [0].CompanyId);
			GetWiFiData ();
		}

		private void refresh_SetOnClickListener ()
		{
			GetWiFiData ();
		}

		Boolean result = false;

		private bool CheckSSID (String SSID)
		{
			List<Connections> connections = data.GetAllConnectionsList ();
			if (connections != null) {
				for (int i = 0; i < connections.Count (); i++) {
					if ((connections [i].ConnectionName.Equals (SSID))) {
						return result = true;
					} else {
						result = false;
					}
				}
				return result;
			}
			return result;
		}

		private bool CheckSSID1 (String SSID)
		{
			List<Connections> connections = data.GetAllConnectionsList ();
			if (connections != null) {
				for (int i = 0; i < connections.Count (); i++) {
					if ((connections [i].ConnectionName.Equals (SSID))) {
						return result = true;
					} else {
						result = false;
					}
				}
				return result;
			}
			return result;
		}


		public void RuajTeDhenat ()
		{
			data.RegisterConnection (companyId, SSID);
			Toast.MakeText (ApplicationContext, "Të dhënat u ruajtën. ", ToastLength.Short).Show ();
			return;
		}



	}
}
