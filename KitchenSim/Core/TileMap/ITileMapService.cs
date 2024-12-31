using System.Collections.Generic;

public interface ITileMapService
{
	int GetTileSize();
	void SetTileSize(int tileSize);
	List<List<ITile>> GetTileGrid();
	void SetTileGrid(List<List<ITile>> tileGrid);
}
