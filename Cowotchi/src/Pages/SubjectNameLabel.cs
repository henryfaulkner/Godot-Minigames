using Godot;
using System;

public partial class SubjectNameLabel : RichTextLabel
{
	private Observables _observables { get; set; }
	
	public override void _Ready()
	{
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);
		
		_observables.UpdateSubjectNameLabel += (string text) => Text = text;
	}
}
