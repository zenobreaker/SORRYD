using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 딕셔너리와 리스트로  해당 타입별 유닛들을 관리하기 위함
[System.Serializable]
public class PlayerUnitList
{
    public int upgradeCount;
    private List<PlayerUnit> list;

    public PlayerUnitList()
    {
        list = new List<PlayerUnit>();
    }

    public void AddUnit(PlayerUnit unit)
    {
        list.Add(unit);
    }

    public void RemoveUnit(PlayerUnit unit)
    {
        list.Remove(unit);
    }

    public void ApplyUpgrade(int value)
    {
        upgradeCount = value; 

        foreach(var info in list)
        {
            if (info == null)
                continue;
            info.upgradeCount = upgradeCount;
        }
    }

    public int GetUpgradeCount()
    {
        return upgradeCount;
    }
}

public class UnitManager : MonoBehaviour
{

    public static UnitManager instance;

    // 내 소환한 유닛 리스트
    public Dictionary<UnitType, PlayerUnitList> units = new();

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void Start()
    {
        units.Add(UnitType.EXPLOSIVE, new PlayerUnitList());
        units.Add(UnitType.PIERCE, new PlayerUnitList());
        units.Add(UnitType.NORMAL, new PlayerUnitList());
    }

    public void AddMyUnit(PlayerUnit unit)
    {
        if (units.TryGetValue(unit.unitInfo.unitType, out PlayerUnitList unitList))
        {
            unitList.AddUnit(unit);
        }
    }


    public void SellUnit(PlayerUnit unit)
    {
        if (units.TryGetValue(unit.unitInfo.unitType, out PlayerUnitList unitList))
        {
            unitList.RemoveUnit(unit);
        }

        // TODO: 유닛 판매 시, 해당하는 금액 받기 
    }


    // 업그레이드 관련한 정보 전부 적용 하기 
    public void ApplyUpgrade(UnitType type, int value)
    {
        if(units.TryGetValue(type, out PlayerUnitList unitList))
        {
            unitList.ApplyUpgrade(value);
        }
    }

    // 타입별 업그레이드 수 가져오기 
    public int GetUpgradeCountWithType(UnitType type)
    {
        if (units.TryGetValue(type, out PlayerUnitList unitList))
        {
            return unitList.GetUpgradeCount();
        }

        return 0;
    }

}
