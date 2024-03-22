using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BulletType
{ 
    SINGLE,
    EXPLOSIVE,    
}


// 대상을 향해 날아가는 총알 오브젝트 
public class Bullet : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D rigid;

    public float speed; // 총알 속도
    public int power;     // 총알 위력
    public GameObject bomoEffect; // 폭발 이펙트 
    public BulletType type;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ArriveToTarget()
    {
        if(Vector2.Distance(transform.position, target.position) < 0.02f * speed)
        {
            return false;
        }
        return true; 
    }

    public void MoveTo()
    {

    }
}
