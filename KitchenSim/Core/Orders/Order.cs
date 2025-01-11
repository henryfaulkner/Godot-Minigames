using Godot;
using System;
using System.Collections.Generic;

public class Order
{    
    public OrderType OrderType { get; set; } 
    public IRecipeBuilder RecipeBuilder { get; set; }
    public bool IsCompleted { get; set; }
}