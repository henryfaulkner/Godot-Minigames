using Godot;
using System;
using System.Collections.Generic;

public class FrenchFryBuilder : IRecipeBuilder
{
	readonly ILoggerService _logger;
	Recipe _recipe;

	public FrenchFryBuilder(ILoggerService logger)
	{
		_recipe = new Recipe("FrenchFries", new List<IRecipeComponent>());
		_logger = logger;
	}

	public void Reset()
	{
		_recipe = new Recipe("FrenchFries", new List<IRecipeComponent>());
	}

	public IRecipe GetResult()
	{
		return _recipe;
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
