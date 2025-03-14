using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

    public int GetCount()
    {
        return list.Count;
    }

    public void ApplyUpgrade(int value)
    {
        upgradeCount = value;

        foreach (var info in list)
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

    public PlayerUnit GetUnit(string id)
    {
        PlayerUnit target = null;

        foreach (var unit in list)
        {
            if (unit.idName.Equals(id))
                target = unit;
        }

        return target;
    }

    public IEnumerable<PlayerUnit> GetUnits()
    {
        return list;
    }
}

// �÷��̾� ������ �Ѳ����� �ٷ�� ���� �׷� Ŭ����
public class PlayerUnitGroup
{
    List<PlayerUnit> units = new List<PlayerUnit>();

    public void AddUnit(PlayerUnit unit)
    {
        if (!units.Contains(unit))
            units.Add(unit);
    }

    public void RemoveUnit(PlayerUnit unit)
    {
        units.Remove(unit);
    }


    public void MoveGroup(Vector3 newPosition)
    {
        foreach (var unit in units)
        {

        }
    }

}

public class UnitManager : MonoBehaviour
{

    public static UnitManager instance;

    private DataController dataController;

    // �� ��ȯ�� ���� ����Ʈ
    public Dictionary<UnitType, PlayerUnitList> units = new();

    // ���� ������ �׷�ȭ�ϴ� ����Ʈ
    public Dictionary<string, List<PlayerUnit>> groupUnits = new();

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Start()
    {
        if (dataController == null)
            dataController = FindObjectOfType<DataController>();

        units.Add(UnitType.EXPLOSIVE, new PlayerUnitList());
        units.Add(UnitType.PIERCE, new PlayerUnitList());
        units.Add(UnitType.NORMAL, new PlayerUnitList());
    }

    public void AddMyUnit(PlayerUnit unit)
    {
        if (units.TryGetValue(unit.unitInfo.unitType, out PlayerUnitList unitList))
        {
            unitList.AddUnit(unit);

            // ���� ���� �׷쿡 �߰��ϱ�
            if (groupUnits.ContainsKey(unit.idName) == false)
                groupUnits[unit.idName] = new List<PlayerUnit>();

            groupUnits[unit.idName].Add(unit);
        }
    }

    public void RemoveUnit(PlayerUnit unit)
    {
        if (units.TryGetValue(unit.unitInfo.unitType, out PlayerUnitList unitList))
        {
            unitList.RemoveUnit(unit);
        }

        if (groupUnits.ContainsKey(unit.idName))
        {
            groupUnits[unit.idName].Remove(unit);
        }
    }

    public void OnCombineSelectUnits()
    {
        PlayerUnit unit = TouchManager.instance.GetSelectUnit();
        if (unit == null)
        {
            Debug.Log($"������ ���� ���� ");
            return;
        }

        if (groupUnits.TryGetValue(unit.idName, out List<PlayerUnit> units))
        {
            if (units.Count < 3)
            {
                Debug.Log($"���� ���� ����");
                return;
            }

            Debug.Log($"���� ���� {unit.idName}");
            CombineUnits(units[0], units[1], units[2]);
        }
    }

    // ���� �ռ�
    public void CombineUnits(PlayerUnit unit1, PlayerUnit unit2, PlayerUnit unit3)
    {
        // ���� 3�� ���� 
        RemoveUnit(unit1);
        RemoveUnit(unit2);
        RemoveUnit(unit3);

        // ������Ʈ Ǯ���� ��ȯ
        Manager.Instance.ReturnPool(unit1);
        Manager.Instance.ReturnPool(unit2);
        Manager.Instance.ReturnPool(unit3);

        // ���ο� ���� ����
        PlayerUnit newUnit = CreateNewUnitHigerThanGrade(unit1.unitInfo.info.grade);

        newUnit.transform.position = unit1.transform.position;
        Debug.Log($"���� ���� {newUnit.idName}");
        AddMyUnit(newUnit);
    }

    private PlayerUnit CreateNewUnitHigerThanGrade(UnitGrade grade)
    {
        UnitGrade higherGrade = grade + 1 >= UnitGrade.MYTH ? UnitGrade.MYTH : grade + 1;
        UnitType type = GetRandUnitType();

        var unit = Manager.Instance.Spawn(GetUnitID(type, higherGrade));
        return unit as PlayerUnit;
    }


    // 1. ����� ���� ����. 
    private UnitGrade GetRandomGrade()
    {
        // 0���� 1 ������ ������ �Ǽ����� �����ͼ� �Ҽ��� �� �ڸ����� �ݿø��Ͽ� ó��
        float gradeRand = Mathf.Round(Random.Range(0f, 1f) * 100f) / 100f;

        // 1-1 ��ü Ȯ������ �����ؾ��Ѵ�.
        float[] rates = dataController.GetUnitRates();
        UnitGrade grade = UnitGrade.COMMON;
        for (int i = 0; i < rates.Length; i++)
        {
            if (gradeRand <= rates[i])
            {
                grade = (UnitGrade)i;
            }
        }
        Debug.Log($"Decide Grade : {grade}");

        return grade;
    }

    // 2. ����Ÿ�� ������ ����. 
    private UnitType GetRandUnitType()
    {
        int unitType = Random.Range(0, 3);

        return (UnitType)unitType;
    }

    public string GetUnitID(UnitType type, UnitGrade grade)
    {
        string unitID;
        string first = "";
        string second;

        switch (type)
        {
            case UnitType.EXPLOSIVE:
                first = "Explosive";
                break;
            case UnitType.NORMAL:
                first = "Normal";
                break;
            case UnitType.PIERCE:
                first = "Pierce";
                break;
        }

        switch (grade)
        {
            case UnitGrade.RARE:
                second = "Rare";
                break;
            //case UnitGrade.UNIQUE:
            //    second = "Unique";
            //    break;
            case UnitGrade.LEGEND:
                second = "Legend";
                break;
            case UnitGrade.MYTH:
                second = "Myth";
                break;
            default:
                second = "Common";
                break;
        }

        unitID = first + "_" + second;

        return unitID;
    }


    // 3. ��ް� ���� ����Ÿ������ ���� ID ��������
    public string GetRandomUnitID()
    {
        UnitType type = GetRandUnitType();
        UnitGrade grade = GetRandomGrade();

        return GetUnitID(type, grade);
    }

    // Ư�� ���� Ÿ���� ���� ��ȯ
    public int GetUnitCount(UnitType unitType)
    {
        if (units.TryGetValue(unitType, out PlayerUnitList unitList))
            return unitList.GetCount();

        return 0;
    }

    // Ư�� ������ �ִ��� �˻� 
    public PlayerUnit FindUnit(ref PlayerUnit unit)
    {
        if (units.TryGetValue(unit.unitInfo.unitType, out PlayerUnitList unitList))
        {
            if (unitList.GetUnit(unit.idName) != null)
                return unitList.GetUnit(unit.idName);
        }

        return null;
    }

    // ��� ������ ���������Լ� (��޺��� ������ ���� ����Ʈ�� ��ȯ)
    public List<PlayerUnit> GetAllUnits()
    {
        List<PlayerUnit> allUnits = new List<PlayerUnit>();

        // �� ���� Ÿ�Ժ��� ���ֵ��� ������
        foreach (var unitList in units.Values)
        {
            allUnits.AddRange(unitList.GetUnits());
        }

        return allUnits;
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
        if (units.TryGetValue(type, out PlayerUnitList unitList))
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
