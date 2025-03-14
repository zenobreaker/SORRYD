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




[CreateAssetMenu(fileName = "Unit Data", menuName = "Scriptable Object/Unit Data", order = int.MaxValue)]
public class UnitGradeScriptable : ScriptableObject
{
    [Header("최대 강화")]
    public int maxUpgrade;
    // 타입별 강화 수치 정보
    public List<UnitInfo> gradeInfoList = new();
}
