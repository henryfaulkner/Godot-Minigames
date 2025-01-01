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

    public override void _Ready()
    {
        _logger = GetNode<ILoggerService>(Constants.SingletonNodes.RecipeBuilderFactory);
    }

    public BurgerBuilder CreateBurgerBuilder()
    {
        return new BurgerBuilder(_logger);
    }

    public SaladBuilder CreateSaladBuilder()
    {
        return new SaladBuilder(_logger);
    }

    public FrenchFryBuilder CreateFrenchFryBuilder()
    {
        return new FrenchFryBuilder(_logger);
    }
}