using System.Collections;
using UnityEngine;
using Redcode.Pools;
using System;
using System.Collections.Generic;
using System.Linq;

public enum State
{
    IDLE, 
    MOVE,
    ATTACK, 
}


public class PlayerUnit 
    : ObjectPoolInfo
    , IPoolObject
{
    public UnitStatInfoScriptable unitInfo;
    public int upgradeCount; 

    public State state; 

    public int moveSpeed;

    //public ObjectPoolInfo bulletObject;
    public string bulletID;
    public Rigidbody2D rb;
    private Transform targetTr; 

    Vector2 destination = Vector2.zero;
    Coroutine attackCoroutine;
    public LayerMask targetLayer;
    bool isAttacking = false;

    public SpriteRenderer[] myObjs;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myObjs = GetComponentsInChildren<SpriteRenderer>(true).Where(sr => 
        sr.gameObject != this.gameObject).ToArray();
    }

    void Start()
    {
        isAttacking = false; 
    }

    void Update()
    {
        if(state  == State.IDLE)
        {
            CheckRangeInEnemy();
        }
        else if(state == State.MOVE)
        {

        }   
        else if(state == State.ATTACK)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if(rb != null && state == State.MOVE)
        {
            Vector2 dirVec = (destination - rb.position).normalized;
            Vector2 nextVec = moveSpeed * Time.fixedDeltaTime * dirVec; 

            rb.MovePosition(rb.position + nextVec);
            rb.velocity = Vector2.zero;
            CheckArrive();
        }
    }
    public void OnCreatedInPool()
    {
    }

    public void OnGettingFromPool()
    {
        UnitSetUp();
    }

    public void UnitSetUp()
    {
        state = State.IDLE; 

    }


    // 유닛 판매 
    public void SellUnit()
    {
        GameManager.instance.IncreaseMoney(this.unitInfo.info.sellingValue);
        Manager.Instance.ReturnPool(this); 
    }

    // 지정한 위치에 도착했는지 검사 
    public void CheckArrive()
    {
        if (rb == null)
            return;

        // 목표 지점에 도착 했는지 검사 
        if (Vector2.Distance(transform.position, destination) < 0.02f * moveSpeed)
        {
            //Debug.Log("도착 : " + Vector2.Distance(transform.position, destination));
            state = State.IDLE;
            return;
        }
    }

    // 지정한 위치로 목표지정
    public void MoveTo(Vector2 destination)
    {
        state = State.MOVE;

        // 대상 위치와 현재 위치 사이의 거리를 계산
        float distance = Vector2.Distance(transform.position, destination);

        // 대상 위치와 현 재 위치 사이의 거리가 일정 거리 이내일 때
        if (distance < 2)
        {
            // 대상 위치에 다른 유닛이 있는지 확인
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(destination, 2);


            // 다른  유닛이 있으면
            if(collider2Ds.Length > 0)
            {
                // 가장 가까운 유닛을 찾아서 그 근처를 도착지로 조정
                Vector2 closestPosition = collider2Ds[0].transform.position;
                bool isAnother = true; 
                foreach(var collider in collider2Ds)
                {
                    if(collider.transform == this.transform)
                    {
                        isAnother = false; 
                        continue; 
                    }
                    float distToTarget = Vector2.Distance(collider.transform.position, destination);
                    float distToClosest = Vector2.Distance(closestPosition, destination);
                    if (distToTarget < distToClosest)
                    {
                        closestPosition = collider.transform.position;
                    }
                }

                if (isAnother == true)
                {
                    Debug.Log("계산처리");
                    destination = closestPosition;
                }
            }
        }

        this.destination = destination;
    }

    // 대상이 자신의 공격 범위에 있는지 검사
    public void CheckRangeInEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, unitInfo.attackRange,
            targetLayer);
        if(colliders.Length >0)
        {
            foreach(var collider in colliders)
            {
                if(collider.transform.CompareTag("Enemy"))
                {
                    SetTarget(collider.transform);
                    break; 
                }
            }
        }
    }

    // 대상을 타겟팅
    public void SetTarget(Transform target)
    {
        targetTr = target;
        state = State.ATTACK;
    }

    // 공격
    public void Attack()
    {
        if(targetTr != null && isAttacking == false)
        {
            // 적을 향해 오브젝트 발사 
            ShootBullet(targetTr);
        }
        else
        {
            state = State.IDLE;
        }
    }


    int CalcUnitPower()
    {
        if (unitInfo == null)
            return 0;
        // 공격력 계산 ( 기본 공격력 ) + (강화 수 * 등급별 공격력 상승량)
        int upgrade = 0; 
        if(PlayUnitManager.instance != null)
        {
            upgrade = PlayUnitManager.instance.GetUpgradeCount(unitInfo.unitType);
        }

        return unitInfo.attack + (upgrade * unitInfo.additionalAttack);
    }

    float CalcUnitAttackSpeed()
    {
        return unitInfo.attackSpeed + unitInfo.additionalAttackSpeed;
    }


    void ShootBullet(Transform target)
    {
        if (target == null)
            return;

        if(bulletID == "")
        {
            var typeName = "Normal"; 
            if(unitInfo.unitType == UnitType.EXPLOSIVE)
            {
                typeName = "Explosive";
            }
            else if(unitInfo.unitType == UnitType.PIERCE)
            {
                typeName = "Pierce";
            }

            bulletID = "Common_" + typeName + "_Bullet";
        }
        //var bulletObj = Instantiate(bulletPrafab, transform.position, Quaternion.identity);
        var bulletObj = Manager.Instance.Spawn(bulletID);
        if (bulletObj == null) return; 

        bulletObj.transform.position = this.transform.position;
        if (bulletObj.TryGetComponent<BulletObject>(out var bullet))
        {
            bullet.SetUnitType(unitInfo.unitType);
            bullet.SetPower(CalcUnitPower()); 
            bullet.SetTarget(target);
        }

        if(attackCoroutine == null)
            attackCoroutine = StartCoroutine(DelayAttackTime());
    }

    // 딜레이 코루틴
    IEnumerator DelayAttackTime()
    {
        isAttacking = true; 
        yield return new WaitForSeconds(CalcUnitAttackSpeed());
        isAttacking = false;
        attackCoroutine = null; 
    }
    public void SelectUnit()
    {
        if (myObjs.Length <= 0) return;

        myObjs[1].gameObject.SetActive(true);
    }

    public void UnselectUnit()
    {
        if (myObjs.Length <= 0) return;

        myObjs[1].gameObject.SetActive(false);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if(unitInfo != null)
            Gizmos.DrawWireSphere(transform.position, unitInfo.attackRange);
    }

}
