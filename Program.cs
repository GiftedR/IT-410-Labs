internal class Program
{
	static void Main(string[] args)
	{
		SqlClientTest.Run();
		OdbcTest.Run();

		Console.WriteLine("\nDemo complete. Press any key to exit.");
		Console.ReadKey();
	}
}