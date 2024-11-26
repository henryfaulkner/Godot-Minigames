public static class Constants
{
	public static class Config
	{
		public const string ConnectionString = "Data Source=app_database.db;";
	}

	public static class SingletonNodes
	{ 
		public const string LoggerService = "/root/LoggerService";
		public const string Observables = "/root/Observables";
		public const string GameStateInteractor = "/root/GameStateInteractor";
		public const string CommonInteractor = "/root/CommonInteractor";
		public const string EggInteractor = "/root/EggInteractor";
		public const string AnimalInteractor = "/root/AnimalInteractor";
		public const string ForegroundSubjectFactory = "/root/ForegroundSubjectFactory";
		public const string BackgroundSubjectFactory = "/root/BackgroundSubjectFactory";
		public const string AnimationPathFactory = "/root/AnimationPathFactory";
		public const string CommandFactory = "/root/CommandFactory";
	}

	public static class KeyNodePaths
	{
		public const string FarmWanderers = "/root/Main/FarmWanderers";
		public const string BgSpawnPoint = "/root/Main/BgSpawnPoint";
		public const string Commands = "../Main/Commands";
	}
}
