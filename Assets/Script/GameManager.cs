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
    [SerializeField]
    EnemyInfoScriptable enemyInfoScriptable;

    public int currentRound = 1;    // ���� �������� ���� 
    public int maxRound;        // �ִ� ���� 

    GameState gameState;
    public float maxRoundTimer;
    public float maxBossRoundTimer; 
    public float roundTimer;
    Coroutine roundTimerCoroutine;
    Coroutine timerCoroutine;

    bool isBossRound; // ���� �������� Ȯ���ϴ� �÷��� �� 

    bool isWin; // ������ ���������� Ŭ�����ߴ����� ���� �÷��� ��

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
            // TODO: ���� ����  
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
        Debug.Log("���� ���� " + round);

    
        SetRoundTimerValue();

        timerCoroutine ??= StartCoroutine(CoRoundTimer());

        yield return new WaitUntil(() => roundTimer <= 0.0f);
    }

    IEnumerator RoundTimer()
    {
       
        // ���� ���� 
        if (currentRound > maxRound)
            yield break;    // �ִ� ���带 �Ѱ�ٸ� ���� ���� �Ұ�

        Debug.Log("���� ���� " + currentRound);
        // ���� ���� �������� 
        if (enemyInfoScriptable != null)
        {
            isBossRound = enemyInfoScriptable.GetIsBossStage(currentRound);
        }

        if(isBossRound == true)
        {
            Debug.Log($"���� ���� ����");
        }
        // 2. �� ���� 
        spawner.StartSpwan(currentRound);

        SetRoundTimerValue();

        timerCoroutine ??= StartCoroutine(CoRoundTimer());

        yield return new WaitUntil(() => roundTimer <= 0.0f);
        
        // ���� �� ���� 
        currentRound++; 

        StopCoroutine(timerCoroutine);
        timerCoroutine = null;
        // 0419 ���� - �� ������ ������ currentRound ���� 2���� ���߸� Ÿ�̸Ӱ��� �پ�����ʴ´�.
        // �Ʒ� �ڵ尡 ������ �ڷ�ƾ�� ��ø�ǰų� update ������ �̹� �����ϴ��� Ȯ���ϱ� ������
        // ������ ���� �ڷ�ƾ ���� null�� ���� �ʴ´�. yield ���� ������ �����ϱ� ����?
        StopCoroutine(roundTimerCoroutine);
        roundTimerCoroutine = null;
    }

    // ���� �ʵ忡 ���Ͱ� �ִ�� �ִ��� �˻� 
    public bool CheckMaxEnemyCount()
    {
        return spawner != null && spawner.CheckMaxEnemyCount();
    }

    public bool CheckGameFailure()
    {
        // ���� �������� �˻� 
        if (isBossRound == true)
        {
            // �ð��� üũ�ؾ� �Ѵ�.
            if (roundTimer <= 0)
                return true; 
            
        }
        // �Ϲ� ������ ���� ���� ���� �̻� �ִٸ� �й� 
        else if (CheckMaxEnemyCount() == true)
        {
            return true; 
        }

        return false; 
    }

    // ������ ���带 Ŭ�����ߴ��� �˻� 
    public bool CheckFinalRoundClear()
    {
        bool isCurrentRound = currentRound >= maxRound;

        // ���� ī��Ʈ�� 0�̸� �� ������
        if (isCurrentRound == true && spawner.CheckAllClearField() == true)
        {
            return true; 
        }
       
        return false; 
    }

    public void ChangeGameStateToEnd()
    {

        // 1. ���� �¸� �й� ���� üũ 
        bool isFailure = CheckGameFailure();

        // 2.��� ���带 Ŭ�����ߴ��� üũ 
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
