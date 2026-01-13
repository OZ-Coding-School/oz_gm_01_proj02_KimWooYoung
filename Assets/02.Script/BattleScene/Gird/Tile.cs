using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool walkable = true;
    public int height;

    private int unitCount = 0;

    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy")) return;

        bool isMoving = false;

        var playerMove = other.GetComponent<MoveRangeVisualizer>();
        if(playerMove != null)
        {
            isMoving = playerMove.IsMoving;
        }
        var enemyFSM = other.GetComponent<EnemyFSM>();
        if (enemyFSM != null) 
        {
            isMoving = enemyFSM.IsMoving;
        }
        if (isMoving) return;

            unitCount++;
            walkable = false;
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            unitCount--;
            if(unitCount <= 0)
            {
                unitCount = 0;
                walkable = true;
            }
        }
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
