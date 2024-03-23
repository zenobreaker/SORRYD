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
    public int attack;              // ���ݷ�
    public int additionalAttack;    // �߰� ���ݷ�
    public float attackSpeed;       // ���ݼӵ�
    public float additionalAttackSpeed; // �߰� ���ݼӵ�
    public float attackRange;       // ���� ��Ÿ�

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


    // ������ ��ġ�� �����ߴ��� �˻� 
    public void CheckArrive()
    {
        if (rb == null)
            return;

        // ��ǥ ������ ���� �ߴ��� �˻� 
        if (Vector2.Distance(transform.position, destination) < 0.02f * moveSpeed)
        {
            Debug.Log("���� : " + Vector2.Distance(transform.position, destination));
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

    // ������ �ڷ�ƾ
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
