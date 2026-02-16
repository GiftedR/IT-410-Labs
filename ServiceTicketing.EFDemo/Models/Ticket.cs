using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceTicketing.EFDemo.Models
{
	public class Ticket
	{
		public int TicketId	{ get; set;	}
		public string Title	{ get; set;	} =	"";
		public string Description {	get; set; }	= "";
		public string Status { get;	set; } = "";
		public string Priority { get; set; } = "";
		public DateTime	CreatedOn {	get; set; }
		public string? ResolutionSummary { get; set; }
	}
}
