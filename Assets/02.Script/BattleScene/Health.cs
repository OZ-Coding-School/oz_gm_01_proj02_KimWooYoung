using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<Health> onAnyDeath;
    public event Action OnHit;

    private int currentHP;
    private int maxHP;
    public bool isDead = false;
    public bool isPlayer;


    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    public void Init(int maxHp)
    {
        maxHP = maxHp;
        currentHP = maxHP;
        isDead = false;
    }
    public void TakeDamage(int damage)
    {
        if(isDead) return;  

        currentHP -= damage;
        OnHit?.Invoke();

        if (currentHP <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        onAnyDeath?.Invoke(this);
        if (isPlayer)
        {
            TurnManager tm = FindObjectOfType<TurnManager>();
            if (tm != null)
                tm.SetState(TurnState.Lose);
        }
        gameObject.SetActive(false);

    }

    public void Heal(int heal)
    {
        if(isDead) return;

        currentHP += heal;
    }
}
