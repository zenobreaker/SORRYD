using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.UIElements;

public class TouchManager : MonoBehaviour
{
    public static TouchManager instance;

    private Camera mainCamera;
    private Ray ray;
 
    public GameObject selectUnit;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        mainCamera = Camera.main;
    }


    public void Update()
    {
        if (Input.touchCount > 0)
        {
            // 첫 번째 터치 가져오기 
            Touch touch = Input.GetTouch(0);

            // 터치 위치를 화면 좌표에서 월드 좌표로 반환
            Vector2 touchPoistion = Camera.main.ScreenToWorldPoint(touch.position);

            Debug.Log("터치한 위치 : " + touchPoistion);

        }

        if (Input.GetMouseButtonDown(0) == true)
        {
            Vector2 mousePosition = Input.mousePosition;
            //
            var worldPoint = mainCamera.ScreenToWorldPoint(mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(worldPoint);

            if (hit != null && hit.transform.CompareTag("Unit"))
            {
                SelectUnit(hit.transform);
            }
            else if(hit != null && hit.transform.CompareTag("Ground"))
            {
                MoveSelectedUnit(mousePosition);
            }
        }
    }

    // 선택한 유닛이 없으면 해당 유닛을 선택하는 기능
    public void SelectUnit(Transform transform)
    {
        // 이전에 선택한 놈
        if (selectUnit!= null && selectUnit.TryGetComponent<PlayerUnit>(out PlayerUnit prev))
        {
            prev.UnselectUnit();
        }

        if (transform == null)
        {
            selectUnit = null; 
            return; 
        }

        Debug.Log("유닛 선택 : " + transform.name);

        if (transform.gameObject.TryGetComponent<PlayerUnit>(out var unit))
        {
            unit.SelectUnit();
            selectUnit = transform.gameObject;
        }
    }

    public PlayerUnit GetSelectUnit()
    {
        if (selectUnit == null)
            return null; 
         
        if(selectUnit.TryGetComponent<PlayerUnit>(out PlayerUnit unit))
        {
            return unit;
        }

        return null;
    }



    // 유닛이 선택된게 있으면 다음 클릭한 곳으로 이동하는 기능 
    public void MoveSelectedUnit(Vector2 position)
    {
        if (selectUnit == null)
            return;

        Vector2 vector2 = Camera.main.ScreenToWorldPoint(position);
        Debug.Log("선택한 유닛을 이동시킬 지형 선택 ");
        // 해당 유닛을 선택한 곳으로 옮기도록 한다.
        var pu = selectUnit.GetComponent<PlayerUnit>();
        if (pu != null)
        {
            pu.MoveTo(vector2);
        }

        // 선택한 대상 없애기 
        selectUnit = null;
    }

  
}
