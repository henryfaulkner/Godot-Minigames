using Godot;
using System;

public partial class SubjectNameLabel : CanvasLayer
{
	[Export]
	private RichTextLabel NameLabel { get; set; }
	[Export]
	private Button OpenRenameButton { get; set; }
	
	private ILoggerService _logger { get; set; }
	private Observables _observables { get; set; }
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		
		_observables.UpdateSubjectNameLabel += UpdateSubjectNameLabel;
		OpenRenameButton.MouseEntered += HandleButtonMouseEntered;
		OpenRenameButton.Pressed += HandleButtonPressed;
	}

	public void UpdateSubjectNameLabel(string text)
	{
		NameLabel.Text = text;
	}
	
	private void HandleButtonMouseEntered()
	{
		_logger.LogInfo("Call SubjectNameLabel HandleButtonMouseEntered");
	}
	
	private void HandleButtonPressed()
	{
		_logger.LogInfo("Call SubjectNameLabel HandleButtonPressed");
		
		_observables.EmitOpenCloseRenameWindow(true);
	}
}
