using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유닛을 생성하는 스포너
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
