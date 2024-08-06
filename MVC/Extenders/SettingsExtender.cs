public static class SettingsExtender
{
	/// <summary>
	/// This method fills the settings with the parameters passed in the command line.
	/// </summary>
	/// <param name="settings"></param>
	/// <param name="args"></param>
	/// <returns></returns>
	public static Settings FillParameters(this Settings settings, string[] args)
	{
		settings.ConnectionString = args.Where(arg => arg.ToLower().StartsWith("/connectionstring="))
								  .Select(arg => arg.Substring("/connectionstring=".Length))
								  .FirstOrDefault() ?? "";
		settings.LogPath = args.Where(arg => arg.ToLower().StartsWith("/logpath="))
						  .Select(arg => arg.Substring("/logpath=".Length))
						  .FirstOrDefault() ?? ".\\Logs\\";
		settings.RepoPath = args.Where(arg => arg.ToLower().StartsWith("/repopath="))
						  .Select(arg => arg.Substring("/repopath=".Length))
						  .FirstOrDefault() ?? ".\\Repo\\";
		settings.UseGit = args.Where(arg => arg.ToLower().StartsWith("/usegit="))
						  .Select(arg => arg.Substring("/usegit=".Length))
						  .FirstOrDefault() ?? "false";
		settings.LogChannels = args.Where(arg => arg.ToLower().StartsWith("/logchannels="))
								.Select(arg => arg.Substring("/logchannels=".Length))
								.FirstOrDefault() ?? "true";
		settings.LogCodeTemplates = args.Where(arg => arg.ToLower().StartsWith("/logcodetemplates="))
								   .Select(arg => arg.Substring("/logcodetemplates=".Length))
								   .FirstOrDefault() ?? "true";
		return settings;
	}
}
