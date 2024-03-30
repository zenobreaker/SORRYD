using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using Redcode.Pools;



public enum State
{
    IDLE, 
    MOVE,
    ATTACK, 
}


public class PlayerUnit : ObjectPoolInfo, IPoolObject
{
    public UnitStatInfo unitInfo;
    public int upgradeCount; 

    public State state; 

    public int moveSpeed;

    public ObjectPoolInfo bulletObject; 
    public Rigidbody2D rb;
    private Transform targetTr; 

    Vector2 destination = Vector2.zero;
    Coroutine attackCoroutine;
    public LayerMask targetLayer;
    bool isAttacking = false; 

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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


    // ������ ��ġ�� �����ߴ��� �˻� 
    public void CheckArrive()
    {
        if (rb == null)
            return;

        // ��ǥ ������ ���� �ߴ��� �˻� 
        if (Vector2.Distance(transform.position, destination) < 0.02f * moveSpeed)
        {
            //Debug.Log("���� : " + Vector2.Distance(transform.position, destination));
            state = State.IDLE;
            return;
        }
    }

    // ������ ��ġ�� ��ǥ����
    public void MoveTo(Vector2 destination)
    {
        state = State.MOVE;

        // ��� ��ġ�� ���� ��ġ ������ �Ÿ��� ���
        float distance = Vector2.Distance(transform.position, destination);

        // ��� ��ġ�� �� �� ��ġ ������ �Ÿ��� ���� �Ÿ� �̳��� ��
        if (distance < 2)
        {
            // ��� ��ġ�� �ٸ� ������ �ִ��� Ȯ��
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(destination, 2);


            // �ٸ�  ������ ������
            if(collider2Ds.Length > 0)
            {
                // ���� ����� ������ ã�Ƽ� �� ��ó�� �������� ����
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
                    Debug.Log("���ó��");
                    destination = closestPosition;
                }
            }
        }

        this.destination = destination;
    }

    // ����� �ڽ��� ���� ������ �ִ��� �˻�
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

    // ����� Ÿ����
    public void SetTarget(Transform target)
    {
        targetTr = target;
        state = State.ATTACK;
    }

    // ����
    public void Attack()
    {
        if(targetTr != null && isAttacking == false)
        {
            // ���� ���� ������Ʈ �߻� 
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
        // ���ݷ� ��� ( �⺻ ���ݷ� ) + (��ȭ �� * ��޺� ���ݷ� ��·�)
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
        if (bulletObject == null || target == null)
            return;

        //var bulletObj = Instantiate(bulletPrafab, transform.position, Quaternion.identity);
        var bulletObj = Manager.Instance.Spawn(bulletObject.idName);
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

    // ������ �ڷ�ƾ
    IEnumerator DelayAttackTime()
    {
        isAttacking = true; 
        yield return new WaitForSeconds(CalcUnitAttackSpeed());
        isAttacking = false;
        attackCoroutine = null; 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, unitInfo.attackRange);
    }

  
}