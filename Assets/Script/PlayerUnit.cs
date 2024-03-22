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

    // ������ ��ġ�� �����ߴ��� �˻� 
    public void CheckArrive()
    {
        if (rb == null)
            return;

        // ��ǥ ������ ���� �ߴ��� �˻� 
        if (Vector2.Distance(transform.position, destination) > 0.02f * moveSpeed)
        {
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
        if (distance < 10)
        {
            // ��� ��ġ�� �ٸ� ������ �ִ��� Ȯ��
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(destination, 10);


            // �ٸ� ������ ������
            if(collider2Ds.Length > 0)
            {
                // ���� ����� ������ ã�Ƽ� �� ��ó�� �������� ����
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

    // ����� �ڽ��� ���� ������ �ִ��� �˻�
    public void CheckRangeInEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        if(colliders.Length >0)
        {
            SetTarget(colliders[0].transform);
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
        if(targetTr != null)
        {
            // ���� ���� ������Ʈ �߻� 
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
