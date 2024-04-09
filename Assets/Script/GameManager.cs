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
    public float maxRoundTimer;
    public float maxBossRoundTimer; 
    public float roundTimer;
    Coroutine roundTimerCoroutine;
    Coroutine timerCoroutine;

    bool isBossRound; // 보스 라운드인지 확인하는 플래그 값 

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

    void SetRoundTimerValue()
    {
        if (isBossRound == true)
        {
            roundTimer = maxBossRoundTimer;
        }
        else
        {
            roundTimer = maxRoundTimer;
        }
    }

    IEnumerator CoRoundTimer()
    {
        while (roundTimer >= 0)
        {
            roundTimer -= Time.deltaTime;
            yield return null;
        }

        yield return null;
        StopCoroutine(CoRoundTimer());
        timerCoroutine = null;
    }

    IEnumerator RoundTimer()
    {
        // 라운드 시작 
        if (currentRound >= maxRound)
            yield break;    // 최대 라운드를 넘겼다면 게임 진행 불가

        Debug.Log("현재 라운드 " + currentRound);
        // 적 생성 
        spawner.StartSpwan(currentRound);

        SetRoundTimerValue();

        StartCoroutine(CoRoundTimer());

        yield return new WaitUntil(()=> roundTimer <= 0.0f);

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
