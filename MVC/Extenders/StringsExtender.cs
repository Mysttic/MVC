public static class StringsExtender
{
	public static string FormatFileName(this string fileName)
	{
		return fileName.Replace(" ", "_").Replace(":", "_").Replace("/", "_").Replace("\\", "_");
	}
}
