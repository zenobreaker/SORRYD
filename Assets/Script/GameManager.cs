using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;



// 게임의 로직을 전반적으로 관리 &^^
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("게임머니")]
    public int money; 

    public EnemySpawner spawner;

    public UnitSpawner unitSpawner;
    
    enum  RoundState
    {
        READY,
        START,
        END,
    }

    public int currentRound;    // 현재 진행중인 라운드 
    public int maxRound;        // 최대 라운드 

    RoundState roundState;
    float roundTimer;
    Coroutine roundTimerCoroutine;
    private void Awake()
    {
        if (instance == null)
            instance = this; 
    }

    void Update()
    {
        if (roundTimerCoroutine == null)
            roundTimerCoroutine = StartCoroutine(RoundTimer());
      
        if (Input.GetKeyDown(KeyCode.Escape))
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


    IEnumerator RoundTimer()
    {
        // 라운드 시작 
        if (currentRound >= maxRound)
            yield break;    // 최대 라운드를 넘겼다면 게임 진행 불가
       
        // 적 생성 
        spawner.StartSpwan(currentRound);

        yield return new WaitForSeconds(roundTimer);

        currentRound++;
        StopCoroutine(roundTimerCoroutine);
        roundTimerCoroutine = null; 
    }

    // 현재 필드에 몬스터가 최대로 있는지 검사 
    public bool CheckMaxEnemyCount()
    {
        return spawner == null || spawner.CheckMaxEnemyCount();
    }
}
