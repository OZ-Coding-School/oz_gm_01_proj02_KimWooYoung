using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerUnit playerUnit;
    [SerializeField] private GridManager gridManager;

    public bool IsAttacking { get; private set; }

    public bool TryAutoAttack()
    {
        if (IsAttacking) return false;

        Vector2Int playerGrid = gridManager.WorldToGrid(transform.position);

        Vector2Int[] dirs =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        Health[] allHealths = FindObjectsOfType<Health>();

        foreach (var dir in dirs)
        {
            Vector2Int checkGrid = playerGrid + dir;

            foreach (var hp in allHealths)
            {
                if (hp.isDead) continue;
                if (hp.gameObject == gameObject) continue;

                Vector2Int targetGrid =
                    gridManager.WorldToGrid(hp.transform.position);
                Debug.Log($"적 위치 그리드: {targetGrid}");


                if (targetGrid == checkGrid)
                {
                    Debug.Log("공격 대상 발견!");

                    Attack(hp);
                    return true;
                }
            }
        }
        return false;
    }

    private void Attack(Health target)
    {
        IsAttacking = true;
        Debug.Log(
      $"[ATTACK] Player → {target.gameObject.name} | Damage: {playerUnit.Data.Damage}"
  );
        target.TakeDamage(playerUnit.Data.Damage);

        IsAttacking = false;
    }
}
