using System.Collections.Generic;

public class TileMapService : ITileMapService
{
    int _tileSize = -1;
    List<List<IGridTile>> _tileGrid = new List<List<IGridTile>>();     

    public int GetTileSize()
    {
        return _tileSize;        
    } 

    public void SetTileSize(int tileSize)
    {
        _tileGrid = tileSize;
    }

    public List<List<IGridTile>> GetTileGrid()
    {
        return _tileGrid;
    }

    public void SetTileGrid(List<List<IGridTile>> tileGrid)
    {
        _tileGrid = tileGrid;
    }
}