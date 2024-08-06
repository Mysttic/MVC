using Npgsql;

public class PostgreSQLListener : IListener
{
	NpgsqlConnection connection;
	public Settings Settings { get; set; }
	public ILogManager LogManager { get; set; }
	public IGitManager GitManager { get; set; }

	public PostgreSQLListener(Settings settings)
	{
		Settings = settings;
		LogManager = new LogManager(settings);
		GitManager = new GitManager(settings, LogManager);		
	}

	public async Task Listener()
	{
		try
		{
			using (connection = new NpgsqlConnection(Settings.ConnectionString))
			{
				await ((IListener)this).Listen(connection, 
					new NpgsqlCommand("SELECT * FROM CHANNEL", connection), 
					new NpgsqlCommand("SELECT * FROM CODE_TEMPLATE", connection));
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