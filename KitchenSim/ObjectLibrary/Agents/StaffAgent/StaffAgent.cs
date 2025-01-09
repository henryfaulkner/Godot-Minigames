using Godot;
using System;

public partial class StaffAgent : Agent, ITile
{
	Order? _activeOrder;

	ILoggerService _logger;
	IOrderQueueSingleton _orderQueueSingleton;

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_orderQueueSingleton = GetNode<ILoggerService>(Constants.SingletonNodes.OrderQueueSingleton);
	}

	public override void _Process()
	{
		if (_activeOrder == null || _activeOrder.IsCompleted)
		{
			_activeOrder = TryTakeOrder();
		}
	}

	public Order? TryTakeOrder()
	{
		_activeOrder = _orderQueueSingleton.TryDequeue();
	}
}
