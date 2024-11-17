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

	private ILoggerService _logger { get; set; }
	private ProgressBarBusiness _progressBarBusiness;

	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		if (Icon != null && IconTexture != null) Icon.Texture = IconTexture;
		_progressBarBusiness = new ProgressBarBusiness(0, 5, ProgressBar);
	}

	public void UpdateMaxAndValue(int max, int value)
	{
		_progressBarBusiness.UpdateMax(max);
		_progressBarBusiness.UpdateValue(value);
		ValueLabel.Text = _progressBarBusiness.ToString();
	}

	public void UpdateMax(int max)
	{
		_logger.LogInfo($"Start Meter UpdateMax, {max}");
		_progressBarBusiness.UpdateMax(max);
		ValueLabel.Text = _progressBarBusiness.ToString();
		_logger.LogInfo("End Meter UpdateMax");
	}

	public void UpdateValue(int value)
	{
		_logger.LogInfo($"Start Meter UpdateValue, {value}");
		_progressBarBusiness.UpdateValue(value);
		ValueLabel.Text = _progressBarBusiness.ToString();
		_logger.LogInfo("End Meter UpdateValue");
	}
}
