using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ���� ���� ���ִ� Ŭ����
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private ObjectPoolInfo[] enemyInfos; // �� ������
    [SerializeField]
    private float spawnTime;        // �� ���� �ð�
    [SerializeField]
    private  Transform[] wayPoints; // �̵� ��� 

    public int enemyMaxCount;       // ������ ��ȯ�ϴ� �ִ� ��ġ
    public int enemyCurrentSpawnIndex;  // ���� ��ȯ�ϴ� �� index ��
    public bool isBossRound;    // ���� ������ �����ϴ� �������� üũ 
    Coroutine spawnCoroutine;

    public void StartSpwan(int currentIndex)
    {
        if(spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnEnemy(currentIndex));
    }

    // �� ��ȯ
    IEnumerator SpawnEnemy(int currentIndex)
    {
        int spawnCount = 0; 
        while(spawnCount < enemyMaxCount)
        {
            spawnCount++;
            //GameObject enemy = Instantiate(enemyInfos[currentIndex]);
            var enemy = Manager.Instance.Spawn(enemyInfos[currentIndex].idName);
            if(enemy.TryGetComponent<EnemyController>(out var enemyController))
            {
                enemyController.SetUp(wayPoints);
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }
 
}
