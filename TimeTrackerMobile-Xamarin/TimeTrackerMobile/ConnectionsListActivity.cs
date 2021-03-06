﻿
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
	[Activity (Label = "ConnectionsListActivity")]			
	public class ConnectionsListActivity : Activity
	{
		DatabaseHelper data;
		String userId;
		ListView listWifi;
		Spinner spCompany;
		String kompania_e_zgjedhur;
		List<String> stockList = new List<String> ();
		List<String> stocListCompany = new List<String> ();
		int companyId;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_wi_fi);

			data = new DatabaseHelper (ApplicationContext);
			userId = Intent.GetStringExtra ("UserId");
			spCompany = FindViewById <Spinner> (Resource.Id.spCompanywifiadd);
			listWifi = FindViewById <ListView> (Resource.Id.lvWifiData);

			Button btnAddMoreWifi = FindViewById<Button> (Resource.Id.btnAddMoreWifi);

			btnAddMoreWifi.Click += delegate {

				Intent conn = new Intent (ApplicationContext, typeof(ConnectionsActivity));
				conn.PutExtra ("UserId", (string)userId);
				StartActivity (conn);


			};             

			listWifi.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				string selectedFromList = listWifi.GetItemAtPosition (e.Position).ToString ();
				AlertDialog.Builder answer = new AlertDialog.Builder (this);
				AlertDialog alert = answer.Create ();
				alert.SetTitle ("Question?");
				alert.SetIcon (Resource.Drawable.Icon);
				alert.SetMessage ("Do you want to delete this wifi !! ");
				alert.SetButton ("Yes", (s, ev) => {
					try {
						stockList.Remove (selectedFromList);
						List<Connections> connns = data.GetConnectionsList (selectedFromList, companyId);
						data.deleteConnectionById (Convert.ToInt32 (connns [0].ConnectionId));
						List<string> lista = new List<string> ();
						for (int i = 0; i < stockList.Count; i++) {
							stockList.RemoveAt (i);
						}
						GetConnections ();

					} catch (Exception ex) {
						Toast.MakeText (BaseContext, ex + "Gabim në të dhëna ", ToastLength.Long).Show ();
						return;
					}
				});
				alert.SetButton2 ("No", (s, ev) => {
					return;
				});
				alert.Show ();
			};

			spCompany.ItemSelected += spinner_ItemSelected;

			GetCompany (Convert.ToInt32 (userId));

			GetConnections ();


		}

		public void GetCompany (int userId)
		{
			List<Company> getCompany = data.GetCompanyForUserList (userId);
			if (getCompany.Count > 0) {
				for (int i = 0; i < getCompany.Count; i++) {
					stocListCompany.Add (getCompany [i].CompanyName);
					//				companyId = Convert.ToInt32 (getCompany [0].CompanyId);
				}
				ISpinnerAdapter lista = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleDropDownItem1Line, stocListCompany);
				spCompany.Adapter = lista;


			}
		}

		public void GetConnections ()
		{
			for (int i = 0; i < stockList.Count (); i++) {
				stockList.RemoveAt (i);
			}

			List<Connections> conn = data.GetConnectionForCompany (companyId);
			if (conn.Count () > 0) {

				for (int i = 0; i < conn.Count (); i++) {
					stockList.Add (conn [i].ConnectionName);
				}
			}
			MbusheListen ();
		}

		// Mbushet lista me te dhenat e shtuara
		//StringArrayAdapter ad;
		private void MbusheListen ()
		{
			IListAdapter lista = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1, stockList);
			listWifi.Adapter = lista;
		}

		private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;

			kompania_e_zgjedhur = spinner.GetItemAtPosition (e.Position).ToString ();
			List<Company> CcompanyId = data.GetCompanyId (kompania_e_zgjedhur);
			companyId = Convert.ToInt32 (CcompanyId [0].CompanyId);
			GetConnections ();
		}
	
	}
}


