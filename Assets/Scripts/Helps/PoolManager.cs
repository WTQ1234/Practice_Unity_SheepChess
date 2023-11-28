using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class PoolManager : MonoSingleton<PoolManager>
{
    [SerializeField]
    List<PoolInfo<BasicPoolObj>> playerPools;
    [ShowInInspector]
    Dictionary<string, Pool<BasicPoolObj>> dictionary;

    protected void Awake()
    {
        dictionary = new Dictionary<string, Pool<BasicPoolObj>>();
        Initialize(playerPools);
    }

    void Initialize(List<PoolInfo<BasicPoolObj>> poolInfos)
    {
        foreach(var poolInfo in poolInfos)
        {
            var obj = poolInfo.prefab;
            string name = obj.GetType().Name;
            if (!dictionary.ContainsKey(name))
            {
                GameObject temp = new GameObject("Pool: " + poolInfo.prefab.name);//新建一个空对象，用来存放一个池中的所有对象
                temp.transform.parent = transform;
                var pool = CreatePool(poolInfo.prefab);
                pool.Init(poolInfo, temp.transform);
                dictionary.Add(name, pool);
            }
        }
    }
    public Pool<T> CreatePool<T>(T instance) where T : BasicPoolObj
    {
        return new Pool<T>();
    }

    #region Release
    public T Release<T>(T prefab) where T : BasicPoolObj
    {
        string name = prefab.GetType().Name;
        if (dictionary.TryGetValue(name, out var pool))
        {
            return (T)pool.GetPreparedObject();
        }
        return null;
    }
    public T Release<T>(Vector3 position) where T : BasicPoolObj
    {
        string name = typeof(T).Name;
        if (dictionary.TryGetValue(name, out var pool))
        {
            return (T)pool.GetPreparedObject(position);
        }
        return null;
    }
    public T Release<T>(Vector3 position, Quaternion rotation) where T : BasicPoolObj
    {
        string name = typeof(T).Name;
        if (dictionary.TryGetValue(name, out var pool))
        {
            return (T)pool.GetPreparedObject(position, rotation);
        }
        return null;
    }
    public T Release<T>(BasicPoolObj prefab, Vector3 position, Quaternion rotation, Vector3 scale) where T : BasicPoolObj
    {
        string name = typeof(T).Name;
        if (dictionary.TryGetValue(name, out var pool))
        {
            return (T)pool.GetPreparedObject(position, rotation, scale);
        }
        return null;
    }
    #endregion

    public void HideAll<T>(bool hide=true)
    {
        string name = typeof(T).Name;
        if (dictionary.TryGetValue(name, out var pool))
        {
            pool.HideAll(hide);
        }
    }

    public void Recycle<T>(T obj) where T : BasicPoolObj
    {
        string name = obj.GetType().Name;
        if (dictionary.TryGetValue(name, out var pool))
        {
            pool.Recycle(obj);
        }
    }
}
