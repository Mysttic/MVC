using Microsoft.Data.SqlClient;

public class MSSQLListener : IListener
{
	SqlConnection connection;
	public Settings Settings { get; set; }
	public ILogManager LogManager { get; set; }
	public IGitManager GitManager { get; set; }

	public MSSQLListener(Settings settings)
	{
		Settings = settings;
		LogManager = new LogManager(settings);
		GitManager = new GitManager(settings, LogManager);
	}

	public async Task Listener()
	{
		try
		{
			using (connection = new SqlConnection(Settings.ConnectionString))
			{
				await ((IListener)this).Listen(connection, 
					new SqlCommand("SELECT * FROM CHANNEL", connection), 
					new SqlCommand("SELECT * FROM CODE_TEMPLATE", connection));
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