using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �����ϴ� ������
public class UnitSpawner : MonoBehaviour
{
    public static UnitSpawner Instance;

    public GameObject playerUnit; 

    private void Awake()
    {
        if (Instance == null)
            Instance = this; 
    }


    public void CreateUnit()
    {
        var unit = Instantiate(playerUnit); 

    }

}
