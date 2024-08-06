
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public interface IListener
{
	ILogManager LogManager { get; set; }
	IGitManager GitManager { get; set; }
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
	async Task Listen(DbConnection dbConnection, DbCommand channelCommand, DbCommand codeTemplateCommand)
	{
		await dbConnection.OpenAsync();
		await LogManager.Log("Connected to the database.", GetType().Name);

		Dictionary<string, string> channels = new Dictionary<string, string>();
		Dictionary<string, string> codeTemplates = new Dictionary<string, string>();

		while (true)
		{
			await ExecuteCommand(channelCommand, channels, "Channels");
			await ExecuteCommand(codeTemplateCommand, codeTemplates, "CodeTemplates");

			// Wait for a while before querying again
			await Task.Delay(5000); // 5 seconds
		}
	}
	async Task ExecuteCommand(DbCommand command, Dictionary<string, string> dictionary, string caller)
	{
		try
		{
			using (DbDataReader reader = await command.ExecuteReaderAsync())
			{
				while (await reader.ReadAsync())
				{
					using (DbDataReaderDto readerDto = new DbDataReaderDto(reader))
					{
						if (!dictionary.TryGetValue(readerDto.Id, out string? value))
						{
							dictionary[readerDto.Id] = readerDto.Content;
							await GitManager.Change(readerDto, caller);
						}
						else if (value != readerDto.Content)
						{
							dictionary[readerDto.Id] = readerDto.Content;
							await GitManager.Change(readerDto, caller);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			await LogManager.Log($"Error reading data: {ex.Message}", GetType().Name);
		}

	}
}
