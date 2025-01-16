using Godot; 
using System;
using System.Collections.Generic;

public partial class CustomerAgent : Agent, ITile
{
	string _firstName;
	string _lastName;
	Order? _order;
	Table _tableTarget;

	ILoggerService _logger;
	IOrderQueueSingleton _orderQueueSingleton;
	IOrderFactory _orderFactory;
	INamePickerService _namePickerService;
	ITablesSingleton _tablesSingleton;

	#region State Machine
	States _state;
	enum States
	{
		MovingToTable,
		WaitingAtTable,
		Leaving,
	}
	#endregion

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_orderQueueSingleton = GetNode<IOrderQueueSingleton>(Constants.SingletonNodes.OrderQueueSingleton);
		_orderFactory = GetNode<IOrderFactory>(Constants.SingletonNodes.OrderFactory);
		_namePickerService = GetNode<INamePickerService>(Constants.SingletonNodes.NamePickerService);
		_tablesSingleton = GetNode<ITablesSingleton>(Constants.SingletonNodes.TablesSingleton);

		ReadyAgent();

		_firstName = _namePickerService.GetRandomName();
		_lastName = "Customer";
		_order = DecideOrder();

		_state = States.MovingToTable;
		
		_logger.LogInfo("End of Customer _Ready");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_order == null) return;

		if (_state == States.WaitingAtTable) return;

		if (_tableTarget == null)
		{
			_logger.LogInfo("Customer looking for table");
			_tableTarget = _tablesSingleton.TryGetAvailableTable();
			if (_tableTarget != null)
			{
				SetNavTarget(_tableTarget.GetNodeSelf());
			}
		}
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
		SetNavTarget(null);

		_logger.LogInfo("CUSTOMER: An agent reached its target.");
		switch (_state)
		{
			case States.MovingToTable:
				_state = States.WaitingAtTable; 

				PlaceOrder(_order);
				_order.IsRequested = true;
				_tableTarget.Order = _order;

				_tableTarget.StopUsing();
				_tableTarget = null;
				break;
			case States.WaitingAtTable:
				break;
			case States.Leaving:
				break;
			default:
				break;
		}
	}
}
