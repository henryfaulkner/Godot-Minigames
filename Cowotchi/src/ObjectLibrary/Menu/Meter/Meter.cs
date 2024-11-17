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

	public override void _Ready()
	{
		if (Icon != null && IconTexture != null) Icon.Texture = IconTexture;
		
		_progressBarBusiness = new ProgressBarBusiness(0, 5, ProgressBar);
	}

	public void UpdateMaxAndValue(int max, int value)
	{
		_progressBarBusiness.CurrentLevel = value;
		_progressBarBusiness.MaxLevel = max;
		_progressBarBusiness.TweenVisualLevelTowardCurrentLevel();
		
		ValueLabel.Text = $"{value}/{max}";
	}

	public void UpdateMax(int max)
	{
		_progressBarBusiness.MaxLevel = max;
		_progressBarBusiness.TweenVisualLevelTowardCurrentLevel();
		
		ValueLabel.Text = $"{_progressBarBusiness.CurrentLevel}/{max}";
	}

	public void UpdateValue(int value)
	{
		_progressBarBusiness.CurrentLevel = value;
		_progressBarBusiness.TweenVisualLevelTowardCurrentLevel();
		
		ValueLabel.Text = $"{value}/{_progressBarBusiness.MaxLevel}";
	}
}
