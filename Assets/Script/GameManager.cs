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

    public int START_GAME_MONEY = 300;

    public EnemySpawner spawner;

    public UnitSpawner unitSpawner;
    
    enum  GameState
    {
        READY,
        GAME_PLAY,
        END,
    }
    [SerializeField]
    EnemyInfoScriptable enemyInfoScriptable;

    public int currentRound = 1;    // 현재 진행중인 라운드 
    public int maxRound;        // 최대 라운드 

    GameState gameState;
    public float maxRoundTimer;
    public float maxBossRoundTimer; 
    public float roundTimer;
    Coroutine roundTimerCoroutine;
    Coroutine timerCoroutine;

    bool isBossRound; // 보스 라운드인지 확인하는 플래그 값 

    bool isWin; // 게임을 정상적으로 클리어했는지에 대한 플래그 값

    private void Awake()
    {
        if (instance == null)
            instance = this; 
    }

    void Update()
    {

        if(gameState == GameState.READY)
        {
            SetGameMoney();
            gameState = GameState.GAME_PLAY;
        }
        else if(gameState == GameState.GAME_PLAY)
        {

            if (roundTimerCoroutine == null)
                roundTimerCoroutine = StartCoroutine(RoundTimer());

            ChangeGameStateToEnd();
        }   
        else
        {
            // TODO: 종료 로직  
        }

   
      
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(spawner != null)
            {
                spawner.StartSpwan(0);
            }
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            money = 9999;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(unitSpawner != null)
            {
                unitSpawner.CreateUnit("Unit1");
            }
        }
    }

    public void SetGameMoney()
    {
        money = START_GAME_MONEY;
    }

    public void UseMoney(int value)
    {
        money -= value;
        if (money <= 0)
            money = 0; 
    }

    public float GetRoundRemainTime()
    {
        return roundTimer;
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
        StopCoroutine(timerCoroutine);
        timerCoroutine = null;
    }

    IEnumerator RoundRoutine(int round)
    {
        Debug.Log("현재 라운드 " + round);

    
        SetRoundTimerValue();

        timerCoroutine ??= StartCoroutine(CoRoundTimer());

        yield return new WaitUntil(() => roundTimer <= 0.0f);
    }

    IEnumerator RoundTimer()
    {
       
        // 라운드 시작 
        if (currentRound > maxRound)
            yield break;    // 최대 라운드를 넘겼다면 게임 진행 불가

        Debug.Log("현재 라운드 " + currentRound);
        // 라운드 정보 가져오기 
        if (enemyInfoScriptable != null)
        {
            isBossRound = enemyInfoScriptable.GetIsBossStage(currentRound);
        }

        if(isBossRound == true)
        {
            Debug.Log($"보스 라운드 시작");
        }
        // 2. 적 스폰 
        spawner.StartSpwan(currentRound);

        SetRoundTimerValue();

        timerCoroutine ??= StartCoroutine(CoRoundTimer());

        yield return new WaitUntil(() => roundTimer <= 0.0f);
        
        // 라운드 수 증가 
        currentRound++; 

        StopCoroutine(timerCoroutine);
        timerCoroutine = null;
        // 0419 버그 - 위 두줄이 없으면 currentRound 값이 2에서 멈추며 타이머값이 줄어들지않는다.
        // 아래 코드가 없으면 코루틴이 중첩되거나 update 문에서 이미 존재하는지 확인하기 때문에
        // 변수로 담은 코루틴 값이 null이 되지 않는다. yield 이후 내용이 존재하기 때문?
        StopCoroutine(roundTimerCoroutine);
        roundTimerCoroutine = null;
    }

    // 현재 필드에 몬스터가 최대로 있는지 검사 
    public bool CheckMaxEnemyCount()
    {
        return spawner != null && spawner.CheckMaxEnemyCount();
    }

    public bool CheckGameFailure()
    {
        // 보스 라운드인지 검사 
        if (isBossRound == true)
        {
            // 시간을 체크해야 한다.
            if (roundTimer <= 0)
                return true; 
            
        }
        // 일반 라운드라면 적이 일정 개수 이상 있다면 패배 
        else if (CheckMaxEnemyCount() == true)
        {
            return true; 
        }

        return false; 
    }

    // 마지막 라운드를 클리어했는지 검사 
    public bool CheckFinalRoundClear()
    {
        bool isCurrentRound = currentRound >= maxRound;

        // 몬스터 카운트가 0이면 다 잡힌것
        if (isCurrentRound == true && spawner.CheckAllClearField() == true)
        {
            return true; 
        }
       
        return false; 
    }

    public void ChangeGameStateToEnd()
    {

        // 1. 게임 승리 패배 인지 체크 
        bool isFailure = CheckGameFailure();

        // 2.모든 라운드를 클리어했는지 체크 
        bool isAllClear = CheckFinalRoundClear();
       
        if (isFailure == false && isAllClear == false )
            return;

        gameState = GameState.END;
        if(roundTimerCoroutine != null)
            StopCoroutine(roundTimerCoroutine);
        if (isAllClear == true )
        {
            isWin = true;
            Debug.Log($"All Clear!!!");
        }
        else
        {
            isWin = false;
            Debug.Log($"Failure..");
        }

    }
}
