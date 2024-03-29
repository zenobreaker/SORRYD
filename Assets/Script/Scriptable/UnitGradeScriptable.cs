using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UnitInfo
{
    [Header("���")]
    public UnitGrade grade;
    [Header("�ǸŰ���")]
    public int sellingValue;

}

[System.Serializable]
public class UnitStatInfo : UnitInfo
{
    [Header("����Ÿ��")]
    public UnitType unitType;
    [Header("���ݷ�")]
    public int attack;              // ���ݷ�
    [Header("�߰� ���ݷ�")]
    public int additionalAttack;    // �߰� ���ݷ�
    [Header("���ݼӵ�")]
    public float attackSpeed;       // ���ݼӵ�
    [Header("�߰� ���ݼӵ�")]
    public float additionalAttackSpeed; // �߰� ���ݼӵ�
    [Header("���� ��Ÿ�")]
    public float attackRange;       // ���� ��Ÿ�
    
}



[CreateAssetMenu(fileName = "Unit Data", menuName = "Scriptable Object/Unit Data", order = int.MaxValue)]
public class UnitGradeScriptable : ScriptableObject
{
    [Header("�ִ� ��ȭ")]
    public int maxUpgrade;
    // Ÿ�Ժ� ��ȭ ��ġ ����
    public List<UnitInfo> gradeInfoList = new();
}
