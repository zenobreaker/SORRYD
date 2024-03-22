using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트를 선택하면 해당 오브젝트를 고르는 클래스
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
