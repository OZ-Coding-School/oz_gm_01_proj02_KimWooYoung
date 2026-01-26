using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRangeVisualizer : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private BFSMoveRange bfsMoveRange;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Animator animator;

    [Header("PlayerMove")]
    [SerializeField] private int moveRange = 3;
    [SerializeField] private float moveSpeed = 5f;

    [Header("TileColor")]
    [SerializeField] private Color moveColor = Color.yellow;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private GridHighlight gridHighlight;

    private List<Vector2Int> currentMoveTiles = new List<Vector2Int>();
    private Vector2Int facingDir;
    private bool isMoving = false;
    
    public bool IsMoving => isMoving;

    private void Update()
    {
        if (isMoving) return;

        if (Input.GetMouseButtonDown(0)) 
        {
            TryMove();
        }
    }

    public void ShowMoveRage()
    {
        StartCoroutine(ShowMoveRangeRoutineCo());
    }
    
    public void TryMove()
    {
        if(isMoving) return;
        if (currentMoveTiles.Count ==0) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask)) return;

        Vector2Int targetGrid = gridManager.WorldToGrid(hit.point);

        if(!currentMoveTiles.Contains(targetGrid)) return;

        Vector3 targetWorld = gridManager.GridToWorld(targetGrid);

        StartCoroutine(MoveToCo(targetWorld));
    }

    private IEnumerator MoveToCo(Vector3 targetWorld)
    {
        isMoving = true;
        animator.SetBool("IsMoving", true);

        Vector2Int currentGrid = gridManager.WorldToGrid(transform.position);
        Vector2Int targetGrid = gridManager.WorldToGrid(targetWorld);

        while (currentGrid != targetGrid)
        {
            Vector2Int nextGrid = currentGrid;

            // X 먼저 이동
            if (targetGrid.x != currentGrid.x)
            {
                nextGrid.x += targetGrid.x > currentGrid.x ? 1 : -1;
                facingDir = targetGrid.x > currentGrid.x ? Vector2Int.right : Vector2Int.left;
            }
            // X가 같으면 Z 이동
            else if (targetGrid.y != currentGrid.y)
            {
                nextGrid.y += targetGrid.y > currentGrid.y ? 1 : -1;
                facingDir = targetGrid.y > currentGrid.y ? Vector2Int.up : Vector2Int.down;
            }

            Vector3 nextWorld = gridManager.GridToWorld(nextGrid);

            // 한 칸 이동
            yield return MoveDirectiontCo(nextWorld);

            currentGrid = nextGrid;
        }


        currentMoveTiles.Clear();
        gridManager.ResetAllTiles(defaultColor);
        transform.position = gridManager.GridToWorld(currentGrid);
        isMoving = false;
        animator.SetBool("IsMoving", false);

    }


    private IEnumerator ShowMoveRangeRoutineCo()
    {
        gridHighlight.DisableHover();

        gridManager.ResetAllTiles(defaultColor);

        Vector2Int playerGridPos = gridManager.WorldToGrid(transform.position);

        currentMoveTiles = bfsMoveRange.GetMoveRange(playerGridPos, moveRange);

        gridManager.HighlightTiles(currentMoveTiles, moveColor);

        yield return null;

        gridHighlight.enableHover = true;
    }

    private IEnumerator MoveDirectiontCo(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            Direction();

            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
    private void Direction()
    {
        Vector3 dir = Vector3.zero;

        if (facingDir == Vector2Int.right) dir = Vector3.right;
        if (facingDir == Vector2Int.left) dir = Vector3.left;
        if (facingDir == Vector2Int.up) dir = Vector3.forward;
        if (facingDir == Vector2Int.down) dir = Vector3.back;

        if (dir == Vector3.zero) return;
        Quaternion targetRot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            360f * Time.deltaTime);
    }

}
