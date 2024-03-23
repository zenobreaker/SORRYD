using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

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

    bool isAttacking = false; 

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isAttacking = false; 
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


    public void UnitSetUp()
    {
        state = State.IDLE; 

    }


    // 지정한 위치에 도착했는지 검사 
    public void CheckArrive()
    {
        if (rb == null)
            return;

        // 목표 지점에 도착 했는지 검사 
        if (Vector2.Distance(transform.position, destination) < 0.02f * moveSpeed)
        {
            Debug.Log("도착 : " + Vector2.Distance(transform.position, destination));
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        if(colliders.Length >0)
        {
            foreach(var collider in colliders)
            {
                if(collider.transform.CompareTag("Enemy"))
                {
                    SetTarget(colliders[0].transform);
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

    void ShootBullet(Transform target)
    {
        if (bulletPrafab == null || target == null)
            return;

        var bulletObj = Instantiate(bulletPrafab, transform.position, Quaternion.identity);

        if(bulletObj.TryGetComponent<Bullet>(out var bullet))
        {
            bullet.SetTarget(target);
        }

        if(attackCoroutine == null)
            attackCoroutine = StartCoroutine(DelayAttackTime());
    }

    // 딜레이 코루틴
    IEnumerator DelayAttackTime()
    {
        isAttacking = true; 
        yield return new WaitForSeconds(attackSpeed);
        isAttacking = false;
        attackCoroutine = null; 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
