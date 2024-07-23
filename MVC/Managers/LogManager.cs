using System.Runtime.CompilerServices;

public class LogManager : ILogManager
{
	Settings Settings { get; set; }
	public LogManager(Settings settings)
	{
		Settings = settings;
		if (!Directory.Exists(Settings.LogPath))
			Directory.CreateDirectory(Settings.LogPath);
	}

	/// <summary>
	/// Log a message to the log file
	/// </summary>
	/// <param name="message"></param>
	/// <param name="caller"></param>
	/// <returns></returns>
	public async Task Log(string message, [CallerMemberName] string caller = "")
	{
		string logMessage = $"{DateTime.Now} - {caller}: {message}";
		Console.WriteLine(logMessage);		
		await File.AppendAllTextAsync(Settings.LogPath + "\\log.txt", logMessage + Environment.NewLine);
	}
}
