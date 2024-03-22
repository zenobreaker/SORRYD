using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ���� ���� ���ִ� Ŭ����
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyPrafab; // �� ������
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
            GameObject enemy = Instantiate(enemyPrafab[currentIndex]);
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if(enemyController != null)
            {
                enemyController.SetUp(wayPoints);
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }
 
}
