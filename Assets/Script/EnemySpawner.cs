using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  적을 생성 해주는 클래스
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private ObjectPoolInfo[] enemyInfos; // 적 프리팹
    [SerializeField]
    private float spawnTime;        // 적 스폰 시간
    [SerializeField]
    private  Transform[] wayPoints; // 이동 경로 

    public int enemyMaxCount;       // 적마다 소환하는 최대 수치
    public int enemyCurrentSpawnIndex;  // 현재 소환하는 적 index 값
    public bool isBossRound;    // 현재 보스가 등장하는 라운드인지 체크 
    Coroutine spawnCoroutine;

    public void StartSpwan(int currentIndex)
    {
        if(spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnEnemy(currentIndex));
    }

    // 적 소환
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
