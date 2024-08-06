using Oracle.ManagedDataAccess.Client;

public class OracleListener : IListener
{
	OracleConnection connection;
	public Settings Settings { get; set; }
	public ILogManager LogManager { get; set; }
	public IGitManager GitManager { get; set; }

	public OracleListener(Settings settings)
	{
		Settings = settings;
		LogManager = new LogManager(settings);
		GitManager = new GitManager(settings, LogManager);
	}

	public async Task Listener()
	{
		try
		{
			using (connection = new OracleConnection(Settings.ConnectionString))
			{
				await ((IListener)this).Listen(connection, 
					new OracleCommand("SELECT * FROM CHANNEL", connection), 
					new OracleCommand("SELECT * FROM CODE_TEMPLATE", connection));
			}
		}
		catch (Exception ex)
		{
			await LogManager.Log(ex.Message, GetType().Name);
		}
		finally
		{
			connection?.Close();
		}
	}
}