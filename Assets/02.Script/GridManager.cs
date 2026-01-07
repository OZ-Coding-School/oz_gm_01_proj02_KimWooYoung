using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Tile tilePrefab;
    float tileSize = 2f;

    public int width = 10;
    public int height = 10;

    public Tile[,] tiles;
    void Start()
    {
        GenerateGrid();
    }
    void GenerateGrid()
    {
        tiles = new Tile[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Tile tile = Instantiate(tilePrefab, new Vector3(x*tileSize, 0, y*tileSize), Quaternion.identity, transform);

                tile.gridPos = new Vector2Int(x, y);
                tile.walkable = true;
                tile.SetColor(Color.white);

                tiles[x, y] = tile;

            }
        }
    }
    public Tile GetTile(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= width || pos.y < 0 ||pos.y >= height) return null;

        return tiles[pos.x, pos.y];
    }
}
