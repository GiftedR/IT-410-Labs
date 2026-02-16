using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceTicketing.EFDemo.Models
{
	public class User
	{
		public int UserId { get; set; }
		public string FullName { get; set; } = "";
		public string Email { get; set; } = "";
		public DateTime CreatedOn { get; set; }
	}
}
