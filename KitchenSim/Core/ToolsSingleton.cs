using Godot;
using System;
using System.Collections.Generic;

public interface IToolsSingleton
{
    CuttingBoard? TryGetAvailableCuttingBoard();
    OvenAndStove? TryGetAvailableOvenAndStove();
    Fridge? TryGetAvailableFridge();
}

public partial class ToolsSingleton : Node, IToolsSingleton
{
    List<CuttingBoard> _cuttingBoardList = new List<CuttingBoard>();
    List<OvenAndStove> _ovenAndStoveList = new List<FridgOvenAndStove>();
    List<Fridge> _fridgeList = new List<Fridge>();

    public void SetToolsList(List<CuttingBoard> cuttingBoardList, OvenAndStove ovenAndStoveList, Fridge fridgeList)
    {
        _cuttingBoardList = cuttingBoardList;
        _ovenAndStoveList = ovenAndStoveList;
        _fridgeList = fridgeList;
    }

    public CuttingBoard? TryGetAvailableCuttingBoard()
    {
        CuttingBoard? result = null;
        foreach (var cuttingBoard in _cuttingBoardList)
        {
            if (cuttingBoard.CheckIfInUse() == false) return cuttingBoard; 
        }
    }

    public OvenAndStove? TryGetAvailableOvenAndStove()
    {
        OvenAndStove? result = null;
        foreach (var ovenAndStove in _ovenAndStoveList)
        {
            if (ovenAndStove.CheckIfInUse() == false) return ovenAndStove; 
        }
    }

    public Fridge? TryGetAvailableFridge()
    {
        Fridge? result = null;
        foreach (var fridge in _fridgeList)
        {
            if (fridge.CheckIfInUse() == false) return fridge; 
        }
    }
}