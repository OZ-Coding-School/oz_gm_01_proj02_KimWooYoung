using UnityEngine;

public class PlacementSysyem : MonoBehaviour
{
    [SerializeField] GameObject mouseIndicator;
    [SerializeField]
    private InputManager inputManager;

    private void Update()
    {
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        mouseIndicator.transform.position = mousePosition;

    }
}
