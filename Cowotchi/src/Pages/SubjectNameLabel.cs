using Godot;
using System;

public partial class SubjectNameLabel : RichTextLabel
{
	private ILoggerService _logger { get; set; }
	private Observables _observables { get; set; }
	
	public override void _Ready()
	{
		_logger = GetNode<ILoggerService>(Constants.SingletonNodes.LoggerService);
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		
		_observables.UpdateSubjectNameLabel += UpdateSubjectNameLabel;
	}

	public void UpdateSubjectNameLabel(string text)
	{
		Text = text;
	}
}
