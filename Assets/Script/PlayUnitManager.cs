using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public enum UnitType
{
    EXPLOSIVE,
    PIERCE,
    NORMAL,
}

public enum UnitGrade
{
    COMMON,
    RARE,
    UNIQUE,
    LEGEND,
    MYTH,
}

[System.Serializable]
public class UnitUpgradeInfo
{
    public UnitType type;
    public int upgrade;
    [Header("강화 비용")]
    public int upgradeCost;
    public UnitUpgradeInfo(UnitType value1, int value2)
    {
        this.type = value1;
        this.upgrade = value2;
    }
}



public class PlayUnitManager : MonoBehaviour
{
    public static PlayUnitManager instance;

    public UnitGradeScriptable gradeScriptable;

    Dictionary<UnitType, UnitUpgradeInfo> unitUpgardeDict;

    private void Awake()
    {
        if(instance == null)    
            instance = this;

        // 데이터 사전에 생성
        unitUpgardeDict = new Dictionary<UnitType, UnitUpgradeInfo>();
        unitUpgardeDict.Add(UnitType.EXPLOSIVE, new UnitUpgradeInfo(UnitType.EXPLOSIVE, 0));
        unitUpgardeDict.Add(UnitType.PIERCE, new UnitUpgradeInfo(UnitType.PIERCE, 0));
        unitUpgardeDict.Add(UnitType.NORMAL, new UnitUpgradeInfo(UnitType.NORMAL, 0));
    }

    public UnitUpgradeInfo GetUpgradeInfo(UnitType type)
    {
        var info = unitUpgardeDict[type];
        return info;
    }

    public void SetUgpradeCount(UnitType type)
    {
        var info = unitUpgardeDict[type];
        if(info != null)
        {
            if (info.upgrade < gradeScriptable.maxUpgrade)
                info.upgrade += 1;
        }
    }
    
    public int GetUpgradeCount(UnitType type)
    {
        var info = unitUpgardeDict[type];
        return info.upgrade;
    }
}
