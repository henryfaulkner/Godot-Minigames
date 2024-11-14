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
	private Label Value { get; set; }
	
	public override void _Ready()
	{
		if (Icon != null && IconTexture != null) Icon.Texture = IconTexture;
	}
	
	public void UpdateMaxAndValue(int max, int value)
	{
		
	}
	
	public void UpdateMax(int max) 
	{
		
	}
	
	public void UpdateValue(int value)
	{
		
	}
}
