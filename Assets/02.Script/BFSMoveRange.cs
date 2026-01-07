using System.Collections.Generic;
using UnityEngine;

public class BFSMoveRange : MonoBehaviour
{
    GridManager gridManager;

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }
    public List<Vector2Int> GetMoveRange(Vector2Int start, int moveRange)
    {
        var visited = new HashSet<Vector2Int>();
        var cost = new Dictionary<Vector2Int, int>();
        var quene = new Queue<Vector2Int>();

        quene.Enqueue(start);
        visited.Add(start);
        cost[start] = 0;

        Vector2Int[] dirs =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right,
        };

        while(quene.Count > 0)
        {
            var current = quene.Dequeue();
            foreach(var dir in dirs)
            {
                var next = current + dir;

                if(!IsValid(next) || visited.Contains(next)) continue;

                int nextCost = cost[current] + 1;

                if(nextCost > moveRange) continue;

                quene.Enqueue(next);
                visited.Add(next);
                cost[next] = nextCost;
            }
        }
        visited.Remove(start);

        return new List<Vector2Int>(visited);
    }
    bool IsValid(Vector2Int pos)
    {
        if(pos.x < 0 || pos.x >= gridManager.width) return false;
        if(pos.y < 0 || pos.y >= gridManager.height) return false;

        Tile tile = gridManager.GetTile(pos);
        if(tile == null) return false;

        return tile.walkable;
    }
}
