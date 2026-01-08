using System;
using System.Data.Odbc;
using System.Runtime.InteropServices;

class OdbcTest
{
	public static void Run()
	{
		Console.WriteLine("\n=== ODBC Connection Test ===");

		if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			Console.WriteLine("Note: ODBC demo is primarily designed for Windows.");
			Console.WriteLine("On macOS/Linux, ODBC driver installation and names differ.");
			Console.WriteLine("You may skip running this section if drivers are unavailable.\n");
		}

		// We'll just try the first server candidate here for simplicity.
		//string server = ConnectionConfig.PossibleServers[0];
		foreach (string server in ConnectionConfig.PossibleServers)
		{
			string connString =
				"Driver={ODBC Driver 17 for SQL Server};" +
				$"Server={server};" +
				$"Database={ConnectionConfig.Database};" +
				$"Uid={ConnectionConfig.SqlUser};" +
				$"Pwd={ConnectionConfig.SqlPassword};";

			Console.WriteLine($"Attempting ODBC connection using server: {server}");

			try
			{
				using (var conn = new OdbcConnection(connString))
				{
					conn.Open();
					Console.WriteLine("SUCCESS: ODBC connection successful!");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("ODBC Error:");
				Console.WriteLine(ex.Message);
				Console.WriteLine("\nODBC Troubleshooting Tips:");
				Console.WriteLine("- Confirm 'ODBC Driver 17 for SQL Server' is installed.");
				Console.WriteLine("- Try replacing the server name with your known instance name.");
				Console.WriteLine("- Ensure SQL Server accepts TCP/IP connections.");
			}
		}
	}
}