using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������Ʈ�� �����ϸ� �ش� ������Ʈ�� ���� Ŭ����
public class ObjectDetecter : MonoBehaviour
{
    private Camera camra;
    private Ray ray;
    private RaycastHit hit;

    private void Awake()
    {
        camra = Camera.main;
    }

}
