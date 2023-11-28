using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

[Serializable]
public struct PoolInfo<T> where T : BasicPoolObj
{
    public T prefab;
    public int size;
}

public class Pool<T> where T : BasicPoolObj
{
    public PoolInfo<T> mPoolInfo;    
    public T Prefab => mPoolInfo.prefab;

    [SerializeField] List<T> mList_Output;
    [SerializeField] Queue<T> queue;
    [SerializeField] Transform parent;
    [SerializeField] Vector3 localScale;

    public void Init(PoolInfo<T> poolInfo, Transform transform)
    {
        mPoolInfo = poolInfo;
        this.parent = transform;//从外界传入到内部
        queue = new Queue<T>();
        mList_Output = new List<T>();
        localScale = Prefab.transform.localScale;
        for (int i = 0; i < mPoolInfo.size; i++)
        {
            queue.Enqueue(Copy());
        }
    }
    T Copy()
    {
        T copy = GameObject.Instantiate<T>(Prefab, parent);
        return copy;
    }

    T GetAvailableObject()
    {
        T obj = null;
        if (queue.Count > 0 && !queue.Peek().IsActive)
        {
            obj = queue.Dequeue();//从对象池中弹出
        }
        else
        {
            obj = Copy();
        }
        obj.gameObject.SetActive(false);
        obj.transform.localScale = localScale;
        mList_Output.Add(obj);
        return obj;
    }

    public void Recycle(T obj)
    {
        if (mList_Output.Contains(obj))
        {
            mList_Output.Remove(obj);
        }
        obj.SetActive(false);
        queue.Enqueue(obj);
    }

    public void HideAll(bool Hide)
    {
        foreach (var obj in mList_Output)
        {
            obj.SetActive(false);
            queue.Enqueue(obj);
        }
        mList_Output.Clear();
    }

    #region GetPreparedObject 重载
    public T GetPreparedObject()
    {
        T obj = GetAvailableObject();
        obj.SetActive(true);
        return obj;
    }
    public T GetPreparedObject(Vector3 position)
    {
        T obj = GetAvailableObject();
        obj.SetActive(true);
        obj.transform.position = position;
        return obj;
    }
    public T GetPreparedObject(Vector3 position, Quaternion rotation)
    {
        T obj = GetAvailableObject();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }
    public T GetPreparedObject(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        T obj = GetAvailableObject();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.localScale = scale;
        return obj;
    }
    #endregion
}
