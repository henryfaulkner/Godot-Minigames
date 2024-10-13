using Godot;
using System;

public partial class LobbyScene : Control
{
	[Export]
	private Button JoinGameButton { get; set; }
	
	private LobbyService _serviceLobby;
	
	public override void _Ready()
	{
		_serviceLobby = GetNode<LobbyService>("/root/LobbyService");
		
		JoinGameButton.Pressed += OnJoinGamePressed;
	}

	private void OnJoinGamePressed()
	{
		try
		{
			_serviceLobby.JoinGame();
		} 
		catch (Exception ex)
		{
			GD.Print($"LobbyScene OnJoinGamePressed exception: {ex.Message}");
		}
	}
}
