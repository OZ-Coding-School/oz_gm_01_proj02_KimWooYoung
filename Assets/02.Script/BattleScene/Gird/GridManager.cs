using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Tile Prefabs")]
    public Tile flatTilePrefab;
    public Tile nomalTilePrefab;
    public Tile highTilePrefab;

    [Header("Grid Settings")]
    public float tileSize = 2f;
    public int gridwidth = 10;
    public int gridheight = 10;
    private float heightVisual = 0.31f;

    [Header("Random Spawn")]
    [Range(0, 100)] public int flatChance = 85;
    [Range(0, 100)] public int normalChance = 12;
    [Range(0, 100)] public int highChance = 3;

    public Tile[,] tiles;
    void Start()
    {
        GenerateGrid();
    }
    private void GenerateGrid()
    {
        tiles = new Tile[gridwidth, gridheight];

        for (int y = 0; y < gridheight; y++)
        {
            for (int x = 0; x < gridwidth; x++)
            {
                Tile prefab = GetRandomTilePrefab();

                Vector3 woldPos = new Vector3(x*tileSize, prefab.height * heightVisual, y*tileSize);

                Tile tile = Instantiate(prefab, woldPos, Quaternion.identity, transform);

                tile.gridPos = new Vector2Int(x, y);

                tiles[x, y] = tile;
            }
        }
    }
    private Tile GetRandomTilePrefab()
    {
        int roll = Random.Range(0, 100);
        int cumulative = 0;

        cumulative += flatChance;
        if (roll < cumulative) return flatTilePrefab;

        cumulative += normalChance;
        if (roll < cumulative) return nomalTilePrefab;

        return highTilePrefab;
    }
    public Tile GetTile(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= gridwidth || pos.y < 0 ||pos.y >= gridheight) return null;

        return tiles[pos.x, pos.y];
    }
    public bool CanMove(Vector2Int from, Vector2Int to)
    {
        Tile fromTile = GetTile(from);
        Tile toTile = GetTile(to);

        if(fromTile == null || toTile == null) return false;

        int heightDiff = toTile.height -fromTile.height;

        if(heightDiff> 1) return false;

        return true;
    }

    public void ResetAllTiles(Color color)
    {
        for (int y = 0; y < gridheight; y++) 
        {
            for(int x = 0; x < gridwidth; x++)
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
        x = Mathf.Clamp(x, 0, gridwidth-1);
        y = Mathf.Clamp(y, 0, gridheight-1); 

        return new Vector2Int(x, y);

    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        Tile tile = GetTile(gridPos);
        //벡터2 입력을 받아 그리드 공간 크기를 곱한
        float x = gridPos.x * tileSize;
        float z = gridPos.y * tileSize;

        float tileTopY = tile.transform.position.y + tile.transform.localScale.y * 0.5f;

        float playerHalfHeight = 0.5f;
        //벡터3로 반환
        return new Vector3(x, tileTopY + playerHalfHeight, z);
    }

    //private void OnValidate()
    //{
    //    int total = flatChance + normalChance + highChance;
    //    if (total != 100)
    //    {
    //        Debug.LogWarning($"Tile spawn chance total = {total} (should be 100)");
    //    }
    //}



}