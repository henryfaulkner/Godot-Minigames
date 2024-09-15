using Godot;
using Newtonsoft.Json;
using System;

public class PlayerConfigBusiness
{
	private static readonly string _CONFIG_FILE_DIRECTORY = "res://src/Core/PlayerConfigs/PlayerConfigs/";

	public void Commit(string fileName, PlayerConfigModel config)
	{
		GD.Print($"Commit PlayerConfig to {GetFilePath(fileName)}");
		try
		{
			string content = JsonConvert.SerializeObject(config, Formatting.Indented);
			using var file = FileAccess.Open(GetFilePath(fileName), FileAccess.ModeFlags.Write);
			file.StoreString(content);
		}
		catch (Exception exception)
		{
			GD.Print($"Commit exception: {exception}");
			GD.Print($"Commit config: {config}");
		}
	}

	public PlayerConfigModel Load(string fileName)
	{
		PlayerConfigModel result;
		try
		{
			GD.Print("Load");
			using var file = FileAccess.Open(GetFilePath(fileName), FileAccess.ModeFlags.Read);
			string content = file.GetAsText();
			GD.Print($"Loaded {content}");
			result = JsonConvert.DeserializeObject<PlayerConfigModel>(content);
		}
		catch (Exception exception)
		{
			GD.Print($"Error {exception.Message}");
			result = new PlayerConfigModel();
		}
		return result;
	}

	public void Reset(string fileName)
	{
		try
		{
			var defaultConfigModel = Load($"{fileName}-default");
			Commit(fileName, defaultConfigModel);
		}
		catch (Exception exception)
		{
			GD.Print($"Commit exception: {exception}");
		}
	}

	private StringName GetFilePath(string fileName)
	{
		return new StringName($"{_CONFIG_FILE_DIRECTORY}{fileName}.json");
	}
}
