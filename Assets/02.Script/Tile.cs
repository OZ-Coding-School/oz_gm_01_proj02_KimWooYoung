using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPos;
    public bool walkable = true;
    public bool occupied = false;

    public void SetColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
}
