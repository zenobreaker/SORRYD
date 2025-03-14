using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

// 플레이어 유닛을 한꺼번에 다루기 위한 그룹 클래스
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

    // 내 소환한 유닛 리스트
    public Dictionary<UnitType, PlayerUnitList> units = new();

    // 같은 유닛을 그룹화하는 리스트
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

            // 같은 유닛 그룹에 추가하기
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
            Debug.Log($"선택한 유닛 없음 ");
            return;
        }

        if (groupUnits.TryGetValue(unit.idName, out List<PlayerUnit> units))
        {
            if (units.Count < 3)
            {
                Debug.Log($"유닛 개수 부족");
                return;
            }

            Debug.Log($"조합 실행 {unit.idName}");
            CombineUnits(units[0], units[1], units[2]);
        }
    }

    // 유닛 합성
    public void CombineUnits(PlayerUnit unit1, PlayerUnit unit2, PlayerUnit unit3)
    {
        // 기존 3개 제거 
        RemoveUnit(unit1);
        RemoveUnit(unit2);
        RemoveUnit(unit3);

        // 오브젝트 풀링에 반환
        Manager.Instance.ReturnPool(unit1);
        Manager.Instance.ReturnPool(unit2);
        Manager.Instance.ReturnPool(unit3);

        // 새로운 유닛 생성
        PlayerUnit newUnit = CreateNewUnitHigerThanGrade(unit1.unitInfo.info.grade);

        newUnit.transform.position = unit1.transform.position;
        Debug.Log($"조합 성공 {newUnit.idName}");
        AddMyUnit(newUnit);
    }

    private PlayerUnit CreateNewUnitHigerThanGrade(UnitGrade grade)
    {
        UnitGrade higherGrade = grade + 1 >= UnitGrade.MYTH ? UnitGrade.MYTH : grade + 1;
        UnitType type = GetRandUnitType();

        var unit = Manager.Instance.Spawn(GetUnitID(type, higherGrade));
        return unit as PlayerUnit;
    }


    // 1. 등급을 먼저 고른다. 
    private UnitGrade GetRandomGrade()
    {
        // 0부터 1 사이의 랜덤한 실수값을 가져와서 소수점 두 자리까지 반올림하여 처리
        float gradeRand = Mathf.Round(Random.Range(0f, 1f) * 100f) / 100f;

        // 1-1 전체 확률에서 결정해야한다.
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

    // 2. 공격타입 유닛을 고른다. 
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


    // 3. 등급과 유닛 공격타입으로 유닛 ID 가져오기
    public string GetRandomUnitID()
    {
        UnitType type = GetRandUnitType();
        UnitGrade grade = GetRandomGrade();

        return GetUnitID(type, grade);
    }

    // 특정 유닛 타입의 개수 반환
    public int GetUnitCount(UnitType unitType)
    {
        if (units.TryGetValue(unitType, out PlayerUnitList unitList))
            return unitList.GetCount();

        return 0;
    }

    // 특정 유닛이 있는지 검사 
    public PlayerUnit FindUnit(ref PlayerUnit unit)
    {
        if (units.TryGetValue(unit.unitInfo.unitType, out PlayerUnitList unitList))
        {
            if (unitList.GetUnit(unit.idName) != null)
                return unitList.GetUnit(unit.idName);
        }

        return null;
    }

    // 모든 유닛을 가져오는함수 (등급별로 관리된 유닛 리스트를 반환)
    public List<PlayerUnit> GetAllUnits()
    {
        List<PlayerUnit> allUnits = new List<PlayerUnit>();

        // 각 유닛 타입별로 유닛들을 가져옴
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

        // TODO: 유닛 판매 시, 해당하는 금액 받기 
    }


    // 업그레이드 관련한 정보 전부 적용 하기 
    public void ApplyUpgrade(UnitType type, int value)
    {
        if (units.TryGetValue(type, out PlayerUnitList unitList))
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
