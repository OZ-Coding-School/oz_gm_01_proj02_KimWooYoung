using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    [SerializeField] private BaseSO playerSO;
    [SerializeField] private Animator animator;

    private Health health;

    public BaseSO Data => playerSO;

    private void Awake()
    {
        health = GetComponent<Health>();
        health.Init(playerSO.maxHp);

        health.OnHit += PlayHit;
    }

    private void PlayHit()
    {
        animator.SetTrigger("Hit");
    }

    private void Update()
    {
        Debug.Log($"플레이어 현재 체력 : {health.CurrentHP}");

    }

}
