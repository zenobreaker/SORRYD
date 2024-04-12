using Redcode.Pools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DefenseType
{
    IMPACT,         // ���
    SPECIAL,        // Ư��
    MANUFACTURING,  // ����
}

public interface IDamageable
{
    public void Damaged(UnitType unitType, int value);
}

public class EnemyController : ObjectPoolInfo, IPoolObject, IDamageable
{
    public float speed;
    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;

    bool isLive;


    public int hp;
    public int currentHP;

    // ���� �׾��� �� �߻��ϴ� �̺�Ʈ
    public event EventHandler EnemyDied;

    private Vector3 moveDirection;

    private int wayPointCount;  // �̵� ��� ����
    private Transform[] wayPoints; // �̵� ��� �迭; 
    private int currentIndex = 0;   // ���� ��ǥ ���� �ε��� ��

    public DefenseType defenseType;

    public int dropCoin;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // ü���� 0���ϰ� �Ǹ� �Ŵ������� ��ȯ
        if(currentHP <= 0)
        {
            OnEnemyDied();
        }
    }

    private void FixedUpdate()
    {
        Vector2 wayPoint = new Vector2(wayPoints[currentIndex].position.x, wayPoints[currentIndex].position.y); 
        Vector2 dirVec = (wayPoint - rigid.position).normalized;
        Vector2 nextVec = speed * Time.fixedDeltaTime * dirVec;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;

        NextMoveTo(); 
    }

    public void OnCreatedInPool()
    {
        
    }

    public void OnGettingFromPool()
    {
    }

    public void SetUp(Transform[] wayPoints)
    {
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // ���� ��ġ�� wayPonts�� ù ����� ����
        currentIndex = 0;
        transform.position = wayPoints[currentIndex].position;
      
        currentHP = hp;
    }

    public void NextMoveTo()
    {
        // ���� waypoints�� �Ÿ��� 0.02 * speed ���� �۴ٸ� ����
        // ������ �ӵ��� ������ �� �����ӿ� 0.02���� ũ�� �����̱� ������ 
        // if���� �ɸ��� �ʰ� ��θ� Ż���ϴ� ������Ʈ�� �߻��� �� �ִ�. 
        if(Vector3.Distance(transform.position, wayPoints[currentIndex].position) > 0.02f * speed)
        {
            return; 
        }

        //���� �̵��� wayPoints �� ���� �ִٸ�
        if (currentIndex < wayPointCount - 1)
        {
            currentIndex++;
        }
        else
        {
            currentIndex = 0; // 0���� �ʱ�ȭ 
        }
    }

    public void Damaged(UnitType unitType, int value)
    {
        switch(unitType)
        {
            case UnitType.EXPLOSIVE:
                // ������ ������ ��ݹ� ȿ����
                // ��� ���� �� ��� 
                if( defenseType  == DefenseType.IMPACT)
                {
                    value *= (int)2.0f; 
                }
                else if(defenseType == DefenseType.MANUFACTURING)
                {
                    value *= (int)0.5f;
                }

                break;
            case UnitType.PIERCE:
                // ������ ������ Ư���� ȿ���� 
                // ��� ��� �� ���
                if (defenseType == DefenseType.SPECIAL)
                {
                    value *= (int)2.0f;
                }
                else if (defenseType == DefenseType.IMPACT)
                {
                    value = Mathf.RoundToInt(value * 0.5f);
                }
                break;
            case UnitType.NORMAL:
                // ����� ������ ��� ���Ÿ�Կ� �ѹ�� ����
                // ��� ��ü���� ������ ����
                break;
            default:
                break; 
        }

        currentHP -= value;
    }

    public void OnEnemyDied()
    {
        EnemyDied?.Invoke(this, EventArgs.Empty);
        DropCoin();
    }

    void DropCoin()
    {
        GameManager.instance.money += dropCoin;
    }
}
