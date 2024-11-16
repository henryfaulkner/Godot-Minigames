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
		_progressBarBusiness = new ProgressBarBusiness(0, 5, ValueLabel, ProgressBar);
	}

	public override void _Ready()
	{
		if (Icon != null && IconTexture != null) Icon.Texture = IconTexture;
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
		_progressBarBusiness.CurrentLevel = value;
		_progressBarBusiness.ValueLabel.Text = $"{_progressBarBusiness.CurrentLevel}/{_progressBarBusiness.MaxLevel}";
		_progressBarBusiness.TweenVisualLevelTowardCurrentLevel();
	}
}
