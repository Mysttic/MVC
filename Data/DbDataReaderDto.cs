using System.Data.Common;

public class DbDataReaderDto : IDisposable
{
	public DbDataReaderDto(DbDataReader reader)
	{
		Id = reader[0].ToString();
		Name = reader[1].ToString();
		Revision = reader[2].ToString();
		Content = reader[3].ToString();
	}
	public string? Id { get; set; }
	public string? Name { get; set; }
	public string? Revision { get; set; }
	public string? Content { get; set; }

	public void Dispose()
	{
		GC.SuppressFinalize(this);
	}
}
