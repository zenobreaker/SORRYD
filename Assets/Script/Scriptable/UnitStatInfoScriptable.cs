using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UnitStat", menuName = "Game/UnitStat")]
public class UnitStatInfoScriptable : ScriptableObject
{
    [Header("ID")]
    public string idname = "";
    [Header("�⺻���� ")]
    public UnitInfo info;
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

    public UnitStatInfoScriptable Clone()
    {
        UnitStatInfoScriptable info = Instantiate(this);

        return info;
    }
}
