using Godot;
using System;

public partial class ServerScene : Node2D
{
	private ServerService _serviceServer; 

	public override void _Ready()
	{
		try
		{
			_serviceServer = GetNode<ServerService>("/root/ServerService");
			GD.Print(_serviceServer == null);
			_serviceServer.CreateGame();
		} 
		catch (Exception ex)
		{
			GD.Print($"ServerScene _Ready exception: {ex.Message}");
		}
	}
}
