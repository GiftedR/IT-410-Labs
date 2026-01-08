using System;
using System.Data.SqlClient;

class SqlClientTest
{
	public static void Run()
	{
		Console.WriteLine("=== SQLClient Connection Test ===");
		ConnectionConfig.PrintServerCandidates();

		foreach (var server in ConnectionConfig.PossibleServers)
		{
			string connString =
				$"Server={server};Database={ConnectionConfig.Database};" +
				$"User Id={ConnectionConfig.SqlUser};Password={ConnectionConfig.SqlPassword};";

			Console.WriteLine($"\nAttempting SQLClient connection using server: {server}");

			#pragma warning disable CS0618 // Disable Obsolete Warning
			using (var conn = new SqlConnection(connString))
			{
				try
				{
					conn.Open();
					Console.WriteLine($"SUCCESS: Connected via SQLClient using {server}");
					return;
				}
				catch (Exception ex)
				{
					Console.WriteLine($"FAILED using {server}: {ex.Message}");
				}
			}
			#pragma warning restore CS0618
		}

		Console.WriteLine("\nAll SQLClient connection attempts failed.");
		Console.WriteLine("Please verify:");
		Console.WriteLine("- Your SQL Server instance name");
		Console.WriteLine("- That SQL Server is running");
		Console.WriteLine("- That your username/password are correct");
	}
}