using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Tile Prefabs")]
    public Tile flatTilePrefab;
    public Tile normalTilePrefab;
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

    [Header("Spawn Positions")]
    public List<Transform> playerSpawnObjects;
    public List<Transform> enemySpawnObjects;

    //오브젝트 스폰 위치 저장
    private HashSet<Vector2Int> spawnGridSet = new HashSet<Vector2Int>();

    private HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();



    public Tile[,] tiles;

    private void Awake()
    {
        CacheSpawnGrids();
        GenerateGrid();

    }
    private void GenerateGrid()
    {
        tiles = new Tile[gridwidth, gridheight];

        for (int y = 0; y < gridheight; y++)
        {
            for (int x = 0; x < gridwidth; x++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);

                Tile prefab;

                if (IsSpawnPosition(gridPos))
                {
                    prefab = flatTilePrefab;
                }
                else
                {
                    prefab = GetRandomTilePrefab();
                }
                //월드좌표 계산
                Vector3 woldPos = new Vector3
                    (x*tileSize, prefab.height * heightVisual, y*tileSize);
                //타일 생성
                Tile tile = Instantiate
                    (prefab, woldPos, Quaternion.identity, transform);

                tiles[x, y] = tile;
            }
        }
    }
    //player enemy 위치 저장
    private void CacheSpawnGrids()
    {
        spawnGridSet.Clear();

        foreach(var tile in playerSpawnObjects)
        {
            if(tile == null) continue;   
            spawnGridSet.Add(WorldToGrid(tile.position));
        }
        foreach(var tile in enemySpawnObjects)
        {
            if(tile == null) continue;
            spawnGridSet.Add(WorldToGrid(tile.position));
        }
    }

    //spawnGridSet에 pos가 해당되는지 확인
    private bool IsSpawnPosition(Vector2Int pos)
    {
       return spawnGridSet.Contains(pos);
    }

    //가중치에 따른 그리드 생성
    private Tile GetRandomTilePrefab()
    {
        int roll = Random.Range(0, 100);
        int cumulative = 0;

        cumulative += flatChance;
        if (roll < cumulative) return flatTilePrefab;

        cumulative += normalChance;
        if (roll < cumulative) return normalTilePrefab;

        return highTilePrefab;
    }


    //이동 가능 판단
    public bool CanMove(Vector2Int from, Vector2Int to)
    {
        Tile fromTile = GetTile(from);
        Tile toTile = GetTile(to);

        if(fromTile == null || toTile == null) return false;

        if(IsOccupied(to)) return false;

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

    public Tile GetTile(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= gridwidth || pos.y < 0 || pos.y >= gridheight) return null;

        return tiles[pos.x, pos.y];
    }

    public bool IsOccupied(Vector2Int pos)
    {
        return occupied.Contains(pos);
    }

    public void SetOccupied(Vector2Int pos)
    {
        occupied.Add(pos);
    }

    public void ClearOccupied(Vector2Int pos)
    {
        occupied.Remove(pos);
    }
}