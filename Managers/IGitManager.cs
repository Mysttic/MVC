using LibGit2Sharp;

public interface IGitManager
{
	public Task Change(DbDataReaderDto dto, string caller = "");

	public Repository GetRepoFromPath();

	public Task NewRepo(string path);

	public Task Stage(string path);

	public Task Commit(string message, string caller);

	public bool HasChanges();
}
