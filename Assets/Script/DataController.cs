using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SDConstData
{
    public int id;
    public int grade;
    public int sellValue;
    public float appearRate;
    public int sellable;
}

[System.Serializable]
public class SDConstdataGroup
{
    public SDConstData[] sdConstData;
}

[System.Serializable]
public class ConstData
{
    public int id;
    public UnitGrade grade;
    public int sellValue;
    public float appearRate;
    public bool sellable;

    public ConstData(int id, int grade, int sellValue, float rate, int able)
    {
        this.id = id;
        this.grade = (UnitGrade)grade;
        this.sellValue = sellValue;
        this.appearRate = rate;
        sellable = able == 1; 
    }
}

public class DataController : MonoBehaviour
{
    public TextAsset jsonData;

    public SDConstdataGroup dataGroup;
    public List<ConstData> dataList = new List<ConstData>();

    void Start()
    {
        if (jsonData == null)
            return; 

        dataGroup = JsonUtility.FromJson<SDConstdataGroup>(jsonData.text);
        if (dataGroup.sdConstData == null) return;

        foreach (var data in dataGroup.sdConstData)
        {
            if (data == null) continue; 

            ConstData constData = new ConstData(data.id, data.grade, data.sellValue,
                data.appearRate, data.sellable);

            dataList.Add(constData);
        }
    }

    public float GetUnitRate(UnitGrade grade)
    {
        var data = dataList.Find(x => x.grade == grade);
        if(data != null)
        {
            return data.appearRate;
        }

        return 0.0f; 
    }

    public float[] GetUnitRates()
    {
        float[] rates = new float[(int)UnitGrade.MAX];

        for(int i =0; i < rates.Length; i++)
        {
            rates[i] = GetUnitRate((UnitGrade)i);
        }

        return rates; 
    }


}
