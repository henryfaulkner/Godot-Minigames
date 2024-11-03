using Godot;
using System;

public partial class Main : Node3D
{
	private ILoggerService _logger { get; set; }
	private ICommonInteractor _commonInteractor { get; set; } 

	public override void _Ready()
	{
		GD.Print($"Call Main _Ready");
		try
		{
			_commonInteractor = GetNode<ICommonInteractor>("/root/CommonInteractor");
			_commonInteractor.InitDatabaseIfRequired();
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Main _Ready Error: {ex.Message}");
			_logger.LogError($"Main _Ready Error: {ex.Message}", ex);
		}
	}
}
