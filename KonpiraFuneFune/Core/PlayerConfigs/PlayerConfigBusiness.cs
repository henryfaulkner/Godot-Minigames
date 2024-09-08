using Godot;
using Newtonsoft.Json;
using System;

public class PlayerConfigBusiness
{
	private static readonly string _CONFIG_FILE_DIRECTORY = "res://Core/PlayerConfigs/PlayerConfigs/";
	private StringName FilePath { get; set; }

	public PlayerConfigBusiness(string fileName)
	{
		FilePath = new StringName($"{_CONFIG_FILE_DIRECTORY}{fileName}");
	}

	public void Commit(PlayerConfigModel config)
	{
		GD.Print($"Commit PlayerConfig to {FilePath}");
		try
		{
			string content = JsonConvert.SerializeObject(config, Formatting.Indented);
			using var file = FileAccess.Open(FilePath, FileAccess.ModeFlags.Write);
			file.StoreString(content);
		}
		catch (Exception exception)
		{
			GD.Print($"Commit exception: {exception}");
			GD.Print($"Commit config: {config}");
		}
	}

	public PlayerConfigModel Load()
	{
		try
		{
			GD.Print("Load");
			//_serviceLogger.LogDebug("Load");
			using var file = FileAccess.Open(FilePath, FileAccess.ModeFlags.Read);
			string content = file.GetAsText();
			var json = JsonConvert.DeserializeObject<PlayerConfigModel>(content);
			return json;
		}
		catch (Exception exception)
		{
			//_serviceLogger.LogError($"Load exception: {exception}");
		}
		return new PlayerConfigModel();
	}
}
