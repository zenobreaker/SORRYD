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
        // ���� ���� 
        if (currentRound >= maxRound)
            yield break;    // �ִ� ���带 �Ѱ�ٸ� ���� ���� �Ұ�
       
        // �� ���� 
        spawner.StartSpwan(currentRound);

        yield return new WaitForSeconds(roundTimer);

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
