
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TimeTrackerMobile
{
	public class Data : View
	{  
	
			DatabaseHandler db = new DatabaseHandler();



		//users
		public long RegisterUser(String name, String surname, String username, String password) {
			ContentValues values = new ContentValues();
			values.Put("FirstName", name);
			values.Put("LastName", surname);
			values.Put("Username", username);
			values.Put("Password", password);

			return db.insert("Users", null, values);
		}

		public Data (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public Data (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
		}
	}
}

