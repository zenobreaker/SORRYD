using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum BulletType
{ 
    SINGLE,
    EXPLOSIVE,    
}


// 대상을 향해 날아가는 총알 오브젝트 
public class BulletObject : MonoBehaviour
{
    public Transform target;
    public SpriteRenderer sprite;
    

    public float speed; // 총알 속도
    public int power;     // 총알 위력
    public GameObject bomoEffect; // 폭발 이펙트 
    public BulletType type;


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>(); 
    }
    // Start is called before the first frame update
    void Start()
    {
        if(sprite != null)
        {
            sprite.flipX = true;
        }
    }

    // Update is called once per frame
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
                // 총알 삭제
                Destroy(this.gameObject);
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

    public void SetTarget(Transform transform)
    {
        if (transform == null) return;

        this.target = transform;
    }
}
