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


// ����� ���� ���ư��� �Ѿ� ������Ʈ 
public class BulletObject : ObjectPoolInfo, IPoolObject
{
    public Transform target;
    public SpriteRenderer sprite;

    public float speed; // �Ѿ� �ӵ�
    public int power;     // �Ѿ� ����
    public GameObject bomoEffect; // ���� ����Ʈ 
    public UnitType unitType;
    public BulletType bulletType;

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
        if(sprite != null)
        {
            sprite.flipX = true;
        }
    }

    void Update()
    {
        if(target != null)
        {
            Vector2 dirVec = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(target.position.y, target.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.Translate(speed * Time.deltaTime * dirVec, Space.World); 
            if(ArriveToTarget())
            {
                // ������Ʈ �ݳ� 
                Manager.Instance.ReturnPool(this);

                // ������ �������� �ִ���, �ƴϸ� �߰� ������Ʈ�� �����ϴ���
                 if(target.TryGetComponent(out EnemyController enemy))
                {
                    enemy.Damaged(unitType, power);
                }


            }
        }
    }

    public bool ArriveToTarget()
    {
        if(Vector2.Distance(transform.position, target.position) > 0.02f * speed)
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

}