using System.Collections.Generic;

public interface ITileMapService
{
	List<List<IGridTile>> GetTileGrid();
	void SetTileGrid(List<List<IGridTile>> tileGrid);
}
