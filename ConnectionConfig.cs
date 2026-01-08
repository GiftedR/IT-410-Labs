static class ConnectionConfig
{
	public static readonly string[] PossibleServers =
	{
		"localhost",
		"localhost\\SQLEXPRESS",
		"(localdb)\\MSSQLLocalDB",
	};

	public const string Database = "ODBCDatabase";

	public const string SqlUser = "ODBCUser";
	public const string SqlPassword = "ODBCPassword";

	public static void PrintServerCandidates()
	{
		Console.WriteLine("Server Candidates to Try:");
		foreach (string s in PossibleServers)
		{
			Console.WriteLine($" - {s}");
		}
	}
}