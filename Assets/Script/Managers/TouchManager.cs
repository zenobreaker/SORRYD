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
            // ù ��° ��ġ �������� 
            Touch touch = Input.GetTouch(0);

            // ��ġ ��ġ�� ȭ�� ��ǥ���� ���� ��ǥ�� ��ȯ
            Vector2 touchPoistion = Camera.main.ScreenToWorldPoint(touch.position);

            Debug.Log("��ġ�� ��ġ : " + touchPoistion);

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

    // ������ ������ ������ �ش� ������ �����ϴ� ���
    public void SelectUnit(Transform transform)
    {
        // ������ ������ ��
        if (selectUnit!= null && selectUnit.TryGetComponent<PlayerUnit>(out PlayerUnit prev))
        {
            prev.UnselectUnit();
        }

        if (transform == null)
        {
            selectUnit = null; 
            return; 
        }

        Debug.Log("���� ���� : " + transform.name);

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



    // ������ ���õȰ� ������ ���� Ŭ���� ������ �̵��ϴ� ��� 
    public void MoveSelectedUnit(Vector2 position)
    {
        if (selectUnit == null)
            return;

        Vector2 vector2 = Camera.main.ScreenToWorldPoint(position);
        Debug.Log("������ ������ �̵���ų ���� ���� ");
        // �ش� ������ ������ ������ �ű⵵�� �Ѵ�.
        var pu = selectUnit.GetComponent<PlayerUnit>();
        if (pu != null)
        {
            pu.MoveTo(vector2);
        }

        // ������ ��� ���ֱ� 
        selectUnit = null;
    }

  
}
