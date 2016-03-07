
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
	[Activity (Label = "Company ", Theme = "@android:style/Theme.Holo.Light")]			
	public class CompanyActivity : Activity
	{
		DatabaseHelper data;
		String userId;
		EditText company;
		ListView listCompany;
		List<String> stockList = new List<String> ();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.activity_company);

			data = new DatabaseHelper (ApplicationContext);
			userId = Intent.GetStringExtra ("UserId");
			company = FindViewById <EditText> (Resource.Id.edtCompany);
			listCompany = FindViewById <ListView> (Resource.Id.lvCompany);
		
			Button brnAddCompany = FindViewById <Button> (Resource.Id.btnAddCompany);
			brnAddCompany.Click += delegate {
				String wcompany = company.Text;
				if (!wcompany.Equals ("")) {
					stockList.Add (wcompany);
					MbusheListen ();
					company.Text = "";
				} else {
					Toast.MakeText (BaseContext, "Shënoni kompaninë ", ToastLength.Long).Show ();
					return;
				}
			};

			Button btnSaveCompanies = FindViewById<Button> (Resource.Id.btnSaveCompanies);

			btnSaveCompanies.Click += delegate {
				try {
					if (stockList.Count > 0) {
						for (int j = 0; j < stockList.Count; j++) {
							String name = stockList [j];
							if (!CheckNameInList (name)) {
								data.RegisterCompany (Convert.ToInt32 (userId), name);
							}
						}

						Toast.MakeText (BaseContext, "Të dhënat u ruajtën me sukses! ", ToastLength.Long).Show ();
					}
				} catch (Exception) {
				}

				Intent conn = new Intent (ApplicationContext, typeof(ConnectionsActivity));
				conn.PutExtra ("UserId", (string)userId);
				conn.PutExtra ("LogIn", "LogIn");
				StartActivity (conn);


			};             
      
			listCompany.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				string selectedFromList = listCompany.GetItemAtPosition (e.Position).ToString ();
				AlertDialog.Builder answer = new AlertDialog.Builder (this);
				AlertDialog alert = answer.Create ();
				alert.SetTitle ("Question?");
				alert.SetIcon (Resource.Drawable.Icon);
				alert.SetMessage ("Do you want to delete this company !! ");
				alert.SetButton ("Yes", (s, ev) => {
					try {
						stockList.Remove (selectedFromList);
						List<Company> companies = data.GetCompanyId (selectedFromList);
						data.deleteCompanyForCompanyId (companies [0].CompanyId);
						data.deleteConnectionByCompanyId (companies [0].CompanyId);
						List<string> lista = new List<string> ();
						for (int i = 0; i < stockList.Count; i++) {
							stockList.RemoveAt (i);
						}
						GetCompany ();
						
					} catch (Exception) {
						Toast.MakeText (BaseContext, "Gabim në të dhëna ", ToastLength.Long).Show ();
						return;
					}
				});
				alert.SetButton2 ("No", (s, ev) => {
					return;
				});
				alert.Show ();
			};

		}

		public void GetCompany ()
		{
			stockList.Clear ();
			List<Company> companies = data.GetCompanyForUserList (Convert.ToInt32 (userId));
			if (companies.Count () > 0) {

				for (int i = 0; i < companies.Count (); i++) {
					stockList.Add (companies [i].CompanyName);
				}
			}
			MbusheListen ();
		}

		// Mbushet lista me te dhenat e shtuara
		private void MbusheListen ()
		{
			IListAdapter lista = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1, stockList);
			listCompany.Adapter = lista;
		}

		bool result;

		private bool CheckNameInList (String name)
		{
			List<Company> lista = data.GetCompanyForUserList (Convert.ToInt32 (userId));

			if (lista != null) {
				for (int i = 0; i < lista.Count (); i++) {

					if ((lista [i].CompanyName.Equals (name))) {
						return result = true;
					} else {
						result = false;
					}
				}
				return result;
			}
			return false;
		}

		protected override void OnResume ()
		{
			base.OnResume (); 
			GetCompany ();

		}
	}
}

