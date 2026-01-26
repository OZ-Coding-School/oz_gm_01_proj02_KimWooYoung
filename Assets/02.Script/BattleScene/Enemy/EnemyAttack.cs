using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private GridManager gridManager;

    private Health selfHp;

    private void Awake()
    {
        selfHp = GetComponent<Health>();
        if (selfHp == null)
            selfHp = GetComponentInParent<Health>();
    }

    public bool CanAttack(Transform target)
    {
        return enemySO.attackType == AttackType.Melee
            ? IsInMeleeRange(target)
            : IsInRangedRange(target);
    }

    public void Attack(Transform target)
    {

        switch (enemySO.attackType)
        {
            case AttackType.Melee:
                TryMeleeAttack(target);
                break;

            case AttackType.Ranged:
                TryRangedAttack(target);
                break;
        }
    }

    void TryMeleeAttack(Transform target)
    {

        if (!IsInMeleeRange(target))
        {
            return;
        }

        DealDamage(target);
    }

    void TryRangedAttack(Transform target)
    {
        if (!IsInRangedRange(target)) return;
        DealDamage(target);
    }


    void DealDamage(Transform target)
    {
        Health targetHp = target.GetComponent<Health>();
        if (targetHp == null) return;

        int dmg = CalculateDamage();
        Animator targetAnim = target.GetComponent<Animator>();
        if(targetAnim != null)
        {
            targetAnim.SetTrigger("Hit");
        }

        targetHp.TakeDamage(dmg);
    }

    private int CalculateDamage()
    {
        int dmg = enemySO.Damage;

        if (enemySO.trait == EnemyTrait.Berserk && selfHp != null)
        {

            float ratio = (float)selfHp.CurrentHP / selfHp.MaxHP;
            if (ratio <= enemySO.berserkHpRatio)
            {
                dmg = Mathf.RoundToInt(dmg * enemySO.berserkDamageMul);
            }
        }

        return dmg;
    }

    bool IsInMeleeRange(Transform target)
    {
        Vector2Int self = gridManager.WorldToGrid(transform.position);
        Vector2Int tgt = gridManager.WorldToGrid(target.position);

        int dist = Mathf.Abs(self.x - tgt.x) + Mathf.Abs(self.y - tgt.y);


        return dist == 1; // 인접 타일
    }

    bool IsInRangedRange(Transform target)
    {
        Vector2Int self = gridManager.WorldToGrid(transform.position);
        Vector2Int tgt = gridManager.WorldToGrid(target.position);

        int dist = Mathf.Abs(self.x - tgt.x) + Mathf.Abs(self.y - tgt.y);

        return dist <= enemySO.attackRange; // 예: 3칸
    }

}
