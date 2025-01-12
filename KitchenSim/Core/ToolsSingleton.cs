using Godot;
using System;
using System.Collections.Generic;

public interface IToolsSingleton
{
	void AddCuttingBoard(CuttingBoard cuttingBoard);
	void AddOvenAndStove(OvenAndStove ovenAndStove);
	void AddFridge(Fridge fridge);
	CuttingBoard? TryGetAvailableCuttingBoard();
	OvenAndStove? TryGetAvailableOvenAndStove();
	Fridge? TryGetAvailableFridge();
}

public partial class ToolsSingleton : Node, IToolsSingleton
{
	List<CuttingBoard> _cuttingBoardList = new List<CuttingBoard>();
	List<OvenAndStove> _ovenAndStoveList = new List<OvenAndStove>();
	List<Fridge> _fridgeList = new List<Fridge>();

	ILoggerService _logger;

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
	}

	public void AddCuttingBoard(CuttingBoard cuttingBoard)
	{
		_logger.LogInfo("Add Cutting Board");
		_cuttingBoardList.Add(cuttingBoard);
	}

	public void AddOvenAndStove(OvenAndStove ovenAndStove)
	{
		_logger.LogInfo("Add Oven and Stove");
		_ovenAndStoveList.Add(ovenAndStove);
	}

	public void AddFridge(Fridge fridge)
	{
		_logger.LogInfo("Add Fridge");
		_fridgeList.Add(fridge);
	}

	public CuttingBoard? TryGetAvailableCuttingBoard()
	{
		foreach (var cuttingBoard in _cuttingBoardList)
		{
			if (cuttingBoard.CheckIfInUse() == false) 
			{
				_logger.LogInfo("Found Cutting Board");
				cuttingBoard.SetToUsing();
				return cuttingBoard;
			} 
		}
		return null;
	}

	public OvenAndStove? TryGetAvailableOvenAndStove()
	{
		foreach (var ovenAndStove in _ovenAndStoveList)
		{
			if (ovenAndStove.CheckIfInUse() == false) 
			{
				_logger.LogInfo("Found Oven and Stove");
				ovenAndStove.SetToUsing();
				return ovenAndStove;
			} 
		}
		return null;
	}

	public Fridge? TryGetAvailableFridge()
	{
		foreach (var fridge in _fridgeList)
		{
			if (fridge.CheckIfInUse() == false) 
			{
				_logger.LogInfo("Found Fridge");
				fridge.SetToUsing();
				return fridge;
			} 
		}
		return null;
	}
}
