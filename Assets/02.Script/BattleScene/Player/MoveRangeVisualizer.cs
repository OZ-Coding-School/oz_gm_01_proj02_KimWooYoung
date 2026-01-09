using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRangeVisualizer : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private BFSMoveRange bfsMoveRange;
    [SerializeField] private LayerMask groundMask;

    [Header("PlayerMove")]
    [SerializeField] private int moveRange = 3;
    [SerializeField] private float moveSpeed = 5f;

    [Header("TileColor")]
    [SerializeField] private Color moveColor = Color.yellow;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private GridHighlight gridHighlight;

    private List<Vector2Int> currentMoveTiles = new List<Vector2Int>();
    private bool isMoving = false;

    private void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowMoveRage();
        }
        if (Input.GetMouseButtonDown(0)) 
        {
            TryMove();
        }
    }

    private void ShowMoveRage()
    {
        StartCoroutine(ShowMoveRangeRoutine());
    }

    private void TryMove()
    {
        if(isMoving) return;
        if (currentMoveTiles.Count ==0) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask)) return;

        Vector2Int targetGrid = gridManager.WorldToGrid(hit.point);

        if(!currentMoveTiles.Contains(targetGrid)) return;

        Vector3 targetWorld = gridManager.GridToWorld(targetGrid);

        StartCoroutine(MoveTo(targetWorld));
    }

    IEnumerator MoveTo(Vector3 target)
    {
        isMoving = true;


        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
        currentMoveTiles.Clear();
        isMoving = false;

        gridManager.ResetAllTiles(defaultColor);
    }

    IEnumerator ShowMoveRangeRoutine()
    {
        gridHighlight.DisableHover();

        gridManager.ResetAllTiles(defaultColor);

        Vector2Int playerGridPos = gridManager.WorldToGrid(transform.position);

        currentMoveTiles = bfsMoveRange.GetMoveRange(playerGridPos, moveRange);

        gridManager.HighlightTiles(currentMoveTiles, moveColor);

        yield return null;

        gridHighlight.enableHover = true;


    }

}
