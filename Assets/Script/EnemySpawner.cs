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
    public bool isBossRound;    // ���� ������ �����ϴ� �������� üũ 
    Coroutine spawnCoroutine;

    private void Awake()
    {
        poolManager = GetComponent<PoolManager>(); 
    }

    public void StartSpwan(int currentIndex)
    {
        if(spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnEnemy(currentIndex));
    }


    public ObjectPoolInfo EnemySpawn(string name)
    {
        return poolManager.GetFromPool<ObjectPoolInfo>(name); 
    }

    // �� ��ȯ
    IEnumerator SpawnEnemy(int currentIndex)
    {
        int spawnCount = 0; 
        while(spawnCount < enemyMaxCount)
        {
            spawnCount++;
            //GameObject enemy = Instantiate(enemyInfos[currentIndex]);
            //var enemy = Manager.Instance.Spawn(enemyInfos[currentIndex].idName);
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
 
    public void HandleEnemyDied(object sender, EventArgs e)
    {
        //Manager.Instance.ReturnPool((ObjectPoolInfo)sender);
        var info = sender as ObjectPoolInfo;
        if (info != null)
            poolManager.TakeToPool<ObjectPoolInfo>(info.idName, info);
        currentEnemyCount -= 1;
    }
}
