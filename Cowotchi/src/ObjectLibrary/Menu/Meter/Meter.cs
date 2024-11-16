using Godot;
using System;

public partial class Meter : MarginContainer
{
	[Export]
	private TextureRect Icon { get; set; }

	[Export]
	private Texture2D IconTexture { get; set; }

	[Export]
	private ProgressBar ProgressBar { get; set; }

	[Export]
	private Label ValueLabel { get; set; }

	private ProgressBarBusiness _progressBarBusiness;

	public Meter()
	{
		_progressBarHelper = new ProgressBarHelper(0, 5, ValueLabel, ProgressBar);
	}

	public override void _Ready()
	{
		_meterObservable = GetNode<MeterObservable>("/root/MeterObservable");

		if (Icon != null && IconTexture != null) Icon.Texture = IconTexture;

		switch (MeterType)
		{
			case Enumerations.Meters.Heart:

				break;
			case Enumerations.Meters.Hunger:
				break;
			default:
				break;
		}
	}

	public void UpdateMaxAndValue(int max, int value)
	{
		_progressBarBusiness.CurrentLevel = value;
		_progressBarBusiness.MaxLevel = max;
		_progressBarBusiness.ValueLabel.Text = $"{_progressBarBusiness.CurrentLevel}/{_progressBarBusiness.MaxLevel}";
		_progressBarBusiness.TweenVisualLevelTowardCurrentLevel();
	}

	public void UpdateMax(int max)
	{
		_progressBarBusiness.MaxLevel = max;
		_progressBarBusiness.ValueLabel.Text = $"{_progressBarBusiness.CurrentLevel}/{_progressBarBusiness.MaxLevel}";
		_progressBarBusiness.TweenVisualLevelTowardCurrentLevel();
	}

	public void UpdateValue(int value)
	{
		_progressBarBusiness.ValueLevel = value;
		_progressBarBusiness.ValueLabel.Text = $"{_progressBarBusiness.CurrentLevel}/{_progressBarBusiness.MaxLevel}";
		_progressBarBusiness.TweenVisualLevelTowardCurrentLevel();
	}
}
