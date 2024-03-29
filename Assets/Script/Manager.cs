using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }

    protected virtual void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public virtual ObjectPoolInfo Spawn(string name)
    {
        return null;
    }

    public virtual void ReturnPool(ObjectPoolInfo clone)
    {

    }

}
