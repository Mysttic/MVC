
public interface IListener
{
	ILogManager LogManager { get; set; }
	Settings Settings { get; set; }
	public void Start()
	{
		if (Settings == null || string.IsNullOrEmpty(Settings.ConnectionString))
		{
			//log and cancel
			LogManager.Log("Connection string is empty", GetType().Name).Wait();
		}
		try
		{
			LogManager.Log("Starting listener", GetType().Name).Wait();
			Listener().Wait();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			Stop();
			throw;
			//log and cancel
		}
	}
	public void Stop()
	{
		LogManager.Log("Stopping listener", GetType().Name).Wait();
	}
	Task Listener();
}
