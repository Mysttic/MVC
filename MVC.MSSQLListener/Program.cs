internal class Program
{
	private static void Main(string[] args)
	{
		Settings settings = new Settings().FillParameters(args);
		IListener listener = new MSSQLListener(settings);
		AppDomain.CurrentDomain.ProcessExit += (s, e) => listener.Stop();
		listener.Start();		
	}
}