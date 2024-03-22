using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임의 로직을 전반적으로 관리 &^^
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
