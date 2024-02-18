using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject[] deathParticle;
    [SerializeField] private float maxHealth;
    private float currentHealth;

    public float CurrentHealth { get => currentHealth; private set => currentHealth = value; }
    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }



    public event IDamageable.TakeDamageEvent OnTakeDamage;
    public event IDamageable.DeathEvent OnDeath;




    public void TakeDamage(float damage, Vector3 position)
    {
        var damageTaken = Mathf.Clamp(damage, 0, currentHealth);
        currentHealth -= damageTaken;
        if (damageTaken != 0)
        {
            LevelUIController.Instance.SetPlayerHealth(currentHealth / maxHealth, currentHealth.ToString());
            ParticlePool.Instance.DamageablesHitParticlesPool.GetFromPool(position);
            OnTakeDamage?.Invoke(damageTaken, position);
        }

        if (currentHealth == 0 && damageTaken != 0)
        {
            OnDeath?.Invoke(transform.position);
            StartCoroutine(Death());
        }
    }

    private IEnumerator Death()
    {
        for (int i = 0; i < deathParticle.Length; i++)
        {
            yield return new WaitForSeconds(0.3f);
            Instantiate(deathParticle[i], transform.position, transform.rotation, null);
        }
        Time.timeScale = 0;
        gameObject.SetActive(false);
    }


    void Start()
    {
        currentHealth = maxHealth;
        LevelUIController.Instance.SetPlayerHealth(currentHealth / maxHealth, currentHealth.ToString());
    }
}
