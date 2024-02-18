using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{

    [SerializeField] private float maxHealth;
    private float currentHealth;

    public float CurrentHealth  { get => currentHealth; private set => currentHealth = value; }
    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }



    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;

    public void TakeDamage(float damage, Vector3 position)
    {
        var damageTaken = Mathf.Clamp(damage, 0,currentHealth);
        currentHealth -= damageTaken;
        if (damageTaken != 0)
        {
            OnTakeDamage?.Invoke(damageTaken, position );
        }

        if (currentHealth == 0 && damageTaken != 0)
        {
            OnDeath?.Invoke(transform.position);
        }
    }

    void Awake()
    {
        currentHealth = maxHealth;
    }


}
