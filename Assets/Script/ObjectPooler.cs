using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public abstract class ObjectPoolInfo : MonoBehaviour
{
    public string idName;
}


public class ObjectPooler : Manager
{
    PoolManager poolManager;

    protected override void Awake()
    {
        base.Awake();
        poolManager = GetComponent<PoolManager>();
    }

    public override ObjectPoolInfo Spawn(string name)
    {
        return poolManager.GetFromPool<ObjectPoolInfo>(name);
    }

    public override void ReturnPool(ObjectPoolInfo clone)
    {
        poolManager.TakeToPool<ObjectPoolInfo>(clone.idName, clone);
    }

}
