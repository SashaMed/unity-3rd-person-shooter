using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    public float CurrentHealth { get; }
    public float MaxHealth { get; }

    public delegate void TakeDamageEvent(float damage, Vector3 position);
    public event TakeDamageEvent OnTakeDamage;

    public delegate void DeathEvent(Vector3 position);
    public event DeathEvent OnDeath;

    public void TakeDamage(float damage, Vector3 position);
}
