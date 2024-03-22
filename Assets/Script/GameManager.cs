using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ������ ���������� ���� &^^
public class GameManager : MonoBehaviour
{
    public EnemySpawner spawner;

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
    }
}
