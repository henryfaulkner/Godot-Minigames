using Godot;
using System;
using System.Collections.Generic;
using System.Text;

public class Order
{ 
	public Guid Id { get; set; }   	
	public Enumerations.OrderTypes OrderType { get; set; } 
	public IRecipeBuilder RecipeBuilder { get; set; }
	public bool IsRequested { get; set; }
	
	#region State Machine
	public States State { get; set; }
	public enum States 
	{
		Available,
		Preparing,
		Delivering,
		Delivered,
	}
	#endregion
	
	public Order()
	{
		State = States.Available;
		IsRequested = false;
	}

	public string ToString()
	{
		var strBuilder = new StringBuilder(string.Empty);
		strBuilder.Append($"Preparing {OrderType.GetDescription()} order | ");
		strBuilder.Append($"Current activity is {RecipeBuilder.GetCurrentActivity()}.");
		return strBuilder.ToString();
	}
}
