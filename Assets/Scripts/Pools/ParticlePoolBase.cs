using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticlePoolBase : MonoBehaviour
{
    public string ParticlePoolId { get => transform.name; }
    [SerializeField] protected GameObject prefab;

    protected Queue<GameObject> prefabQueue = new Queue<GameObject>();

    [SerializeField] protected int countOfrefabs = 10;
    private ObjectPool<GameObject> prefabPool;


    protected virtual void Awake()
    {
        //prefabPool = new ObjectPool<GameObject>(GetPrefab);
        GrowPool();
    }

    private GameObject GetPrefab() { return prefab; }

    protected virtual void GrowPool()
    {
        for (int i = 0; i < countOfrefabs; i++)
        {
            var instanceToAdd = Instantiate(prefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public virtual void AddToPool(GameObject obj)
    {
        obj.SetActive(false);
        //prefabPool.Release(obj);
        prefabQueue.Enqueue(obj);
    }

    public virtual GameObject GetFromPool(Transform pos)
    {
        if (prefabQueue.Count == 0)
        {
            GrowPool();
        }
        var instance = prefabQueue.Dequeue();
        //var instance = prefabPool.Get();
        instance.SetActive(true);
        instance.transform.position = pos.position;
        return instance;
    }

    public virtual GameObject GetFromPool(Vector3 pos)
    {
        if (prefabQueue.Count == 0)
        {
            GrowPool();
        }
        var instance = prefabQueue.Dequeue();
        //var instance = prefabPool.Get();
        instance.SetActive(true);
        instance.transform.position = pos;
        return instance;
    }
}
