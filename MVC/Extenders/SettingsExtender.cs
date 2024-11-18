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
		settings.GitChannels = args.Where(arg => arg.ToLower().StartsWith("/gitchannels="))
								.Select(arg => arg.Substring("/gitchannels=".Length))
								.FirstOrDefault() ?? "true";
		settings.GitCodeTemplates = args.Where(arg => arg.ToLower().StartsWith("/gitcodetemplates="))
								   .Select(arg => arg.Substring("/gitcodetemplates=".Length))
								   .FirstOrDefault() ?? "true";
		settings.CommitMode = args.Where(arg => arg.ToLower().StartsWith("/commitmode="))
			.Select(arg => arg.Substring("/commitmode=".Length))
								.FirstOrDefault() switch
								{
									"Auto" => CommitMode.Auto,
									"Manual" => CommitMode.Manual,
									_ => CommitMode.Auto
								};
		return settings;
	}
}
