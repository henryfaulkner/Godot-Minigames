using Godot;
using System;

public interface IOrderFactory
{
    Order CreateBurgerOrder(); 
    Order CreateSaladOrder(); 
    Order CreateFrenchFriesOrder(); 
}

public class OrderFactory : Node, IOrderFactory
{
    ILoggerService _logger;

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

    public Order CreateBurgerOrder()
    {
        var result = new Order();
        result.OrderType = Enumerations.OrderTypes.Burger;
        result.IsCompleted = false;
        return result;
    }

    public Order CreateSaladOrder()
    {
        var result = new Order();
        result.OrderType = Enumerations.OrderTypes.Salad;
        result.IsCompleted = false;
        return result;
    }

    public Order CreateFrenchFriesOrder()
    {
        var result = new Order();
        result.OrderType = Enumerations.OrderTypes.FrenchFries;
        result.IsCompleted = false;
        return result;
    }
}