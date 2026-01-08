using System.Collections.Generic;
using UnityEngine;

public class BFSMoveRange : MonoBehaviour
{
    GridManager gridManager;

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }
    
    //시작 start 이동가능거리 moveRange
    public List<Vector2Int> GetMoveRange(Vector2Int start, int moveRange)
    {
        //이미 방문한 타일저장(중복방지)
        var visited = new HashSet<Vector2Int>();
        //타일의 위치와 이동비용 저장
        var cost = new Dictionary<Vector2Int, int>();
        //타일 위치 저장
        var queue = new Queue<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);
        cost[start] = 0;

        Vector2Int[] dirs =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right,
        };

        while(queue.Count > 0)
        {
            var current = queue.Dequeue();
            foreach(var dir in dirs)
            {
                var next = current + dir;

                if(!IsValid(next) || visited.Contains(next)) continue;

                int nextCost = cost[current] + 1;

                if(nextCost > moveRange) continue;

                queue.Enqueue(next);
                visited.Add(next);
                cost[next] = nextCost;
            }
        }
        //시작점은 제외
        visited.Remove(start);

        return new List<Vector2Int>(visited);
    }
    //그리드 범위 벗어날시 x
    bool IsValid(Vector2Int pos)
    {
        if(pos.x < 0 || pos.x >= gridManager.width) return false;
        if(pos.y < 0 || pos.y >= gridManager.height) return false;

        Tile tile = gridManager.GetTile(pos);
        if(tile == null) return false;

        return tile.walkable;
    }
}
