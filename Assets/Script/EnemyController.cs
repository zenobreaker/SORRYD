using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;

    bool isLive;


    private Vector3 moveDirection;

    private int wayPointCount;  // �̵� ��� ����
    private Transform[] wayPoints; // �̵� ��� �迭; 
    private int currentIndex = 0;   // ���� ��ǥ ���� �ε��� ��
    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void SetUp(Transform[] wayPoints)
    {
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // ���� ��ġ�� wayPonts�� ù ����� ����
        currentIndex = 0;
        transform.position = wayPoints[currentIndex].position;
    }

    public void NextMoveTo()
    {
        // ���� waypoints�� �Ÿ��� 0.02 * spee ���� �۴ٸ� ����
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
}
