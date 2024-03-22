using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum UnitType
{
    SPEED_TYPE,
    POWER_TYPE,
}

public enum State
{
    IDLE, 
    MOVE,
    ATTACK, 
}


public class PlayerUnit : MonoBehaviour
{
    public int attack;              // 공격력
    public int additionalAttack;    // 추가 공격력
    public float attackSpeed;       // 공격속도
    public float additionalAttackSpeed; // 추가 공격속도
    public float attackRange;       // 공격 사거리

    public int rank;
    public int value; 


    public UnitType type;

    public State state; 

    public int moveSpeed;

    public GameObject bulletPrafab; 
    public Rigidbody2D rb;
    private Transform targetTr; 

    Vector2 destination = Vector2.zero;
    Coroutine attackCoroutine; 

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

        }
    }

    private void FixedUpdate()
    {
        if(rb != null && state == State.MOVE)
        {
            rb.MovePosition(rb.position + destination);
            CheckArrive();
        }
    }

    // 지정한 위치에 도착했는지 검사 
    public void CheckArrive()
    {
        if (rb == null)
            return;

        // 목표 지점에 도착 했는지 검사 
        if (Vector2.Distance(transform.position, destination) > 0.02f * moveSpeed)
        {
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
        if (distance < 10)
        {
            // 대상 위치에 다른 유닛이 있는지 확인
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(destination, 10);


            // 다른 유닛이 있으면
            if(collider2Ds.Length > 0)
            {
                // 가장 가까운 유닛을 찾아서 그 근처를 도착지로 조정
                Vector2 closestPosition = collider2Ds[0].transform.position;
                foreach(var collider in collider2Ds)
                {
                    float distToTarget = Vector2.Distance(collider.transform.position, destination);
                    float distToClosest = Vector2.Distance(closestPosition, destination);
                    if (distToTarget < distToClosest)
                    {
                        closestPosition = collider.transform.position;
                    }
                }
                destination = closestPosition;
            }
        }

        this.destination = destination;
    }

    // 대상이 자신의 공격 범위에 있는지 검사
    public void CheckRangeInEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        if(colliders.Length >0)
        {
            SetTarget(colliders[0].transform);
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
        if(targetTr != null)
        {
            // 적을 향해 오브젝트 발사 
        }
        else
        {
            state = State.IDLE;
        }
    }

    IEnumerator ShootBullet(Transform target)
    {
        if (bulletPrafab == null || target == null)
            yield break; 

        
    }
}
