public static class Constants
{
	public static class Config
	{
		public const string ConnectionString = "Data Source=app_database.db;";
	}

	public static class SingletonNodes
	{ 
		public const string LoggerService = "/root/LoggerService";
		public const string CommonInteractor = "/root/CommonInteractor";
		public const string EggInteractor = "/root/EggInteractor";
		public const string AnimalInteractor = "/root/AnimalInteractor";
		public const string ForegroundSubjectFactory = "/root/ForegroundSubjectFactory";
		public const string Observables = "/root/Observables";
	}
}
