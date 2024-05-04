using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ųʸ��� ����Ʈ��  �ش� Ÿ�Ժ� ���ֵ��� �����ϱ� ����
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

    // �� ��ȯ�� ���� ����Ʈ
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

        // TODO: ���� �Ǹ� ��, �ش��ϴ� �ݾ� �ޱ� 
    }


    // ���׷��̵� ������ ���� ���� ���� �ϱ� 
    public void ApplyUpgrade(UnitType type, int value)
    {
        if(units.TryGetValue(type, out PlayerUnitList unitList))
        {
            unitList.ApplyUpgrade(value);
        }
    }

    // Ÿ�Ժ� ���׷��̵� �� �������� 
    public int GetUpgradeCountWithType(UnitType type)
    {
        if (units.TryGetValue(type, out PlayerUnitList unitList))
        {
            return unitList.GetUpgradeCount();
        }

        return 0;
    }

}
