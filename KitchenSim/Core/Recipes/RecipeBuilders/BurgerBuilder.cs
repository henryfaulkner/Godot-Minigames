using Godot;
using System;
using System.Collections.Generic;

public class BurgerBuilder : IRecipeBuilder
{
	string _currentActivity;

	readonly ILoggerService _logger;
	Recipe _recipe;
	IToolsSingleton _toolsSingleton;

	public BurgerBuilder(ILoggerService logger, IToolsSingleton toolsSingleton)
	{
		_recipe = new Recipe("Burger", new List<IRecipeComponent>());
		_logger = logger;
		_toolsSingleton = toolsSingleton;
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

	public string GetCurrentActivity()
	{
		return _currentActivity;
	}

	public ITool? CheckForBestNextStep()
	{
		var pattyComponent = _recipe.TryGetComponent("Patty"); 
		if (pattyComponent == null)
		{
			var availableFridge = _toolsSingleton.TryGetAvailableFridge();
			if (availableFridge != null) return availableFridge;

			// exit 
			return null;
		}

		var donenessProp = pattyComponent.TryGetValue("Doneness");
		if (donenessProp != "Cooked")
		{
			var availableOvenAndStove = _toolsSingleton.TryGetAvailableOvenAndStove();
			if (availableOvenAndStove != null) return availableOvenAndStove;
		}

		var productComponent = _recipe.TryGetComponent("Product"); 
		if (productComponent == null
			|| pattyComponent.HasProperty("Bun")
			|| pattyComponent.HasProperty("Cheese"))
		{
			var availableFridge = _toolsSingleton.TryGetAvailableFridge();
			if (availableFridge != null) return availableFridge;
		}
		
		return null;
	}

	public void CheckFridge()
	{
		GrabPatty();
		AddBun();
		AddCheese();
	}

	public void CookWithOvenAndStove()
	{
		CookPatty();
		AddCookedPatty();
	}

	public void ChopIngredients()
	{
		return;
	}


	private void GrabPatty()
	{
		_currentActivity = "Grabbing patty";

		_recipe.AddComponent(
			new RecipeComponent("Patty", new Dictionary<string, string> {
				{"Doneness", "NotCooked"}
			})
		);
	}

	private void CookPatty()
	{
		_currentActivity = "Cooking patty";

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

	private void AddCookedPatty()
	{
		_currentActivity = "Adding cooked patty to product";

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
	}

	private void AddBun()
	{
		_currentActivity = "Adding bun to product";

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

	private void AddCheese()
	{
		_currentActivity = "Adding cheese to product";

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
