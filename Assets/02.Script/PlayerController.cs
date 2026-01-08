using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GridManager gridManager;
    public BFSMoveRange bFSMoveRange;
    public int moveRange = 3;

    Vector2Int currentGridPos;

    private void Start()
    {
        currentGridPos = gridManager.WorldToGrid(transform.position);
    }
}
