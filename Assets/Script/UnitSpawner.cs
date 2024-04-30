using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �����ϴ� ������
public class UnitSpawner : MonoBehaviour
{
    public static UnitSpawner Instance;

    public List<string> unitIDList = new List<string>();

    public int SPAWN_UNIT_COST = 100;

    public DataController dataController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this; 
    }

    private void Start()
    {
        if(dataController == null)
            dataController = FindObjectOfType<DataController>();
    }

    public string GetUnitID(UnitType type, UnitGrade grade)
    {
        string unitID;
        string first = "" ;
        string second; 

        switch(type)
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

        switch(grade)
        {
            case UnitGrade.RARE:
                second = "Rare";
                break;
            case UnitGrade.UNIQUE:
                second = "Unique";
                break;
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

    public void ButtonEventCreateUnit()
    {
        if (dataController == null)
            return;

        // 1. ����� ���� ����. 
        // 0���� 1 ������ ������ �Ǽ����� �����ͼ� �Ҽ��� �� �ڸ����� �ݿø��Ͽ� ó��
        float gradeRand = Mathf.Round(Random.Range(0f, 1f) * 100f) / 100f;

        // 1-1 ��ü Ȯ������ �����ؾ��Ѵ�.
        float[] rates = dataController.GetUnitRates();
        UnitGrade grade = UnitGrade.COMMON;
        for(int i = 0; i < rates.Length; i++)
        {
            if(gradeRand <= rates[i])
            {
                grade = (UnitGrade)i;
            }
        }
        Debug.Log($"Decide Grade : {grade}");

        // 2. �ش� ��޿��� ����Ÿ�� ������ ����. 
        int unitType = Random.Range(0, 3);

        // 3. ��ް� ���� ����Ÿ������ ���� ID ��������
        var unitID = GetUnitID((UnitType)unitType, grade);
        Debug.Log($"Appear Unit ID : {unitID}");
        CreateUnit(unitID); 
    }


    public void CheatCreateUnit(string name)
    {
        var unit = Manager.Instance.Spawn(name);
        if (unit == null) return; 

        if (unit.TryGetComponent<PlayerUnit>(out PlayerUnit playerUnit))
        {
            playerUnit.transform.position = this.transform.position;

            // ���ָŴ����� �� ���� �߰�
            UnitManager.instance.AddMyUnit(playerUnit);
        }
    }

    public void CreateUnit(string name)
    {
        //var unit = Instantiate(playerUnit); 
        int money = GameManager.instance.money;
        if(SPAWN_UNIT_COST > money)
        {
            Debug.Log("�ڽ�Ʈ�� �����Ͽ� ������ ���� �� �����ϴ�.");
            return;
        }

        GameManager.instance.UseMoney(SPAWN_UNIT_COST);

        var unit = Manager.Instance.Spawn(name);

        if(unit.TryGetComponent<PlayerUnit>(out PlayerUnit playerUnit))
        {
            playerUnit.transform.position = this.transform.position;

            // ���ָŴ����� �� ���� �߰�
            UnitManager.instance.AddMyUnit(playerUnit);
        }

    }


    
}
