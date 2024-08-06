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

	public async Task ChannelListener()
	{
		try
		{
			using (connection = new NpgsqlConnection(Settings.ConnectionString))
			{
				await connection.OpenAsync();
				await LogManager.Log("ChannelListener connected to the database.", GetType().Name);

				using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM CHANNEL", connection))
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
											channels[readerDto.Id] = readerDto.Content;
											await GitManager.Change(readerDto, GetType().Name);
										}
										else if (value != readerDto.Content)
										{
											channels[readerDto.Id] = readerDto.Content;
											await GitManager.Change(readerDto, GetType().Name);											
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

	public async Task CodeTemplateListener()
	{
		try
		{
			using (connection = new NpgsqlConnection(Settings.ConnectionString))
			{
				await connection.OpenAsync();
				await LogManager.Log("CodeTemplateListener connected to the database.", GetType().Name);

				using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM CODE_TEMPLATE", connection))
				{
					Dictionary<string, string> codeTemplates = new Dictionary<string, string>();

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
										if (!codeTemplates.TryGetValue(readerDto.Id, out string? value))
										{
											codeTemplates[readerDto.Id] = readerDto.Content;
											await GitManager.Change(readerDto, GetType().Name);
										}
										else if (value != readerDto.Content)
										{
											codeTemplates[readerDto.Id] = readerDto.Content;
											await GitManager.Change(readerDto, GetType().Name);
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

