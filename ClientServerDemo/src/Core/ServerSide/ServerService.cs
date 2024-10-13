using Godot;
using System;

public partial class ServerService : Node
{
	public static ServerService Instance { get; set; }
	
	// These signals can be connected to by a UI lobby scene or the game scene.
	[Signal]
	public delegate void PlayerConnectedEventHandler(int peerId, Godot.Collections.Dictionary<string, string> playerInfo);
	[Signal]
	public delegate void PlayerDisconnectedEventHandler(int peerId);
	[Signal]
	public delegate void ServerDisconnectedEventHandler();
	
	private readonly ENetMultiplayerPeer NetworkPeer;
	
	/// This will contain player info for every player,
	// with the keys being each player's unique IDs.
	private Godot.Collections.Dictionary<long, Godot.Collections.Dictionary<string, string>> Players { get; set; }
	
	// This is the local player info. This should be modified locally
	// before the connection is made. It will be passed to every other peer.
	// For example, the value of "name" can be set to something the player
	// entered in a UI scene.
	private Godot.Collections.Dictionary<string, string> PlayerInfo { get; set; }
	
	public ServerService() 
	{
		NetworkPeer = new ENetMultiplayerPeer();
		
		Players = new Godot.Collections.Dictionary<long, Godot.Collections.Dictionary<string, string>>();
		PlayerInfo = new Godot.Collections.Dictionary<string, string>()
		{
			{ "Name", "PlayerName" },
		};
	}
	
	public override void _Ready()
	{
		NetworkPeer.PeerConnected += OnPlayerConnection;
		NetworkPeer.PeerDisconnected += OnPlayerDisconnection;
	}

	public Error CreateGame()
	{
		var peer = new ENetMultiplayerPeer();
		Error error = peer.CreateServer(ServerConstants.DEFAULT_PORT, ServerConstants.MAX_CONNECTIONS);

		if (error != Error.Ok)
		{
			return error;
		}

		Multiplayer.MultiplayerPeer = peer;
		Players[1] = PlayerInfo;
		EmitSignal(SignalName.PlayerConnected, 1, PlayerInfo);
		return Error.Ok;
	}

	private void OnPlayerConnection(long id) 
	{
		GD.Print($"Player {id} joined the server.");
	} 

	private void OnPlayerDisconnection(long id) 
	{
		GD.Print($"Player {id} left the server.");
	} 
}
