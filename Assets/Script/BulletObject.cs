using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Redcode.Pools;

public enum BulletType
{
    SINGLE,
    EXPLOSIVE,
}


// 대상을 향해 날아가는 총알 오브젝트 
public class BulletObject : ObjectPoolInfo, IPoolObject
{
    public Transform target;
    public SpriteRenderer sprite;

    public float speed; // 총알 속도
    public int power;     // 총알 위력
    public GameObject bomoEffect; // 폭발 이펙트 
    public UnitType unitType;
    public BulletType bulletType;

    public delegate void DoAction();
    public DoAction OnDoAction;

    public void OnCreatedInPool()
    {

    }

    public void OnGettingFromPool()
    {

    }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (sprite != null)
        {
            sprite.flipX = true;
        }
    }

    void Update()
    {
        if (target != null)
        {
            Vector2 dirVec = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(target.position.y, target.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.Translate(speed * Time.deltaTime * dirVec, Space.World);
            if (ArriveToTarget())
            {
                // 오브젝트 반납 
                Manager.Instance.ReturnPool(this);

                // 적에게 데미지를 주는지, 아니면 추가 오브젝트를 생성하는지
                if (target.TryGetComponent(out EnemyController enemy))
                {
                    enemy.Damaged(unitType, power);
                }

                 // 이벤트 실행

                if(OnDoAction != null)
                {
                    OnDoAction?.Invoke();
                }

            }
        }
    }

    public bool ArriveToTarget()
    {
        if (Vector2.Distance(transform.position, target.position) > 0.02f * speed)
        {
            return false;
        }
        return true;
    }

    public void SetUnitType(UnitType type)
    {
        unitType = type;
    }

    public void SetPower(int power)
    {
        this.power = power;
    }

    public void SetTarget(Transform transform)
    {
        if (transform == null) return;

        this.target = transform;
    }

    // 총알 프리팹에 폭발 프리팹 을 넣고
    // 총알에 델리게이트 이벤트를 서넝ㄴ 
    // 유닛이 생성될 때 혹은 총알을 쏠 때 델리게이트에 이벤트를 심어둔다
    // 이벤트는 여러 효과를 구현한다.

}
