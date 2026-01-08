using UnityEngine;

public class GridClickTest : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Transform indicator; // 구체 / 큐브 등
    [SerializeField] LayerMask groundMask;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
            {
                Vector3 worldPos = hit.point;

                //  World → Grid
                Vector2Int gridPos = gridManager.WorldToGrid(worldPos);

                // Grid → World (타일 중앙)
                Vector3 centerPos = gridManager.GridToWorld(gridPos);

                // 결과 표시
                indicator.position = centerPos;

                Debug.Log(
                    $"World: {worldPos} → Grid: {gridPos} → Center: {centerPos}"
                );
            }
        }
    }
}
