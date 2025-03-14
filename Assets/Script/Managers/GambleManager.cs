using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 유닛 도박 뽑기 
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

    // 일반 뽑기 (커먼 ~ 레어) 
    public void OnGamble_Normal()
    {
        // 0.5 0.3 0.2 
        UnitGrade grade = UnitGrade.COMMON;
        float rand = Random.Range(0.0f, 1.0f);
        if(rand <= 0.7f)
            grade = UnitGrade.COMMON;
        else 
            grade = UnitGrade.RARE;

        // 유닛타입 
        int type = Random.Range(0, 3);
        UnitType unitType = (UnitType)type;

        SpawnRandomUnit(unitType, grade);
    }


    // 고급 뽑기 (레어~ 전설) 
    public void OnGamble_Special()
    {
        // 0.5 0.3 0.2 
        UnitGrade grade = UnitGrade.RARE;
        float rand = Random.Range(0.0f, 1.0f);
        if (rand <= 0.8f)
            grade = UnitGrade.RARE;
        else
            grade = UnitGrade.LEGEND;

        // 유닛타입 
        int type = Random.Range(0, 3);
        UnitType unitType = (UnitType)type;

        SpawnRandomUnit(unitType, grade);
    }
    
    // 신화 뽑기 (유니크 ~ 전설)
    public void OnGamble_Mythtic()
    {
        // 0.5 0.3 0.2 
        UnitGrade grade = UnitGrade.RARE;
        float rand = Random.Range(0.0f, 1.0f);
        if (rand <= 0.8f)
            grade = UnitGrade.RARE;
        else
            grade = UnitGrade.LEGEND;

        // 유닛타입 
        int type = Random.Range(0, 3);
        UnitType unitType = (UnitType)type;

        SpawnRandomUnit(unitType, grade);
    }
}
