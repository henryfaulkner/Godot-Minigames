using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public interface IRecipe 
{
	void AddComponent(IRecipeComponent component);
	IRecipeComponent? TryGetComponent(string name);
	void RemoveComponent(IRecipeComponent component);
}

public class Recipe : IRecipe
{
	readonly string _name;
	readonly List<IRecipeComponent> _components = new List<IRecipeComponent>();

	public Recipe(string name, List<IRecipeComponent> components)
	{
		_name = name;
		_components = components;
	}

	public void AddComponent(IRecipeComponent component)
	{
		_components.Add(component);
	}

	public IRecipeComponent? TryGetComponent(string name)
	{
		IRecipeComponent? result = null;
		int len = _components.Count;
		for (int i = 0; i < len; i += 1)
		{
			if (_components[i].GetName() == name) 
				result = _components[i];
		}
		return result;
	}

	public void RemoveComponent(IRecipeComponent component)
	{
		_components.Remove(component);
	}
	
	public override string ToString()
	{
		return $"{_name} contains:\n{string.Join("\n", _components.Select(c => $"- {c}"))}";
	}
}
