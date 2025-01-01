using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public interface IRecipeComponent
{
	string GetName();
	string TryGetValue(string key);
	void SetPropertyValue(string key, string newValue);
	bool HasProperty(string key);
}

public class RecipeComponent : IRecipeComponent
{
	string _name { get; set; }
	Dictionary<string, string> _properties { get; set; }

	public RecipeComponent(string name, Dictionary<string, string> properties = null)
	{
		_name = name;
		_properties = properties ?? new Dictionary<string, string>();
	}
	
	public string GetName()
	{
		return _name;
	}

	public string TryGetValue(string key)
	{
		_properties.TryGetValue(key, out string result);
		return result;
	}

	public void SetPropertyValue(string key, string newValue)
	{
		_properties[key] = newValue;
	}

	
	public bool HasProperty(string key)
	{
		return _properties.TryGetValue(key, out string result);
	} 

	public override string ToString()
	{
		var props = string.Join(", ", _properties.Select(p => $"{p.Key}: {p.Value}"));
		return $"{_name} ({props})";
	}
}
