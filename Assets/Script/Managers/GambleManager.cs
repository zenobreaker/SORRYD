using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���� ���� �̱� 
/// </summary>
public class GambleManager : MonoBehaviour
{
    public static GambleManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this; 
    }
    
    private void SpawnRandomUnit(UnitType InType, UnitGrade InGrade)
    {
        var id = UnitManager.instance.GetUnitID(InType, InGrade);

        UnitSpawner.Instance.CreateUnit(id);
    }

    // �Ϲ� �̱� (Ŀ�� ~ ����) 
    public void OnGamble_Normal()
    {
        // 0.5 0.3 0.2 
        UnitGrade grade = UnitGrade.COMMON;
        float rand = Random.Range(0.0f, 1.0f);
        if(rand <= 0.7f)
            grade = UnitGrade.COMMON;
        else 
            grade = UnitGrade.RARE;

        // ����Ÿ�� 
        int type = Random.Range(0, 3);
        UnitType unitType = (UnitType)type;

        SpawnRandomUnit(unitType, grade);
    }


    // ��� �̱� (����~ ����) 
    public void OnGamble_Special()
    {
        // 0.5 0.3 0.2 
        UnitGrade grade = UnitGrade.RARE;
        float rand = Random.Range(0.0f, 1.0f);
        if (rand <= 0.8f)
            grade = UnitGrade.RARE;
        else
            grade = UnitGrade.LEGEND;

        // ����Ÿ�� 
        int type = Random.Range(0, 3);
        UnitType unitType = (UnitType)type;

        SpawnRandomUnit(unitType, grade);
    }
    
    // ��ȭ �̱� (����ũ ~ ����)
    public void OnGamble_Mythtic()
    {
        // 0.5 0.3 0.2 
        UnitGrade grade = UnitGrade.RARE;
        float rand = Random.Range(0.0f, 1.0f);
        if (rand <= 0.8f)
            grade = UnitGrade.RARE;
        else
            grade = UnitGrade.LEGEND;

        // ����Ÿ�� 
        int type = Random.Range(0, 3);
        UnitType unitType = (UnitType)type;

        SpawnRandomUnit(unitType, grade);
    }
}
