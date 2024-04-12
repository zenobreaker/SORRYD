using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using System.Runtime.InteropServices;

/// <summary>
///  적을 생성 해주는 클래스
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    public PoolManager poolManager;

    [SerializeField]
    private float spawnTime;        // 적 스폰 시간
    [SerializeField]
    private  Transform[] wayPoints; // 이동 경로 

    public int currentEnemyCount; // 현재 소환된 적 개수
    public int enemyMaxCount;       // 적마다 소환하는 최대 수치
    public int enemyCurrentSpawnIndex;  // 현재 소환하는 적 index 값
    //Coroutine spawnCoroutine;

    public bool isSpawnEnd = false;

    [SerializeField]
    EnemyInfoScriptalbe enemyInfoScriptalbe;

    List<EnemyInfo> enemyInfoList = new List<EnemyInfo>();
    List<ObjectPoolInfo> enemyList = new List<ObjectPoolInfo>();

    private void Awake()
    {
        poolManager = GetComponent<PoolManager>(); 
    }

    private void Start()
    {
        // 에네미 소환
        EnemySpawn();
    }

    public void StartSpwan(int currentIndex)
    {
         StartCoroutine(SpawnEnemy(currentIndex));
    }


    public void EnemySpawn()
    {
        enemyInfoList = enemyInfoScriptalbe.enemies;
        if (enemyInfoList == null) return;

        foreach (var data in enemyInfoList)
        {
            // 등장 개수만큼 만들기 
            if (data != null)
            {
                // todo 다시하기 할 때 아래 로직을 좀 더 추가해야한다.
                for (int i = 0; i < data.appearCount; i++)
                {
                    var poolInfo = poolManager.GetFromPool<ObjectPoolInfo>(data.enemyID);
                    poolInfo.gameObject.SetActive(false);
                    enemyList.Add(poolInfo);
                }
            }
        }
     }

    ObjectPoolInfo GetEnemy(string name)
    {
        var enemy = enemyList.Find(x => x.idName == name);
        enemyList.Remove(enemy);
        return enemy; 
    }

    // 적 소환
    IEnumerator SpawnEnemy(int currentIndex)
    {
        isSpawnEnd = false; 
        int spawnCount = 0;
        
       
        if (currentIndex < enemyInfoList.Count)
        {
            while (spawnCount < enemyInfoList[currentIndex].appearCount)
            {
                spawnCount++;
                var enemy = GetEnemy(enemyInfoList[currentIndex].enemyID);
                if (enemy == null)
                    continue; 

                if (enemy.TryGetComponent<EnemyController>(out var enemyController))
                {
                    enemy.gameObject.SetActive(true);

                    currentEnemyCount++;
                    enemyController.SetUp(wayPoints);
                    enemyController.EnemyDied += HandleEnemyDied;
                }

                yield return new WaitForSeconds(spawnTime);
            }
        }
        
        yield return null;

        isSpawnEnd = true; 
        //spawnCoroutine = null;
    }
 
    public void HandleEnemyDied(object sender, EventArgs e)
    {
        var info = sender as ObjectPoolInfo;
        if (info != null)
        {
            Debug.Log("반환할 id : " +  info.idName);
            poolManager.TakeToPool<ObjectPoolInfo>(info.idName, info);
        }
        currentEnemyCount -= 1;
    }

    public bool CheckMaxEnemyCount()
    {
        if (currentEnemyCount >= enemyMaxCount)
            return true;
        else
            return false; 
    }

}
