using Godot;
using System;
using System.Collections.Generic;

public partial class WaitingRoom : Popup
{
	[Export]
	private ItemList PlayerList { get; set; }
	
	public override void _Ready()
	{
		PlayerList.Clear();
	}
	
	private void RefreshPlayers(Godot.Collections.Dictionary<long, Godot.Collections.Dictionary<string, string>> players)
	{
		PlayerList.Clear();
		foreach (KeyValuePair<long, Godot.Collections.Dictionary<string, string>> player in players)
		{
			string playerName = player.Value["Name"];
			PlayerList.AddItem(playerName, null, false);
		}
	}
}
