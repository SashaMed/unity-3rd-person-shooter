using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemAutoReturnToPool : MonoBehaviour
{
    private ParticleSystem particle;
    [SerializeField] private float awaitTime = 1f;
    [SerializeField] private string particlePoolId = "null";


    void Start()
    {
        particle = GetComponent<ParticleSystem>();

    }


    public void OnParticleSystemStopped()
    {

        ReturnToPool();
    }




    public void ReturnToPool()
    {
        if (particlePoolId == "null")
        {
            gameObject.SetActive(false);
        }
        else
        {
            ParticlePool.Instance.ReturnParticleToThePool(gameObject, particlePoolId);
        }
    }
}
