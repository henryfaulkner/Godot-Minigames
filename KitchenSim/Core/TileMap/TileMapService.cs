using Godot;
using System;
using System.Collections.Generic;

public partial class TileMapService : Node, ITileMapService
{
	int _tileSize = 32;
	List<List<ITile>> _tileGrid = new List<List<ITile>>();     

	public int GetTileSize()
	{
		return _tileSize;        
	} 

	public void SetTileSize(int tileSize)
	{
		_tileSize = tileSize;
	}

	public List<List<ITile>> GetTileGrid()
	{
		return _tileGrid;
	}

	public void SetTileGrid(List<List<ITile>> tileGrid)
	{
		_tileGrid = tileGrid;
	}
}
