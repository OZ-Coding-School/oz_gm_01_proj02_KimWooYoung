using UnityEngine;
public enum EnemyTrait
{
    None,
    Berserk
}

public enum AttackType
{
    Melee,
    Ranged,
}
[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]

public class EnemySO : ScriptableObject
{
    [Header("기본 능력치")]
    public int maxHp;
    public int Damage;
    public int attackRange;

    [Header("Berserk")]
    public float berserkHpRatio = 0.3f; // 30% 이하
    public float berserkDamageMul = 1.5f;

    public AttackType attackType;

    public EnemyTrait trait;

}
