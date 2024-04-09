using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

/// <summary>
///  ���� ���� ���ִ� Ŭ����
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    public PoolManager poolManager;

    [SerializeField]
    private ObjectPoolInfo[] enemyInfos; // �� ������
    [SerializeField]
    private float spawnTime;        // �� ���� �ð�
    [SerializeField]
    private  Transform[] wayPoints; // �̵� ��� 

    public int currentEnemyCount; // ���� ��ȯ�� �� ����
    public int enemyMaxCount;       // ������ ��ȯ�ϴ� �ִ� ��ġ
    public int enemyCurrentSpawnIndex;  // ���� ��ȯ�ϴ� �� index ��
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

    // �� ��ȯ
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
