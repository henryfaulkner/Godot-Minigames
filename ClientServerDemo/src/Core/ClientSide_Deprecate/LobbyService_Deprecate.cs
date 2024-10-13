// Godot Docs useful example
// https://docs.godotengine.org/en/stable/tutorials/networking/high_level_multiplayer.html#example-lobby-implementation

// Rayuse's multiplayer tutorial series
// https://www.youtube.com/watch?v=MGo06JvYkrA

// general guide
// https://www.youtube.com/watch?v=aZ239kNcyng

using Godot;
using System;

public partial class LobbyService_Deprecate : Node
{
	// These signals can be connected to by a UI lobby scene or the game scene.
	[Signal]
	public delegate void PlayerConnectedEventHandler(int peerId, Godot.Collections.Dictionary<string, string> playerInfo);
	[Signal]
	public delegate void PlayerDisconnectedEventHandler(int peerId);
	[Signal]
	public delegate void ServerDisconnectedEventHandler();

	private readonly ENetMultiplayerPeer NetworkPeer;
	private string SelectedIP { get; set; }
	private int SelectedPort { get; set; }
	
	/// This will contain player info for every player,
	// with the keys being each player's unique IDs.
	private Godot.Collections.Dictionary<long, Godot.Collections.Dictionary<string, string>> Players { get; set; }
	
	// This is the local player info. This should be modified locally
	// before the connection is made. It will be passed to every other peer.
	// For example, the value of "name" can be set to something the player
	// entered in a UI scene.
	private Godot.Collections.Dictionary<string, string> PlayerInfo { get; set; }
	
	private int _playersLoaded = 0;

	public LobbyService() 
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
		Multiplayer.PeerConnected += OnPlayerConnection;
		Multiplayer.PeerDisconnected += OnPlayerDisconnection;
		Multiplayer.ConnectedToServer += OnConnectedOk;
		Multiplayer.ConnectionFailed += OnConnectedFail;
		Multiplayer.ServerDisconnected += OnServerDisconnection;
	}

	public Error JoinGame(string address = ServerConstants.DEFAULT_IP)
	{
		var peer = new ENetMultiplayerPeer();
		Error error = peer.CreateClient(address, ServerConstants.DEFAULT_PORT);

		if (error != Error.Ok)
		{
			return error;
		}

		Multiplayer.MultiplayerPeer = peer;
		return Error.Ok;
	}

	private void OnPlayerConnection(long id)
	{
		GD.Print($"Player {id}: Connected");
		
		// When a peer connects, send them my player info.
		// This allows transfer of all desired data for each player, not only the unique ID.
		RpcId(id, MethodName.RegisterPlayer, PlayerInfo);
	}

	private void OnPlayerDisconnection(long id)
	{
		GD.Print($"Player {id}: Disconnected");
	}

	
	private void OnConnectedOk()
	{
		GD.Print("Successfully connected to server");
	}

	private void OnConnectedAlready()
	{
		GD.Print("Already connected to server");
	}

	private void OnConnectedFail()
	{
		GD.Print("Filed to connect to server");
	}

	private void OnServerDisconnection()
	{
		GD.Print("Server disconnected");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer,TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void RegisterPlayer(Godot.Collections.Dictionary<string, string> newPlayerInfo)
	{
		int newPlayerId = Multiplayer.GetRemoteSenderId();
		Players[newPlayerId] = newPlayerInfo;
		EmitSignal(SignalName.PlayerConnected, newPlayerId, newPlayerInfo);
	}
}
