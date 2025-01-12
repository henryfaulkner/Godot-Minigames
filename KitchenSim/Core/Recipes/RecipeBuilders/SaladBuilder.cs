using Godot;
using System;
using System.Collections.Generic;

public class SaladBuilder : IRecipeBuilder
{
	string _currentActivity;

	readonly ILoggerService _logger;
	Recipe _recipe;
	IToolsSingleton _toolsSingleton;

	public SaladBuilder(ILoggerService logger, IToolsSingleton toolsSingleton)
	{
		_recipe = new Recipe("Salad", new List<IRecipeComponent>());
		_logger = logger;
		_toolsSingleton = toolsSingleton;
	}

	public void Reset()
	{
		_recipe = new Recipe("Salad", new List<IRecipeComponent>());
	}

	public IRecipe GetResult()
	{
		return _recipe;
	}

	public bool CheckDoneness()
	{
		var productComponent = _recipe.TryGetComponent("Product"); 
		return productComponent != null 
			&& productComponent.HasProperty("ChoppedLettuce")
			&& productComponent.HasProperty("DicedTomato")
			&& productComponent.HasProperty("Dressing");
	}

	public string GetCurrentActivity()
	{
		return _currentActivity;
	}

	public ITool? CheckForBestNextStep()
	{
		var lettuceComponent = _recipe.TryGetComponent("Lettuce");
		var tomatoComponent = _recipe.TryGetComponent("Tomato");
		var productComponent = _recipe.TryGetComponent("Product");

		if (lettuceComponent == null
			|| tomatoComponent == null)
		{
			var availableFridge = _toolsSingleton.TryGetAvailableFridge();
			if (availableFridge != null) return availableFridge;

			// exit 
			return null;
		}

		var lettuceDoneness = lettuceComponent.TryGetValue("Doneness");
		var tomatoDoneness = tomatoComponent.TryGetValue("Doneness");

		if (lettuceDoneness == "NotChopped"
			|| tomatoDoneness == "NotDiced")
		{
			var availableCuttingBoard = _toolsSingleton.TryGetAvailableCuttingBoard();
			if (availableCuttingBoard != null) return availableCuttingBoard;

			// exit
			return null;
		}
		
		return null;
	}

	public void CheckFridge()
	{
		GrabLettuce();
		GrabTomato();
		AddDressing();
	}

	public void CookWithOvenAndStove()
	{
		return;
	}

	public void ChopIngredients()
	{
		ChopLettuce();
		AddChoppedLettuce();
		DiceTomato();
		AddDicedTomato();
	}

	public void GrabLettuce()
	{
		_currentActivity = "Grabbing lettuce";

		_recipe.AddComponent(
			new RecipeComponent("Lettuce", new Dictionary<string, string> {
				{"Doneness", "NotChopped"}
			})
		);
	}

	public void ChopLettuce()
	{
		_currentActivity = "Chopping lettuce";

		var lettuceComponent = _recipe.TryGetComponent("Lettuce"); 
		if (lettuceComponent == null)
		{
			_logger.LogInfo ("Lettuce cannot be chopped. No Lettuce in inventory.");
			return;
		}

		var donenessProp = lettuceComponent.TryGetValue("Doneness");
		if (donenessProp == null)
		{
			_logger.LogError("Lettuce cannot be chopped. Lettuce does not have doneness property.");
			return;
		}

		lettuceComponent.SetPropertyValue("Doneness", "Chopped");
	}

	public void AddChoppedLettuce()
	{
		_currentActivity = "Adding chopped lettuce to product";

		var lettuceComponent = _recipe.TryGetComponent("Lettuce"); 
		if (lettuceComponent == null)
		{
			_logger.LogInfo ("Lettuce cannot be added. No lettuce in inventory.");
			return;
		}

		var donenessProp = lettuceComponent.TryGetValue("Doneness");
		if (donenessProp == null && donenessProp != "Chopped")
		{
			_logger.LogError("ChoppedLettuce cannot be added. Lettuce is not chopped or does not exist.");
			return;
		}

		var productComponent = _recipe.TryGetComponent("Product"); 
		if (productComponent == null)
		{
			_recipe.AddComponent(
				new RecipeComponent("Product", new Dictionary<string, string> {
					{"ChoppedLettuce", ""},
				})
			);
		}
		else
		{
			productComponent.SetPropertyValue("ChoppedLettuce", "");
		}
	}

	// Diced tomato
	public void GrabTomato()
	{
		_currentActivity = "Grabbing tomato";

		_recipe.AddComponent(
			new RecipeComponent("Tomato", new Dictionary<string, string> {
				{"Doneness", "NotDiced"}
			})
		);
	}

	public void DiceTomato()
	{
		_currentActivity = "Dicing tomato";

		var tomatoComponent = _recipe.TryGetComponent("Tomato"); 
		if (tomatoComponent == null)
		{
			_logger.LogInfo ("Tomato cannot be chopped. No Tomato in inventory.");
			return;
		}

		var donenessProp = tomatoComponent.TryGetValue("Doneness");
		if (donenessProp == null)
		{
			_logger.LogError("Tomato cannot be chopped. Tomato does not have doneness property.");
			return;
		}

		tomatoComponent.SetPropertyValue("Doneness", "Diced");
	}

	public void AddDicedTomato()
	{
		_currentActivity = "Adding diced tomato to product";

		var tomatoComponent = _recipe.TryGetComponent("Tomato"); 
		if (tomatoComponent == null)
		{
			_logger.LogInfo ("Tomato cannot be added. No tomato in inventory.");
			return;
		}

		var donenessProp = tomatoComponent.TryGetValue("Doneness");
		if (donenessProp == null && donenessProp != "Diced")
		{
			_logger.LogError("DicedTomato cannot be added. Tomato is not diced or does not exist.");
			return;
		}

		var productComponent = _recipe.TryGetComponent("Product"); 
		if (productComponent == null)
		{
			_recipe.AddComponent(
				new RecipeComponent("Product", new Dictionary<string, string> {
					{"DicedTomato", ""},
				})
			);
		}
		else
		{
			productComponent.SetPropertyValue("DicedTomato", "");
		}
	}

	public void AddDressing()
	{
		_currentActivity = "Adding dressing to product";

		var productComponent = _recipe.TryGetComponent("Product"); 
		if (productComponent == null)
		{
			_recipe.AddComponent(
				new RecipeComponent("Product", new Dictionary<string, string> {
					{"Dressing", ""},
				})
			);
		}
		else
		{
			productComponent.SetPropertyValue("Dressing", "");
		}
	}
}
