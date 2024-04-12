using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using System.Runtime.InteropServices;

/// <summary>
///  ���� ���� ���ִ� Ŭ����
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    public PoolManager poolManager;

    [SerializeField]
    private float spawnTime;        // �� ���� �ð�
    [SerializeField]
    private  Transform[] wayPoints; // �̵� ��� 

    public int currentEnemyCount; // ���� ��ȯ�� �� ����
    public int enemyMaxCount;       // ������ ��ȯ�ϴ� �ִ� ��ġ
    public int enemyCurrentSpawnIndex;  // ���� ��ȯ�ϴ� �� index ��
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
        // ���׹� ��ȯ
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
            // ���� ������ŭ ����� 
            if (data != null)
            {
                // todo �ٽ��ϱ� �� �� �Ʒ� ������ �� �� �߰��ؾ��Ѵ�.
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

    // �� ��ȯ
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
            Debug.Log("��ȯ�� id : " +  info.idName);
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
