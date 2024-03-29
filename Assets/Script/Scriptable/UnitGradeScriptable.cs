using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UnitInfo
{
    [Header("등급")]
    public UnitGrade grade;
    [Header("판매가격")]
    public int sellingValue;

}

[System.Serializable]
public class UnitStatInfo : UnitInfo
{
    [Header("공격타입")]
    public UnitType unitType;
    [Header("공격력")]
    public int attack;              // 공격력
    [Header("추가 공격력")]
    public int additionalAttack;    // 추가 공격력
    [Header("공격속도")]
    public float attackSpeed;       // 공격속도
    [Header("추가 공격속도")]
    public float additionalAttackSpeed; // 추가 공격속도
    [Header("공격 사거리")]
    public float attackRange;       // 공격 사거리
    
}



[CreateAssetMenu(fileName = "Unit Data", menuName = "Scriptable Object/Unit Data", order = int.MaxValue)]
public class UnitGradeScriptable : ScriptableObject
{
    [Header("최대 강화")]
    public int maxUpgrade;
    // 타입별 강화 수치 정보
    public List<UnitInfo> gradeInfoList = new();
}
