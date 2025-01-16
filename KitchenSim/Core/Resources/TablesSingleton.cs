using Godot;
using System;
using System.Collections.Generic;

public interface ITablesSingleton
{
	void AddTable(Table table);
	Table? TryGetAvailableTable();  
}

public partial class TablesSingleton : Node, ITablesSingleton
{
	List<Table> _tableList = new List<Table>();

	ILoggerService _logger;

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

	public void AddTable(Table table)
	{
		_logger.LogInfo("Add Table");
		_tableList.Add(table);
	}

	public Table? TryGetAvailableTable()
	{
		foreach (var table in _tableList)
		{
			if (table.CheckIfInUse() == false) 
			{
				_logger.LogInfo("Found Table");
				table.SetToUsing();
				return table;
			} 
		}
		return null;
	}
}
