using Godot;
using System;

public partial class PartyLight : OmniLight3D
{
	public void SetColor(Color color)
	{
		LightColor = color ;
	}
}
