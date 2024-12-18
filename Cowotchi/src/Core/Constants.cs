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
		public const string CharacterFactory = "/root/CharacterFactory";
		public const string AnimationPathFactory = "/root/AnimationPathFactory";
		public const string EffectsFactory = "/root/EffectsFactory";
		public const string CommandFactory = "/root/CommandFactory";
		public const string ControllerFactory = "/root/ControllerFactory";
	}

	public static class KeyNodePaths
	{
		public const string FarmWanderers = "/root/Main/FarmWanderers";
		public const string BgSpawnPoint = "/root/Main/BgSpawnPoint";
		public const string Commands = "../Main/Commands";
	}

	public static class AnimalMeshes
	{
		public const string Cat = "res://src/Assets/Meshes/Animals/Cat.tres";
		public const string Dog = "res://src/Assets/Meshes/Animals/Dog.tres";
		public const string Horse = "res://src/Assets/Meshes/Animals/Horse.tres";
		public const string Pig = "res://src/Assets/Meshes/Animals/Pig.tres";
		public const string Sheep = "res://src/Assets/Meshes/Animals/Sheep.tres";
	}
}
