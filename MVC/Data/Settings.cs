public class Settings
{
	public string ConnectionString { get; set; }
	public string LogPath { get; set; }
	public string RepoPath { get; set; }
	public string UseGit { get; set; }

	public string LogChannels { get; set; } = "true";
	public string LogCodeTemplates { get; set; } = "true";
}
