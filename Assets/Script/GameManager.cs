using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ������ ���������� ���� &^^
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("���ӸӴ�")]
    public int money; 

    public EnemySpawner spawner;

    public UnitSpawner unitSpawner;

    private void Awake()
    {
        if (instance == null)
            instance = this; 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(spawner != null)
            {
                spawner.StartSpwan(0);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(unitSpawner != null)
            {
                unitSpawner.CreateUnit("Unit1");
            }
        }
    }
}
