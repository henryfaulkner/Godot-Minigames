using Godot;
using System;
using System.Collections.Generic;

public class FrenchFryBuilder : IRecipeBuilder
{
	readonly ILoggerService _logger;
	Recipe _recipe;
	IToolsSingleton _toolsSingleton;

	public FrenchFryBuilder(ILoggerService logger, IToolsSingleton toolsSingleton)
	{
		_recipe = new Recipe("FrenchFries", new List<IRecipeComponent>());
		_logger = logger;
		_toolsSingleton = toolsSingleton;
	}

	public void Reset()
	{
		_recipe = new Recipe("FrenchFries", new List<IRecipeComponent>());
	}

	public IRecipe GetResult()
	{
		return _recipe;
	}

	public ITool? CheckForBestNextStep()
	{
		var potatoComponent = _recipe.TryGetComponent("Potato");
		var productComponent = _recipe.TryGetComponent("Product");

		if (potatoComponent == null)
		{
			var availableFridge = _toolsSingleton.TryGetAvailableFridge();
			if (availableFridge != null) return availableFridge;

			// exit 
			return null;
		}

		var isSlicedProp = potatoComponent.TryGetValue("IsSliced");
		if (isSlicedProp == null
			|| isSlicedProp == "false")
		{
			var availableCuttingBoard = _toolsSingleton.TryGetAvailableCuttingBoard();
			if (availableCuttingBoard != null) return availableCuttingBoard;
		}

		var isFriedProp = potatoComponent.TryGetValue("IsFried");
		if (isFriedProp == null
			|| isFriedProp == "false")
		{
			var availableOvenAndStove = _toolsSingleton.TryGetAvailableOvenAndStove();
			if (availableOvenAndStove != null) return availableOvenAndStove;
		}
		
		return null;
	}

	public void CheckFridge()
	{
		GrabPotato();
	}

	public void CookWithOvenAndStove()
	{
		FryPotatoSlices();
		AddFrenchFries();
	}

	public void ChopIngredients()
	{
		SlicePotato();
	}

	public bool CheckDoneness()
	{
		var productComponent = _recipe.TryGetComponent("Product"); 
		return productComponent != null 
			&& productComponent.HasProperty("FrenchFries");
	}

	public void GrabPotato()
	{
		_recipe.AddComponent(
			new RecipeComponent("Potato", new Dictionary<string, string> {
				{"IsSliced", "false"},
				{"IsFried", "false"}
			})
		);
	}

	public void SlicePotato()
	{
		var potatoComponent = _recipe.TryGetComponent("Potato");
		if (potatoComponent == null)
		{
			_logger.LogInfo ("Potato cannot be sliced. No potato in inventory.");
			return;
		}

		var isSlicedProp = potatoComponent.TryGetValue("IsSliced");
		if (isSlicedProp == null)
		{
			_logger.LogError("Potato cannot be sliced. Potato does not have IsSliced property.");
			return;
		}

		potatoComponent.SetPropertyValue("IsSliced", "true");
	}

	public void FryPotatoSlices()
	{
		var potatoComponent = _recipe.TryGetComponent("Potato");
		if (potatoComponent == null)
		{
			_logger.LogInfo ("Potato cannot be fried. No potato in inventory.");
			return;
		}

		var isSlicedProp = potatoComponent.TryGetValue("IsSliced");
		if (isSlicedProp == null || isSlicedProp == "false")
		{
			_logger.LogInfo ("Potato cannot be fried. Potato is not sliced or does not have IsSliced property.");
			return;
		}

		potatoComponent.SetPropertyValue("IsFried", "true");
	}

	public void AddFrenchFries()
	{
		var potatoComponent = _recipe.TryGetComponent("Potato");
		if (potatoComponent == null)
		{
			_logger.LogInfo ("French Fries cannot be added. No potato in inventory.");
			return;
		}

		var isSlicedProp = potatoComponent.TryGetValue("IsSliced");
		if (isSlicedProp == null || isSlicedProp == "false")
		{
			_logger.LogInfo ("French Fries cannot be added. Potato is not sliced or does not have IsSliced property.");
			return;
		}

		var isFriedProp = potatoComponent.TryGetValue("IsFried");
		if (isFriedProp == null || isFriedProp == "false")
		{
			_logger.LogInfo ("French Fries cannot be added. Potato Slices not fried or does not have IsFried property.");
			return;
		}

		var productComponent = _recipe.TryGetComponent("Product"); 
		if (productComponent == null)
		{
			_recipe.AddComponent(
				new RecipeComponent("Product", new Dictionary<string, string> {
					{"FrenchFries", ""},
				})
			);
		}
		else
		{
			productComponent.SetPropertyValue("FrenchFries", "");
		}

		_recipe.RemoveComponent(potatoComponent);
	}
}
