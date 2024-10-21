using Godot;
using System;

public partial class ServerScene : Node2D
{
	private LobbyService _serviceLobby;

	public override void _Ready()
	{
		try
		{
			_serviceLobby = GetNode<LobbyService>("/root/LobbyService");
			_serviceLobby.CreateGame();
		} 
		catch (Exception ex)
		{
			GD.Print($"ServerScene _Ready exception: {ex.Message}");
		}
	}
}
