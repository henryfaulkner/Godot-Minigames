using Godot;
using System;

public partial class PlayerHud : Control
{	
	[Export]
	private ProgressBar ThrowSpeedBar { get; set; }
	
	[Export]
	private double PercentIncrementPerFrame { get; set; }

	private bool IsCharging { get; set; } = false;
	private double ChargeValue { get; set; } = 0.0;

	public override void _Process(double delta)
	{
		if (IsCharging)
		{
			if (ChargeValue == ThrowSpeedBar.MaxValue) return;
			else if (ChargeValue + PercentIncrementPerFrame > ThrowSpeedBar.MaxValue) ChargeValue = ThrowSpeedBar.MaxValue;
			else ChargeValue += PercentIncrementPerFrame;
			
			TweenCurrentValue();
		}
	}
	
	public void BeginCharging()
	{
		IsCharging = true;
	}
	
	// return charge percentage
	public double ReleaseCharge()
	{
		var result = ChargeValue / ThrowSpeedBar.MaxValue;
		ChargeValue = 0.0;
		IsCharging = false;
		TweenCurrentValue();
		return result;
	}
	
	public void CancelCharge()
	{
		ChargeValue = 0.0;
		IsCharging = false;
		TweenCurrentValue();
	}
	
	// https://www.youtube.com/watch?v=fpBOEJXZeYs&t=5s
	private void TweenCurrentValue()
	{
		var tween = ThrowSpeedBar.GetTree().CreateTween();
		tween.TweenProperty(ThrowSpeedBar, "value", ChargeValue / ThrowSpeedBar.MaxValue * 100, 1).SetTrans(Tween.TransitionType.Linear);
	}
}
