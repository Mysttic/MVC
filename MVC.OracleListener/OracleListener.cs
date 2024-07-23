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
				await connection.OpenAsync();
				await LogManager.Log("Connected to the database.", GetType().Name);

				using (OracleCommand command = new OracleCommand("SELECT * FROM CHANNEL", connection))
				{
					Dictionary<string, string> channels = new Dictionary<string, string>();

					while (true)
					{
						try
						{
							using (System.Data.Common.DbDataReader reader = await command.ExecuteReaderAsync())
							{
								while (await reader.ReadAsync())
								{
									using (DbDataReaderDto readerDto = new DbDataReaderDto(reader))
									{
										if (!channels.TryGetValue(readerDto.Id, out string? value))
										{
											channels.Add(readerDto.Id, readerDto.Revision);
											await GitManager.Change(readerDto, GetType().Name);
										}
										else if (value != readerDto.Revision)
										{
											await GitManager.Change(readerDto, GetType().Name);
											break;
										}
									}
								}
							}
						}
						catch (Exception ex)
						{
							await LogManager.Log($"Error reading data: {ex.Message}", GetType().Name);
						}

						// Wait for a while before querying again
						await Task.Delay(5000); // 5 seconds
					}
				}
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

