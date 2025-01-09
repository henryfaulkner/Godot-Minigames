using Godot;
using System;
using System.Collections.Generic;

public interface IOrderQueueSingleton
{
    void Enqueue(Order order);
    Order? TryDequeue();
}

public class OrderQueueSingleton
{
    Queue<Order> _orderQueue = new Queue<Order>();

    public OrderQueueSingleton() {}

    public void Enqueue(Order order)
    {
        _orderQueue.Enqueue(order);
    }

    public Order? TryDequeue()
    {
        try
        {
            return _orderQueue.Dequeue();
        }
        catch (InvalidOperationException exception)
        {
            // The queue is empty.
            return null;
        }
    }
}