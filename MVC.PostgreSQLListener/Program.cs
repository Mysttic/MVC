internal class Program
{
	private static async Task Main(string[] args)
	{
		Settings settings = new Settings().FillParameters(args);
		IListener listener = new PostgreSQLListener(settings);
		AppDomain.CurrentDomain.ProcessExit += (s, e) => listener.Stop();
		listener.Start();
	}
}