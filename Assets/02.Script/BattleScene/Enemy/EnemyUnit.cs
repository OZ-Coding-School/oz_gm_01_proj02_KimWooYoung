using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private BaseSO EnemySO;

    private Health health;

    public BaseSO Data => EnemySO;

    private void Awake()
    {
        health = GetComponent<Health>();
        health.Init(EnemySO.maxHp);

    }

    private void Update()
    {
        //Debug.Log($"적 현재 체력 : {health.CurrentHP}");

    }
}
