using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UnitStat", menuName = "Game/UnitStat")]
public class UnitStatInfoScriptable : ScriptableObject
{
    [Header("ID")]
    public string idname = "";
    [Header("기본정보 ")]
    public UnitInfo info;
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

    public UnitStatInfoScriptable Clone()
    {
        UnitStatInfoScriptable info = Instantiate(this);

        return info;
    }
}
