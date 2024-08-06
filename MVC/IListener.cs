
public interface IListener
{
	ILogManager LogManager { get; set; }
	Settings Settings { get; set; }
	public async Task Start()
	{
		if (Settings == null || string.IsNullOrEmpty(Settings.ConnectionString))
		{
			//log and cancel
			LogManager.Log("Connection string is empty", GetType().Name).Wait();
		}
		try
		{
			Task.WaitAll(ChannelListener(), CodeTemplateListener());
			//if (bool.TryParse(Settings?.LogChannels, out bool channelRes))
			//{
			//	LogManager.Log("Starting channel listener", GetType().Name).Wait();
				
			//}
			//if (bool.TryParse(Settings?.LogCodeTemplates, out bool codetemplateRes))
			//{
			//	LogManager.Log("Starting code template listener", GetType().Name).Wait();
			//	await CodeTemplateListener();
			//}
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
	Task ChannelListener();
	Task CodeTemplateListener();
}
