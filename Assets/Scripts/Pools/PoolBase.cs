using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBase : MonoBehaviour
{
    [SerializeField] protected GameObject prefab;

    protected Queue<GameObject> prefabQueue = new Queue<GameObject>();

    [SerializeField] protected int countOfrefabs = 10;
    public static PoolBase Instance { get; protected set; }



    protected virtual void Awake()
    {
        Instance = this;
        GrowPool();
    }

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
        prefabQueue.Enqueue(obj);
    }

    public virtual GameObject GetFromPool(Transform pos)
    {
        if (prefabQueue.Count == 0)
        {
            GrowPool();
        }
        var instance = prefabQueue.Dequeue();
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
        instance.SetActive(true);
        instance.transform.position = pos;
        return instance;
    }
}
