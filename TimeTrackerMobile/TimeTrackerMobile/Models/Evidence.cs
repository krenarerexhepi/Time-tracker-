using System;

namespace TimeTrackerMobile
{

	public class Evidence
	{
		public int EvidenceId { get; set; }

		public DateTime CheckInTime { get; set; }

		public DateTime CheckOutTime { get; set; }

		public int ConnectionId { get; set; }

		public int CompanyId { get; set; }

		public int UserId { get; set; }
	}
}