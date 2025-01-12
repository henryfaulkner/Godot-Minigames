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

	public void AddCuttingBoard(CuttingBoard cuttingBoard)
	{
		_cuttingBoardList.Add(cuttingBoard);
	}

	public void AddOvenAndStove(OvenAndStove ovenAndStove)
	{
		_ovenAndStoveList.Add(ovenAndStove);
	}

	public void AddFridge(Fridge fridge)
	{
		_fridgeList.Add(fridge);
	}

	public CuttingBoard? TryGetAvailableCuttingBoard()
	{
		foreach (var cuttingBoard in _cuttingBoardList)
		{
			if (cuttingBoard.CheckIfInUse() == false) return cuttingBoard; 
		}
		return null;
	}

	public OvenAndStove? TryGetAvailableOvenAndStove()
	{
		foreach (var ovenAndStove in _ovenAndStoveList)
		{
			if (ovenAndStove.CheckIfInUse() == false) return ovenAndStove; 
		}
		return null;
	}

	public Fridge? TryGetAvailableFridge()
	{
		foreach (var fridge in _fridgeList)
		{
			if (fridge.CheckIfInUse() == false) return fridge; 
		}
		return null;
	}
}
