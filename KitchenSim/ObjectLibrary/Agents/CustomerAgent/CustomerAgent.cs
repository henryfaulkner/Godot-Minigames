using Godot; 
using System;
using System.Collections.Generic;

public partial class CustomerAgent : Agent, ITile
{
	string _firstName;
	string _lastName;
	Order? _unrequestedOrder;
	Table _tableTarget;

	ILoggerService _logger;
	IOrderQueueSingleton _orderQueueSingleton;
	IOrderFactory _orderFactory;
	_namePickerService _namePickerService;

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_orderQueueSingleton = GetNode<IOrderQueueSingleton>(Constants.SingletonNodes.OrderQueueSingleton);
		_orderFactory = GetNode<IOrderFactory>(Constants.SingletonNodes.OrderFactory);
		_namePickerService = GetNode<INamePickerService>(Constants.SingletonNodes.NamePickerService);

		ReadyAgent();

		_firstName = _namePickerService.GetRandomName();
		_lastName = "Customer";
		_unrequestedOrder = DecideOrder();

		// Have customer search for a table
		// then PlaceOrder()
	}

	public void PlaceOrder(Order order)
	{
		_orderQueueSingleton.Enqueue(order);
	}

	public Order? DecideOrder()
	{
		Order? result = null;
		
		Random random = new Random();
		int randomNumber = random.Next(0, 3); // The upper bound (3) is exclusive
		switch (randomNumber)
		{
			case (int)Enumerations.OrderTypes.Burger:
				result = _orderFactory.CreateBurgerOrder();
				break;
			case (int)Enumerations.OrderTypes.Salad:
				result = _orderFactory.CreateSaladOrder();
				break;
			case (int)Enumerations.OrderTypes.FrenchFries:
				result = _orderFactory.CreateFrenchFriesOrder();
				break;
			default:
				_logger.LogError("CustomerAgent DecideOrder OrderType did not map properly.");
				break;
		}
		return result;
	}
	
	public override void HandleNavTargetArrival()
	{
		_logger.LogInfo("CUSTOMER: An agent reached its target.");
	}
}
