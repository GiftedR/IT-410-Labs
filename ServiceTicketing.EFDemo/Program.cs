using ServiceTicketing.EFDemo.Data;

namespace ServiceTicketing.EFDemo
{
	internal class Program
	{
		static void	Main(string[] args)
		{
			Console.WriteLine("Starting	EF Core	Demo...");

			using var db = new ServiceTicketingContext();

			Console.WriteLine("DbContext created successfully.");

			Console.WriteLine("Done.");
		}
	}
}
