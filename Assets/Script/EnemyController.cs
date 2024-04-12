using Redcode.Pools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DefenseType
{
    IMPACT,         // 충격
    SPECIAL,        // 특수
    MANUFACTURING,  // 가공
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

    // 적이 죽었을 때 발생하는 이벤트
    public event EventHandler EnemyDied;

    private Vector3 moveDirection;

    private int wayPointCount;  // 이동 경로 개수
    private Transform[] wayPoints; // 이동 경로 배열; 
    private int currentIndex = 0;   // 현재 목표 지점 인덱스 값

    public DefenseType defenseType;

    public int dropCoin;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // 체력이 0이하가 되면 매니저에게 반환
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

        // 현재 위치를 wayPonts의 첫 번재로 설정
        currentIndex = 0;
        transform.position = wayPoints[currentIndex].position;
      
        currentHP = hp;
    }

    public void NextMoveTo()
    {
        // 현재 waypoints와 거리가 0.02 * speed 보다 작다면 실행
        // 이유는 속도가 빠르면 한 프레임에 0.02보다 크게 움직이기 때문에 
        // if문에 걸리지 않고 경로를 탈주하는 오브젝트가 발생할 수 있다. 
        if(Vector3.Distance(transform.position, wayPoints[currentIndex].position) > 0.02f * speed)
        {
            return; 
        }

        //아직 이동할 wayPoints 가 남아 있다면
        if (currentIndex < wayPointCount - 1)
        {
            currentIndex++;
        }
        else
        {
            currentIndex = 0; // 0으로 초기화 
        }
    }

    public void Damaged(UnitType unitType, int value)
    {
        switch(unitType)
        {
            case UnitType.EXPLOSIVE:
                // 폭발형 공격은 충격방어에 효과적
                // 대신 가공 방어엔 취약 
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
                // 관통형 공격은 특수방어에 효과적 
                // 대신 충격 방어엔 취약
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
                // 통상형 공격은 모든 방어타입에 한배로 공격
                // 대신 자체적인 위력이 낮음
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
