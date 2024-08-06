using MySql.Data.MySqlClient;

public class MySQLListener : IListener
{
	MySqlConnection connection;
	public Settings Settings { get; set; }
	public ILogManager LogManager { get; set; }
	public IGitManager GitManager { get; set; }

	public MySQLListener(Settings settings)
	{
		Settings = settings;
		LogManager = new LogManager(settings);
		GitManager = new GitManager(settings, LogManager);
	}

	public async Task Listener()
	{
		try
		{
			using (connection = new MySqlConnection(Settings.ConnectionString))
			{
				await ((IListener)this).Listen(connection, 
					new MySqlCommand("SELECT * FROM CHANNEL", connection), 
					new MySqlCommand("SELECT * FROM CODE_TEMPLATE", connection));
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