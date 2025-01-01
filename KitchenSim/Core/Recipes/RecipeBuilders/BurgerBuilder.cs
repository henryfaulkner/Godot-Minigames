using Godot;
using System;
using System.Collections.Generic;

public class BurgerBuilder : IRecipeBuilder
{
	readonly ILoggerService _logger;
	Recipe _recipe;

	public BurgerBuilder(ILoggerService logger)
	{
		_recipe = new Recipe("Burger", new List<IRecipeComponent>());
		_logger = logger;
	}

	public void Reset()
	{
		_recipe = new Recipe("Burger", new List<IRecipeComponent>());
	}

	public IRecipe GetResult()
	{
		return _recipe;
	}

	public bool CheckDoneness()
	{
		var productComponent = _recipe.TryGetComponent("Product"); 
		return productComponent != null 
			&& productComponent.HasProperty("CookedPatty")
			&& productComponent.HasProperty("Bun")
			&& productComponent.HasProperty("Cheese");
	}

	public void GrabPatty()
	{
		_recipe.AddComponent(
			new RecipeComponent("Patty", new Dictionary<string, string> {
				{"Doneness", "NotCooked"}
			})
		);
	}

	public void CookPatty()
	{
		var pattyComponent = _recipe.TryGetComponent("Patty"); 
		if (pattyComponent == null)
		{
			_logger.LogInfo ("Patty cannot be cooked. No patty in inventory.");
			return;
		}

		var donenessProp = pattyComponent.TryGetValue("Doneness");
		if (donenessProp == null)
		{
			_logger.LogError("Patty cannot be cooked. Patty does not have doneness property.");
			return;
		}

		pattyComponent.SetPropertyValue("Doneness", "Cooked");
	}

	public void AddCookedPatty()
	{
		var pattyComponent = _recipe.TryGetComponent("Patty"); 
		if (pattyComponent == null)
		{
			_logger.LogInfo ("Patty cannot be added. No patty in inventory.");
			return;
		}

		var donenessProp = pattyComponent.TryGetValue("Doneness");
		if (donenessProp == null && donenessProp != "Cooked")
		{
			_logger.LogError("CookedPatty cannot be added. Patty is not cooked or does not exist.");
			return;
		}

		var productComponent = _recipe.TryGetComponent("Product"); 
		if (productComponent == null)
		{
			_recipe.AddComponent(
				new RecipeComponent("Product", new Dictionary<string, string> {
					{"CookedPatty", ""},
				})
			);
		}
		else
		{
			productComponent.SetPropertyValue("CookedPatty", "");
		}
		
		_recipe.RemoveComponent(pattyComponent);
	}

	public void AddBun()
	{
		var productComponent = _recipe.TryGetComponent("Product"); 
		if (productComponent == null)
		{
			_recipe.AddComponent(
				new RecipeComponent("Product", new Dictionary<string, string> {
					{"Bun", ""},
				})
			);
		}
		else
		{
			productComponent.SetPropertyValue("Bun", "");
		}
	}

	public void AddCheese()
	{
		var productComponent = _recipe.TryGetComponent("Product"); 
		if (productComponent == null)
		{
			_recipe.AddComponent(
				new RecipeComponent("Product", new Dictionary<string, string> {
					{"Cheese", ""},
				})
			);
		}
		else
		{
			productComponent.SetPropertyValue("Cheese", "");
		}
	}
}
