
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
	[Activity (Label = "Tab2Activity", Theme = "@android:style/Theme.Holo.Light")]			
	public class Tab2Activity : Activity
	{
		string userId;
		string companyId;
		string login;
		string logout;
		DatabaseHelper data;
		String[] tDataList;
		List<String> lString;


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.tab_activity1);


			DateTime d = DateTime.Now;
			String dateWithoutTime = d.ToString ().Substring (0, 11);

			userId = Intent.GetStringExtra ("UserId");
			companyId = Intent.GetStringExtra ("CompanyId");

			ListView lv = FindViewById<ListView> (Resource.Id.listToday);

			data = new DatabaseHelper (ApplicationContext);
			List<Evidence> tData = data.GetEvidenceUserCompanyList (Convert.ToInt32 (userId), Convert.ToInt32 (companyId));

			if (tData.Count > 0) {
				{
					string emri = "";
					lString = new List<String> ();
					for (int i = 0; i < tData.Count; i++) {
						List <Connections> connection = data.GetCompanyForConnection (tData [i].ConnectionId);
						if (connection != null && connection.Count > 0) {
							emri = connection [0].ConnectionName;
						}

						if (!tData [i].CheckInTime.Equals ("")) {
							login = tData [i].CheckInTime.ToString ().Substring (0, 11);
						}
						if (!tData [i].CheckOutTime.Equals ("")) {
							logout = tData [i].CheckOutTime.ToString ().Substring (0, 11);
						}
						if (!login.Equals ("")) {
							if (!login.Equals (dateWithoutTime)) {
								lString.Add ("Hyrja: " + tData [i].CheckInTime + " " + emri);
							}
						}
						if (!logout.Equals ("01/01/0001 ")) {
							if (!logout.Equals (dateWithoutTime)) {
								lString.Add (logout = "Dalja: " + tData [i].CheckOutTime + " " + emri);
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


