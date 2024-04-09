using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

/// <summary>
///  적을 생성 해주는 클래스
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    public PoolManager poolManager;

    [SerializeField]
    private ObjectPoolInfo[] enemyInfos; // 적 프리팹
    [SerializeField]
    private float spawnTime;        // 적 스폰 시간
    [SerializeField]
    private  Transform[] wayPoints; // 이동 경로 

    public int currentEnemyCount; // 현재 소환된 적 개수
    public int enemyMaxCount;       // 적마다 소환하는 최대 수치
    public int enemyCurrentSpawnIndex;  // 현재 소환하는 적 index 값
    Coroutine spawnCoroutine;

    public bool isSpawnEnd = false;

    private void Awake()
    {
        poolManager = GetComponent<PoolManager>(); 
    }

    public void StartSpwan(int currentIndex)
    {
         StartCoroutine(SpawnEnemy(currentIndex));
    }


    public ObjectPoolInfo EnemySpawn(string name)
    {
        return poolManager.GetFromPool<ObjectPoolInfo>(name); 
    }

    // 적 소환
    IEnumerator SpawnEnemy(int currentIndex)
    {
        isSpawnEnd = false; 
        int spawnCount = 0;
        
        if (currentIndex < enemyInfos.Length)
        {
            while (spawnCount < enemyMaxCount)
            {
                spawnCount++;
                var enemy = EnemySpawn(enemyInfos[currentIndex].idName);
                if (enemy.TryGetComponent<EnemyController>(out var enemyController))
                {
                    currentEnemyCount++;
                    enemyController.SetUp(wayPoints);
                    enemyController.EnemyDied += HandleEnemyDied;
                }

                yield return new WaitForSeconds(spawnTime);
            }
        }
        
        yield return null;

        isSpawnEnd = true; 
        //StopCoroutine(spawnCoroutine);
        spawnCoroutine = null;
    }
 
    public void HandleEnemyDied(object sender, EventArgs e)
    {
        var info = sender as ObjectPoolInfo;
        if (info != null)
            poolManager.TakeToPool<ObjectPoolInfo>(info.idName, info);
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
