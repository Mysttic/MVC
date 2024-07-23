using System.Runtime.CompilerServices;

public interface ILogManager
{
	Task Log(string message, [CallerMemberName] string caller = "");
}
