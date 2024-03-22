using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BulletType
{ 
    SINGLE,
    EXPLOSIVE,    
}


// ����� ���� ���ư��� �Ѿ� ������Ʈ 
public class Bullet : MonoBehaviour
{
    public Transform target;
    public Rigidbody2D rigid;

    public float speed; // �Ѿ� �ӵ�
    public int power;     // �Ѿ� ����
    public GameObject bomoEffect; // ���� ����Ʈ 
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
