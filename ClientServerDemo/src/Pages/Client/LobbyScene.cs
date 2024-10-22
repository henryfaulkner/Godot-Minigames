using Godot;
using System;

public partial class LobbyScene : Control
{
	[ExportGroup("Custom Inputs")]
	[Export]
	private LineEdit NameInput { get; set; }

	[Export]
	private LineEdit IPInput { get; set; }

	[Export]
	private LineEdit PortInput { get; set; }

	[Export]
	private Button JoinGameButton { get; set; }
	
	private LobbyService _serviceLobby;
	private LoggerService _serviceLogger;
	
	public override void _Ready()
	{
		_serviceLobby = GetNode<LobbyService>("/root/LobbyService");
		JoinGameButton.Pressed += OnJoinGamePressed;
		
		_serviceLogger = GetNode<LoggerService>("/root/LoggerService");
		_serviceLogger.LogError("Log error");
	}

	private void OnJoinGamePressed()
	{
		try
		{
			_serviceLobby.SelectedIP = IPInput.Text;
			_serviceLobby.SelectedPort = Int32.Parse(PortInput.Text);
			_serviceLobby.JoinGame();
		} 
		catch (Exception ex)
		{
			GD.Print($"LobbyScene OnJoinGamePressed exception: {ex.Message}");
		}
	}
}
