using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]   
public class EnemyInfo
{
    public string enemyID;
    public int appearStage;
    public int appearCount;
    public bool isBossStage;
}

[CreateAssetMenu(fileName = "Unit Data", menuName = "Scriptable Object/Enemy Data", order = int.MaxValue)]
public class EnemyInfoScriptable : ScriptableObject
{
    public List<EnemyInfo> enemies  = new List<EnemyInfo>(); 

    public bool GetIsBossStage(int round)
    {
        return enemies.Find(x => x.appearStage == round).isBossStage;
    }
}
