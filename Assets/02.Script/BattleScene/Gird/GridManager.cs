using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Tile tilePrefab;
    public float tileSize = 2f;

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

    public void ResetAllTiles(Color color)
    {
        for (int y = 0; y < height; y++) 
        {
            for(int x = 0; x < width; x++)
            {
                tiles[x,y].SetColor(color);
            }
        }
    }
    public void HighlightTiles(List<Vector2Int> positions, Color color)
    {
        foreach(var pos in positions)
        {
            Tile tile = GetTile(pos);
            if(tile != null)
            {
                tile.SetColor(color);
            }
        }
    }

    //월드 좌표에서 그리드 좌표로 변환
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        //벡터를 입력받아 그리드 위치로 변환
        int x = Mathf.FloorToInt(worldPos.x / tileSize);
        int y = Mathf.FloorToInt(worldPos.z / tileSize);

        //x축과 y축 값을 제한
        x = Mathf.Clamp(x, 0, width);
        y = Mathf.Clamp(y, 0, height); 

        return new Vector2Int(x, y);

    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        //벡터2 입력을 받아 그리드 공간 크기를 곱한
        float x = gridPos.x * tileSize;
        float z = gridPos.y * tileSize;

        //벡터3로 반환
        return new Vector3(x, 0.5f, z);
    }

}