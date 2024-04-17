using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;



// ������ ������ ���������� ���� &^^
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("���ӸӴ�")]
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

    public int currentRound;    // ���� �������� ���� 
    public int maxRound;        // �ִ� ���� 

    GameState gameState;
    public float maxRoundTimer;
    public float maxBossRoundTimer; 
    public float roundTimer;
    Coroutine roundTimerCoroutine;
    Coroutine timerCoroutine;

    bool isBossRound; // ���� �������� Ȯ���ϴ� �÷��� �� 

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
        }
        else
        {

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
            roundTimer -= Time.unscaledDeltaTime;
            yield return null;
        }

        yield return null;
        StopCoroutine(CoRoundTimer());
        timerCoroutine = null;
    }

    IEnumerator RoundTimer()
    {
        // ���� ���� 
        if (currentRound >= maxRound)
            yield break;    // �ִ� ���带 �Ѱ�ٸ� ���� ���� �Ұ�

        Debug.Log("���� ���� " + currentRound);
        // �� ���� 
        spawner.StartSpwan(currentRound);

        SetRoundTimerValue();

        if(timerCoroutine == null)
            timerCoroutine  = StartCoroutine(CoRoundTimer());

        yield return new WaitUntil(()=> roundTimer <= 0.0f);

        currentRound++;
        StopCoroutine(roundTimerCoroutine);
        roundTimerCoroutine = null; 
    }

    // ���� �ʵ忡 ���Ͱ� �ִ�� �ִ��� �˻� 
    public bool CheckMaxEnemyCount()
    {
        return spawner == null || spawner.CheckMaxEnemyCount();
    }
}
