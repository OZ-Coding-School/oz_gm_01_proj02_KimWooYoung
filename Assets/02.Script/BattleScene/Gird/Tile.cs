using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walkable = true;
    public int height;

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
