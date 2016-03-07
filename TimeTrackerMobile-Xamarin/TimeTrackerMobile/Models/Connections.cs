using System;

namespace TimeTrackerMobile
{
	public class Connections
	{
		public Connections ()
		{
		}

		public long ConnectionId { get; set; }

		public long CompanyId { get; set; }

		public string ConnectionName{ get; set; }

		public Connections (int connectionId, int companyId, string connectionName)
		{
			ConnectionId = connectionId;
			CompanyId = companyId;
			ConnectionName = connectionName;
		}
	}
}

