using System;

namespace TimeTrackerMobile
{
	public class User
	{

		public int UserId { get; set; }

		public string Username { get; set; }

		public string Password	{ get; set; }

		public string FirstName { get; set; }

		public string LastName	{ get; set; }

		public User ()
		{
			
		}

		public User (int userId, string firstname, string lastname, string username, string password)
		{
			UserId = userId;
			FirstName = firstname;
			LastName = lastname;
			Username = username;
			Password = password;
		}
	}
}