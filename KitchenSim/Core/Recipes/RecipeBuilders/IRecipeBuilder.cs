using Godot;
using System;

public interface IRecipeBuilder
{
    void Reset();
    IRecipe GetResult();
    bool CheckDoneness();

    ITool? CheckForNextStep();
    void CheckFridge();
    void CookWithOvenAndStove();
    void ChopIngredients();
}