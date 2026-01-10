using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AStarPathfinder : MonoBehaviour
{
    private GridManager gridManager;

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }
    public List<Vector2Int> GetAStarPath(Vector2Int start, Vector2Int end)
    {
        //탐색 후보 그리드 저장
        var openSet = new PriorityQueue<Vector2Int>();

        //이동 경로 복원
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        var gScore = new Dictionary<Vector2Int, int>();
        var fScore = new Dictionary<Vector2Int, int>();

        openSet.Enqueue(start, 0);
        gScore[start] = 0;
        fScore[start] = Heuristic(start, end);

        // 확정적으로 검사 끝난 그리드
        var closedSet = new HashSet<Vector2Int>();

        Vector2Int[] dirs =
        {
            Vector2Int.down,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.right
        };

        while (openSet.Count > 0)
        {

            var current = openSet.Dequeue();

            if (current == end)
            {

                return ReconstructPath(cameFrom, end);
            }

            closedSet.Add(current);

            foreach (var dir in dirs)
            {
                var neighbor = current + dir;
                if (!IsValid(neighbor) || closedSet.Contains(neighbor)
                    || !gridManager.CanMove(current, neighbor))
                {
                    continue;
                }

                Tile currentTile = gridManager.GetTile(current);
                Tile nextTile = gridManager.GetTile(neighbor);

                int heightDiff = nextTile.height - currentTile.height;

                int moveCost = 1;
                if (heightDiff > 0)
                {
                    moveCost += heightDiff;
                }

                int tentativeG = gScore[current] + moveCost;

                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, end);

                    openSet.Enqueue(neighbor, fScore[neighbor]);
                }
            }
        }
        return null;
    }

    private int Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private bool IsValid(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= gridManager.gridwidth) return false;
        if (pos.y < 0 || pos.y >= gridManager.gridheight) return false;

        Tile tile = gridManager.GetTile(pos);
        if (tile == null) return false;

        return tile.walkable;
    }

    //경로를 재구성 하는 메서드
    //cameFrom정보와 end위치를 가지고
    //start -> end로 이어지는 실제 경로 리스트를 만든다.
    List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int end)
    {
        //최종 경로를 담을 리스트
        List<Vector2Int> path = new List<Vector2Int>();

        //end에서 시작해서 역으로 start까지 올라감
        var current = end;

        //cameFrom에 Current 키가 존재하는 동안 반복
        while (cameFrom.ContainsKey(current))
        {
            //현재 위치를 경로에 추가
            path.Add(current);
            //한 단계 이전 위치로 이동
            current = cameFrom[current];
        }
        //마지막으로 시작위치도 경로에 포함
        path.Add(current);

        path.Reverse();

        return path;
    }
}
