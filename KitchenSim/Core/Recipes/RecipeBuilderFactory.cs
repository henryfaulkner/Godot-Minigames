using Godot;
using System;

public interface IRecipeBuilderFactory
{
	BurgerBuilder CreateBurgerBuilder();
	SaladBuilder CreateSaladBuilder();
	FrenchFryBuilder CreateFrenchFryBuilder();
}

public partial class RecipeBuilderFactory : Node, IRecipeBuilderFactory
{
	ILoggerService _logger;
	IToolsSingleton _toolsSingleton;

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_toolsSingleton = GetNode<IToolsSingleton>(Constants.SingletonNodes.ToolsSingleton);
	}

	public BurgerBuilder CreateBurgerBuilder()
	{
		return new BurgerBuilder(_logger, _toolsSingleton);
	}

	public SaladBuilder CreateSaladBuilder()
	{
		return new SaladBuilder(_logger, _toolsSingleton);
	}

	public FrenchFryBuilder CreateFrenchFryBuilder()
	{
		return new FrenchFryBuilder(_logger, _toolsSingleton);
	}
}
