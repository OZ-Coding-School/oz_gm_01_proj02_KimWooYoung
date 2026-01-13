using UnityEngine;

[CreateAssetMenu(fileName ="BaseData", menuName = "Data/BaseData")]

public class BaseSO : ScriptableObject
{
    [Header("기본 능력치")]
    public int maxHp;
    public int Damage;
    public int attackRange;
}
