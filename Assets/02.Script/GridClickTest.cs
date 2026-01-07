using UnityEngine;

public class GridClickTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Debug.Log("Clicked Object: " + hit.collider.name);
                Debug.Log("World Pos: " + hit.point);
            }
        }
    }
}
