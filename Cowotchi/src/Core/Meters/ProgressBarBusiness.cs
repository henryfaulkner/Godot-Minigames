using System;
using Godot;

public class ProgressBarBusiness
{
	public float MaxLevel { get; set; }
	public float CurrentLevel { get; set; }

	public Label ValueLabel { get; set; }
	public ProgressBar ProgressBar { get; set; }

	public ProgressBarBusiness(
		float currentLevel,
		float maxLevel,
		Label valueLabel,
		ProgressBar progressBar)
	{
		CurrentLevel = currentLevel;
		MaxLevel = maxLevel;

		ValueLabel = valueLabel;
		ProgressBar = progressBar;
	}

	public bool IsAtMax()
	{
		return CurrentLevel >= MaxLevel;
	}

	public void AddToMeter(float amount)
	{
		if (CurrentLevel + amount < 0)
		{
			CurrentLevel = 0;
		}
		else if (CurrentLevel + amount < MaxLevel)
		{
			CurrentLevel += amount;
		}
		else
		{
			CurrentLevel = MaxLevel;
		}
		ValueLabel.Text = $"{CurrentLevel}/{MaxLevel}";
		TweenVisualLevelTowardCurrentLevel();
	}

	// https://www.youtube.com/watch?v=fpBOEJXZeYs&t=5s
	public void TweenVisualLevelTowardCurrentLevel()
	{
		//if (CurrentLevel == VisualLevel) return;
		var tween = ProgressBar.GetTree().CreateTween();
		tween.TweenProperty(ProgressBar, "value", CurrentLevel / MaxLevel * 100, 1).SetTrans(Tween.TransitionType.Linear);
	}
}
