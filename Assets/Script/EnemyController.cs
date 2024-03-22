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

    private int wayPointCount;  // 이동 경로 개수
    private Transform[] wayPoints; // 이동 경로 배열; 
    private int currentIndex = 0;   // 현재 목표 지점 인덱스 값
    

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

        // 현재 위치를 wayPonts의 첫 번재로 설정
        currentIndex = 0;
        transform.position = wayPoints[currentIndex].position;
    }

    public void NextMoveTo()
    {
        // 현재 waypoints와 거리가 0.02 * spee 보다 작다면 실행
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
}
