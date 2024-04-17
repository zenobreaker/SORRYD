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

    public ObjectPoolInfo CheatSpawn(string name)
    {
        //return poolManager.GetFromPool<ObjectPoolInfo>(name);

        Debug.Log($"��ȯ ��� {name}");

        var obj = poolManager.GetFromPool<ObjectPoolInfo>(name);

        if (obj == null)
        {
            Debug.Log("��ȯ ���� : ������ ����");
            return null;
        }

        return obj;
    }

    public override ObjectPoolInfo Spawn(string name)
    {
        //return poolManager.GetFromPool<ObjectPoolInfo>(name);

        Debug.Log($"��ȯ ��� {name}");

        var obj = poolManager.GetFromPool<ObjectPoolInfo>(name);
        
        if(obj == null)
        {
            Debug.Log("��ȯ ���� : ������ ����"); 
            return null; 
        }

        return obj;
    }

    public override void ReturnPool(ObjectPoolInfo clone)
    {
        poolManager.TakeToPool<ObjectPoolInfo>(clone.idName, clone);
    }

}
