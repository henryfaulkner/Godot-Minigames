using Godot;
using System;
using System.Text;

public partial class StaffAgent : Agent, ITile
{
	// Assume this node exists on the same tree-level as StaffAgent
	Marker2D StaffWaitingPoint { get; set; }

	string _firstName;
	string _lastName;
	Order? _activeOrder;
	ITool? _toolTarget;

	ILoggerService _logger;
	IOrderQueueSingleton _orderQueueSingleton;
	INamePickerService _namePickerService;
	ITablesSingleton _tablesSingleton;
	Observables _observables;

	#region State Machine
	States _state;
	enum States
	{
		Waiting,
		PreparingOrder,
		DeliveringOrder,
		ReturningToKitchen,
	}
	#endregion

	public override void _Ready()
	{
		StaffWaitingPoint = GetNode<Marker2D>("../StaffWaitingPoint");

		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_orderQueueSingleton = GetNode<IOrderQueueSingleton>(Constants.SingletonNodes.OrderQueueSingleton);
		_namePickerService = GetNode<INamePickerService>(Constants.SingletonNodes.NamePickerService);
		_tablesSingleton = GetNode<ITablesSingleton>(Constants.SingletonNodes.TablesSingleton);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);

		ReadyAgent();
		
		_firstName = _namePickerService.GetRandomName();
		_lastName = "Staffer";

		_state = States.Waiting;

		_observables.SetDebugTargetMarker += SetNavTarget;
	}

	public override void _Process(double delta)
	{
		if (_activeOrder == null)
		{
			_activeOrder = TryTakeOrder();
			if (_activeOrder != null) _state = States.PreparingOrder;
		}
		else if (_state == States.DeliveringOrder)
		{
			// take to customer
			Table? table = _tablesSingleton.TryFindTableByOrderId(_activeOrder.Id);
			if (table != null)
			{
				SetNavTarget(table.GetNodeSelf());
			}
		}
		else if (_activeOrder.RecipeBuilder.CheckDoneness())
		{
			_state = States.DeliveringOrder;
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
		SetNavTarget(null);

		_logger.LogInfo("STAFF: An agent reached its target.");
		switch (_state)
		{
			case States.PreparingOrder:
				// Do tool action with delay
				TimingFunctions.SetTimeout(() => {
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

					_toolTarget.StopUsing();
					_toolTarget = null;
				}, 3000);
				break;
			case States.DeliveringOrder:
				_activeOrder = null;
				SetNavTarget(StaffWaitingPoint);
				_state = States.ReturningToKitchen;
				break;
			case States.ReturningToKitchen:
				_state = States.Waiting;
				break;
			default:
				break;
		}
	}
}
