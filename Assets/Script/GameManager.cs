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

    public EnemySpawner spawner;

    public UnitSpawner unitSpawner;
    
    enum  RoundState
    {
        READY,
        START,
        END,
    }

    public int currentRound;    // ���� �������� ���� 
    public int maxRound;        // �ִ� ���� 

    RoundState roundState;
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
        // ���� ���� 
        if (currentRound >= maxRound)
            yield break;    // �ִ� ���带 �Ѱ�ٸ� ���� ���� �Ұ�

        Debug.Log("���� ���� " + currentRound);
        // �� ���� 
        spawner.StartSpwan(currentRound);

        SetRoundTimerValue();

        StartCoroutine(CoRoundTimer());

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
