using Godot;
using System;
using System.Collections.Generic;
using System.Text;

public class Order
{    
	public Enumerations.OrderTypes OrderType { get; set; } 
	public IRecipeBuilder RecipeBuilder { get; set; }
	
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
	}

	public string ToString()
	{
		var strBuilder = new StringBuilder(string.Empty);
		strBuilder.Append($"Order:{OrderType.GetDescription()} ");
		return strBuilder.ToString();
	}
}
