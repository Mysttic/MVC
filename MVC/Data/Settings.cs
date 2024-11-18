public class Settings
{
	public string ConnectionString { get; set; }
	public string LogPath { get; set; }
	public string RepoPath { get; set; }
	public string UseGit { get; set; }
	public CommitMode CommitMode { get; set; }

	public string GitChannels { get; set; } = "true";
	public string GitCodeTemplates { get; set; } = "true";
}
