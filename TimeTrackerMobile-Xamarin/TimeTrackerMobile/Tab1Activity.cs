
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
	[Activity (Label = "Tab1Activity")]			
	public class Tab1Activity : Activity
	{
		String login;
		String logout;
		string userId;
		string companyId;
		DatabaseHelper data;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.tab_activity1);

			DateTime d = DateTime.Now;

			String dateWithoutTime = d.ToString ().Substring (0, 11);

			userId = Intent.GetStringExtra ("UserId");
			companyId = Intent.GetStringExtra ("CompanyId");

			String[] tDataList;
			List<String> lString;
			ListView lv = FindViewById<ListView> (Resource.Id.listToday);

			data = new DatabaseHelper (ApplicationContext);
			List<Evidence> tData = data.GetEvidenceUserCompanyList (Convert.ToInt32 (userId), Convert.ToInt32 (companyId));

			if (tData.Count > 0) {
				{
					lString = new List<String> ();
					for (int i = 0; i < tData.Count; i++) {
						if (!tData [i].CheckInTime.Equals ("")) {
							login = tData [i].CheckInTime.Substring (0, 11);
						}
						if (!tData [i].CheckOutTime.Equals ("")) {
							logout = tData [i].CheckOutTime.Substring (0, 11);
						}
						if (!login.Equals ("")) {
							if (login.Equals (dateWithoutTime)) {
								lString.Add ("Hyrja: " + tData [i].CheckInTime + " " + data.GetCompanyForConnection (tData [i].ConnectionId) [0].ConnectionName);
							}
						}
						if (!logout.Equals ("")) {
							if (logout.Equals (dateWithoutTime)) {
								lString.Add (logout = "Dalja: " + tData [i].CheckOutTime + " " + data.GetCompanyForConnection (tData [i].ConnectionId) [0].ConnectionName);
							}
						}
					}
					tDataList = new String[lString.Count ()];
					for (int i = 0; i < lString.Count (); i++) {
						tDataList [i] = lString [i].ToString ();
					}
					IListAdapter lista = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1, tDataList);
					lv.Adapter = lista;
				}
			}
		}

	}
}



