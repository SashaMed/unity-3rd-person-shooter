using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public ParticlePoolBase HitParticlesPool { get => hitParticlesPool; private set => hitParticlesPool = value; }
    public ParticlePoolBase DamageablesHitParticlesPool { get => takeParticlesPool; private set => takeParticlesPool = value; }

    [SerializeField] private ParticlePoolBase hitParticlesPool;
    [SerializeField] private ParticlePoolBase takeParticlesPool;


    public static ParticlePool Instance { get; protected set; }

    protected void Awake()
    {
        Instance = this;
    }


    public void ReturnParticleToThePool(GameObject particle, string id)
    {
        if (id == HitParticlesPool.ParticlePoolId)
        {
            HitParticlesPool.AddToPool(particle);
        }
        else if (id == DamageablesHitParticlesPool.ParticlePoolId)
        {
            DamageablesHitParticlesPool.AddToPool(particle);
        }
        particle.SetActive(false);
    }
}
