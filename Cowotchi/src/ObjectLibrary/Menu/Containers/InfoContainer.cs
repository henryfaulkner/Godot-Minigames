using Godot;
using System;

public partial class InfoContainer : MarginContainer
{
	[Export]
	public Label Title { get; set; }
	[Export]
	public Label Subtitle { get; set; }
	[Export]
	public RichTextLabel Content { get; set; }
}
