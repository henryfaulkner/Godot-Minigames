using Godot;
using System;

public partial class Server : Node
{
	private const int DEFAULT_PORT = 3234;
	private const int MAX_PLAYERS = 4;

	private readonly ENetMultiplayerPeer Network;
	
	public Server() 
	{
		Network = new ENetMultiplayerPeer();
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	
	public void StartServer()
	{
		
	}
}
