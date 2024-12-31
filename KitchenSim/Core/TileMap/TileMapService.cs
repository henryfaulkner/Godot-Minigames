using System;
using System.Collections.Generic;

public class TileMapService : ITileMapService
{
	int _tileSize = -1;
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
