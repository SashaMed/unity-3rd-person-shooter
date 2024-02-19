using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticlePoolBase : PoolBase
{
    public string ParticlePoolId { get => transform.name; }
}
