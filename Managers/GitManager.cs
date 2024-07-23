using LibGit2Sharp;
using System.Runtime.CompilerServices;

public class GitManager : IGitManager
{
	Settings Settings { get; set; }
	ILogManager LogManager { get; set; }
	public GitManager(Settings settings, ILogManager logManager)
	{
		Settings = settings;
		LogManager = logManager;
		if (!Directory.Exists(Settings.RepoPath))
			Directory.CreateDirectory(Settings.RepoPath);
	}

	/// <summary>
	/// Changes the file content and saves it to the repo
	/// </summary>
	/// <param name="dto"></param>
	/// <param name="caller"></param>
	/// <returns></returns>
	public async Task Change(DbDataReaderDto dto, [CallerMemberName] string caller = "")
	{
		string filePath = Path.Combine(
		Settings.RepoPath,
		dto.Name +
			(Settings.UseGit == "true" ? "" : $"_{DateTime.Now.ToString().Replace('.', '_').Replace(':', '_')}_Rev{dto.Revision}"));
		File.WriteAllText(filePath, dto.Content);

		if (Settings.UseGit == "true")
		{
			if (!Repository.IsValid(Settings.RepoPath))
				await NewRepo(Settings.RepoPath);

			await Stage(filePath);
			if (HasChanges())
				await Commit(dto.Name, caller);
		}
	}

	/// <summary>
	/// Commits the changes to the repo
	/// </summary>
	/// <param name="message"></param>
	/// <param name="caller"></param>
	/// <returns></returns>
	public async Task Commit(string message, string caller)
	{
		using (var repo = GetRepoFromPath())
		{
			var author = new Signature(caller ?? "Unknown", caller ?? "Unknown", DateTimeOffset.Now);
			var committer = author;

			repo.Commit(message, author, committer);
			await LogManager.Log($"Committed: {message} by {caller}");
		}
	}

	/// <summary>
	/// Gets the repo from the path
	/// </summary>
	/// <returns></returns>
	public Repository GetRepoFromPath()
	{
		return new Repository(Settings.RepoPath);
	}

	/// <summary>
	/// Checks if there are any changes in the repo
	/// </summary>
	/// <returns></returns>
	public bool HasChanges()
	{
		using (var repo = GetRepoFromPath())
		{
			return repo.RetrieveStatus().IsDirty;
		}
	}

	/// <summary>
	/// Creates a new repo at the given path
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public async Task NewRepo(string path)
	{
		Repository.Init(path);
		await LogManager.Log($"New repo created at {path}");
	}

	/// <summary>
	/// Stages the file at the given path
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public async Task Stage(string path)
	{
		using (var repo = GetRepoFromPath())
		{
			Commands.Stage(repo, path);
			//await LogManager.Log($"Staged: {path}");
		}
	}
}
