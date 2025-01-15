using Godot;
using System;
using System.Collections.Generic;

public interface INamePickerService
{
	string GetRandomName();
}

public partial class NamePickerService : Node, INamePickerService
{
	List<string> _names;
	
	public NamePickerService()
	{
		// Initialize the list with some generic American first names
		_names = new List<string>
		{
			"James", "Mary", "John", "Patricia", "Robert", "Jennifer",
			"Michael", "Linda", "William", "Elizabeth", "David", "Barbara",
			"Richard", "Susan", "Joseph", "Jessica", "Thomas", "Sarah",
			"Charles", "Karen"
		};
	}

	public string GetRandomName()
	{
		if (_names.Count == 0)
		{
			throw new InvalidOperationException("No names left in the list.");
		}

		Random random = new Random();
		int index = random.Next(_names.Count); // Get a random index
		string name = _names[index];          // Get the name at the random index
		_names.RemoveAt(index);               // Remove the name from the list

		return name;
	}
}
