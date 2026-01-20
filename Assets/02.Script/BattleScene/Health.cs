using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<Health> onAnyDeath;

    private int currentHP;
    private int maxHP;
    public bool isDead = false;

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
        if (currentHP <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;

        onAnyDeath?.Invoke(this);

        gameObject.SetActive(false);
    }

    public void Heal(int heal)
    {
        if(isDead) return;

        currentHP += heal;
    }
}
