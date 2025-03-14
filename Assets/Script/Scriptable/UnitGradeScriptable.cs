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




[CreateAssetMenu(fileName = "Unit Data", menuName = "Scriptable Object/Unit Data", order = int.MaxValue)]
public class UnitGradeScriptable : ScriptableObject
{
    [Header("�ִ� ��ȭ")]
    public int maxUpgrade;
    // Ÿ�Ժ� ��ȭ ��ġ ����
    public List<UnitInfo> gradeInfoList = new();
}
