
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public interface IListener
{
	ILogManager LogManager { get; set; }
	IGitManager GitManager { get; set; }
	Settings Settings { get; set; }
	Task Listener();

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
	async Task Listen(DbConnection dbConnection, DbCommand channelCommand, DbCommand codeTemplateCommand)
	{
		await dbConnection.OpenAsync();
		await LogManager.Log("Connected to the database.", GetType().Name);

		Dictionary<string, string> channels = new Dictionary<string, string>();
		Dictionary<string, string> codeTemplates = new Dictionary<string, string>();

		List<Func<Task>> actions = InitializeActions(channelCommand, codeTemplateCommand, channels, codeTemplates);
		
		while (true)
		{
			foreach (Func<Task> action in actions)			
				await action();
			
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
	List<Func<Task>> InitializeActions(DbCommand channelCommand, DbCommand codeTemplateCommand, Dictionary<string, string> channels, Dictionary<string, string> codeTemplates)
	{
		List<Func<Task>> actions = new List<Func<Task>>();
		if (Settings.GitChannels.ToLower() == bool.TrueString.ToLower())
		{
			actions.Add(async () => await ExecuteCommand(channelCommand, channels, "Channels"));
			LogManager.Log("Listening for changes in Channels", GetType().Name).Wait();
		}
		else		
			LogManager.Log("Not listening for changes in Channels", GetType().Name).Wait();
		
		if (Settings.GitCodeTemplates.ToLower() == bool.TrueString.ToLower())
		{
			actions.Add(async () => await ExecuteCommand(codeTemplateCommand, codeTemplates, "CodeTemplates"));
			LogManager.Log("Listening for changes in CodeTemplates", GetType().Name).Wait();
		}
		else		
			LogManager.Log("Not listening for changes in CodeTemplates", GetType().Name).Wait();
		

		return actions;
	}
}
