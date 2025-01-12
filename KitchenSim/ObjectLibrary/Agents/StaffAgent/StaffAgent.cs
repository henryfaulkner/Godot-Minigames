
using Godot;
using System;
using System.Text;

public partial class StaffAgent : Agent, ITile
{
	string _firstName;
	string _lastName;
	Order? _activeOrder;

	ILoggerService _logger;
	IOrderQueueSingleton _orderQueueSingleton;
	INamePickerService _namePickerService;

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_orderQueueSingleton = GetNode<IOrderQueueSingleton>(Constants.SingletonNodes.OrderQueueSingleton);
		_namePickerService = GetNode<INamePickerService>(Constants.SingletonNodes.NamePickerService);
		
		_firstName = _namePickerService.GetRandomName();
		_lastName = "Staffer";
	}

	public override void _Process(double delta)
	{
		if (_activeOrder == null)
		{
			_activeOrder = TryTakeOrder();
			if (_activeOrder != null) _activeOrder.State = Order.States.Preparing;
		}
		else if (_activeOrder.State == Order.States.Delivering)
		{
			// take to customer
		}
		else
		{
			var toolForNextStep = _activeOrder.RecipeBuilder.CheckForBestNextStep();
			if (toolForNextStep != null) 
				SetNavTarget(toolForNextStep.GetNodeSelf());
		}
	}

	public Order? TryTakeOrder()
	{
		return _orderQueueSingleton.TryDequeue();
	}
	
	public string GetActivity()
	{
		var strBuilder = new StringBuilder(string.Empty);
		strBuilder.Append($"{_firstName} {_lastName}: ");
		if (_activeOrder != null) strBuilder.Append(_activeOrder.ToString());
		return strBuilder.ToString();
	}
}
