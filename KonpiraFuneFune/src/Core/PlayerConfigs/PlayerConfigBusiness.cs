using Godot;
using Newtonsoft.Json;
using System;

// In the future, I should prob use cfg file format over json file format
// based on: https://github.com/godotengine/godot-demo-projects/blob/3.0-d69cc10/gui/input_mapping/controls.gd
public class PlayerConfigBusiness
{
	private static readonly string[] _INPUT_ACTIONS = new string[] { "tap", "grab", "knock" };
	private static readonly string _CONFIG_FILE_DIRECTORY = "res://src/Core/PlayerConfigs/PlayerConfigs/";

	public PlayerConfigBusiness(string fileName)
	{
		try
		{
			Load(fileName);
			foreach (var actionName in _INPUT_ACTIONS)
			{
				// assume we want the first key binding
				var inputEvent = InputMap.ActionGetEvents(actionName)[0];
			}
		}
		catch (Exception e)
		{
			GD.PrintErr($"PlayerConfigBusiness did not construct properly. {e.Message}");
		}
	}

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
			GD.PrintErr($"Commit exception: {exception}");
			GD.PrintErr($"Commit config: {config}");
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
			GD.PrintErr($"Error {exception.Message}");
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
			GD.PrintErr($"Commit exception: {exception}");
		}
	}

	private StringName GetFilePath(string fileName)
	{
		return new StringName($"{_CONFIG_FILE_DIRECTORY}{fileName}.json");
	}
}
