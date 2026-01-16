using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    [SerializeField] private BaseSO playerSO;

    private Health health;

    public BaseSO Data => playerSO;

    private void Awake()
    {
        health = GetComponent<Health>();
        health.Init(playerSO.maxHp);

    }

    private void Update()
    {
        //Debug.Log($"플레이어 현재 체력 : {health.CurrentHP}");

    }

}
