using Godot; 
using System;
using System.Collections.Generic;

public partial class CustomerAgent : Agent, ITile
{
	List<Order> _ordersToPlace;

	ILoggerService _logger;
	IOrderQueueSingleton _orderQueueSingleton;

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_orderQueueSingleton = GetNode<IOrderQueueSingleton>(Constants.SingletonNodes.OrderQueueSingleton);

		//_ordersToPlace = GenerateOrdersToPlace();
	}

	public void PlaceOrder(Order order)
	{
		_orderQueueSingleton.Enqueue(order);
		_ordersToPlace.Remove(order);
	}

	//public List<Order> GenerateOrdersToPlace()
	//{
//
	//}
}
