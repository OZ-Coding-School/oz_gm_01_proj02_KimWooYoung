using UnityEngine;

public class GridHighlight : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private Color hoverColor = Color.green;

    private Tile currentTile;
    private Color previousColor;
    public bool enableHover = true;

    private void Update()
    {
        if (!enableHover) return;
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
        {
            Vector2Int gridPos = gridManager.WorldToGrid(hit.point);
            Tile tile = gridManager.GetTile(gridPos);

            // 타일이 바뀌었을 때만 처리
            if (tile != currentTile)
            {
                // 이전 타일 복구
                if (currentTile != null)
                {
                    currentTile.SetColor(previousColor);
                }

                // 새 타일 하이라이트
                if (tile != null)
                {
                    previousColor = tile.GetCurrnetColor();
                    tile.SetColor(hoverColor);
                }

                currentTile = tile;
            }
        }
        else
        {
            // 그리드 밖으로 나갔을 때
            if (currentTile != null)
            {
                currentTile.SetColor(previousColor);
                currentTile = null;
            }
        }
    }
    public void DisableHover()
    {
        enableHover = false;
        if (currentTile != null) currentTile = null;
    }
}
