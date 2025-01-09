using Godot;
using System;
using System.Collections.Generic;

public partial class TileMapService : Node, ITileMapService
{
	int _tileSize = 32;

	public int GetTileSize()
	{
		return _tileSize;        
	} 

	public void SetTileSize(int tileSize)
	{
		_tileSize = tileSize;
	}
}
