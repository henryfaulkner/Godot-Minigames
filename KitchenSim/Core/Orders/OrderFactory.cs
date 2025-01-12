using Godot;
using System;

public interface IOrderFactory
{
	Order CreateBurgerOrder(); 
	Order CreateSaladOrder(); 
	Order CreateFrenchFriesOrder(); 
}

public partial class OrderFactory : Node, IOrderFactory
{
	ILoggerService _logger;
	IRecipeBuilderFactory _recipeBuilderFactory;

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_recipeBuilderFactory = GetNode<IRecipeBuilderFactory>(Constants.SingletonNodes.RecipeBuilderFactory);
	}

	public Order CreateBurgerOrder()
	{
		var result = new Order();
		result.OrderType = Enumerations.OrderTypes.Burger;
		result.RecipeBuilder = _recipeBuilderFactory.CreateBurgerBuilder();
		return result;
	}

	public Order CreateSaladOrder()
	{
		var result = new Order();
		result.OrderType = Enumerations.OrderTypes.Salad;
		result.RecipeBuilder = _recipeBuilderFactory.CreateSaladBuilder();
		return result;
	}

	public Order CreateFrenchFriesOrder()
	{
		var result = new Order();
		result.OrderType = Enumerations.OrderTypes.FrenchFries;
		result.RecipeBuilder = _recipeBuilderFactory.CreateFrenchFryBuilder();
		return result;
	}
}
