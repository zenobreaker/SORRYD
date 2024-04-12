using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �����ϴ� ������
public class UnitSpawner : MonoBehaviour
{
    public static UnitSpawner Instance;

    public List<string> unitIDList = new List<string>(); 

    private void Awake()
    {
        if (Instance == null)
            Instance = this; 
    }


    public void ButtonEventCreateUnit()
    {
        int rand = Random.Range(0, unitIDList.Count);

        CreateUnit(unitIDList[rand]); 
    }

    public void CreateUnit(string name)
    {
        //var unit = Instantiate(playerUnit); 
        var unit = Manager.Instance.Spawn(name);

        if(unit.TryGetComponent<PlayerUnit>(out PlayerUnit playerUnit))
        {
            playerUnit.transform.position = this.transform.position;

            // ���ָŴ����� �� ���� �߰�
            UnitManager.instance.AddMyUnit(playerUnit);
        }

    }


    
}
