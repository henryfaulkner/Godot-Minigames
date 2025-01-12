
using Godot;
using System;
using System.Text;

public partial class StaffAgent : Agent, ITile
{
	string _firstName;
	string _lastName;
	Order? _activeOrder;

	ITool? _toolTarget;

	ILoggerService _logger;
	IOrderQueueSingleton _orderQueueSingleton;
	INamePickerService _namePickerService;

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_orderQueueSingleton = GetNode<IOrderQueueSingleton>(Constants.SingletonNodes.OrderQueueSingleton);
		_namePickerService = GetNode<INamePickerService>(Constants.SingletonNodes.NamePickerService);

		ReadyAgent();
		
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
		else if (_toolTarget == null)
		{
			_toolTarget = _activeOrder.RecipeBuilder.CheckForBestNextStep();
			if (_toolTarget != null) 
			{
				SetNavTarget(_toolTarget.GetNodeSelf());
			}
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

	public override void HandleNavTargetArrival()
	{
		// Do tool action immediately
		// TODO: add action delay to give work weight 
		_logger.LogInfo("STAFF: An agent reached its target.");
		switch (_toolTarget.GetToolType())
		{
			case Enumerations.ToolTypes.CuttingBoard:
				_activeOrder.RecipeBuilder.ChopIngredients();
				break;
			case Enumerations.ToolTypes.Fridge:
				_activeOrder.RecipeBuilder.CheckFridge();
				break;
			case Enumerations.ToolTypes.OvenAndStove:
				_activeOrder.RecipeBuilder.CookWithOvenAndStove();
				break;
			default:
				_logger.LogError("StaffAgent HandleNavTargetArrival ToolTypes did not map properly.");
				break;
		}

		// leave tool
		SetNavTarget(null);
		_toolTarget = null;
	}
}
