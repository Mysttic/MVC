using System.ServiceProcess;

internal class Program
{
	private static async Task Main(string[] args)
	{
		// Sprawdź, czy aplikacja działa w kontenerze
		if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true" ||
			Environment.UserInteractive ||
			args.Contains("console"))
		{
			// Uruchom logikę bez ServiceBase (tryb konsolowy)
			Settings settings = new Settings().FillParameters(args);
			IListener listener = new MySQLListener(settings);
			Console.CancelKeyPress += (s, e) => listener.Stop();
			listener.Start();

			// Utrzymaj proces w działaniu
			await Task.Delay(-1);
		}
		else
		{
			ServiceBase.Run(new MyService());
		}
	}

	internal class MyService : ServiceBase
	{
		private IListener _listener;

		public MyService()
		{
			this.ServiceName = "MVC.MySQLListenerService";
		}

		protected override void OnStart(string[] args)
		{
			Task.Run(() =>
			{
				Settings settings = new Settings().FillParameters(args);
				_listener = new MySQLListener(settings);
				_listener.Start();
			});
		}

		protected override void OnStop()
		{
			_listener?.Stop();
		}
	}
}