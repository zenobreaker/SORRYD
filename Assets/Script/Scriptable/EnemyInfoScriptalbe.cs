using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]   
public class EnemyInfo
{
    public string enemyID;
    public int appearCount;
}

[CreateAssetMenu(fileName = "Unit Data", menuName = "Scriptable Object/Enemy Data", order = int.MaxValue)]
public class EnemyInfoScriptalbe : ScriptableObject
{
    public List<EnemyInfo> enemies  = new List<EnemyInfo>(); 
}
