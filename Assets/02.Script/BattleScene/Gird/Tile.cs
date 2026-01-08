using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPos;
    public bool walkable = true;
    public bool occupied = false;

    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();   
    }
    public void SetColor(Color color)
    {
        rend.material.color = color;
    }
    public Color GetCurrnetColor()
    {
        return rend.material.color;
    }
}
