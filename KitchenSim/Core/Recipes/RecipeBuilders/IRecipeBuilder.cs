using Godot;
using System;

public interface IRecipeBuilder
{
    void Reset();
    IRecipe GetResult();
    bool CheckDoneness();

    ITool? CheckForBestNextStep();
    void CheckFridge();
    void CookWithOvenAndStove();
    void ChopIngredients();
}