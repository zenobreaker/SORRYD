using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �� ������Ʈ ������ ������ Ư���� �̺�Ʈ�� �ο����ش�. 
/// </summary>
public class UnitAttckSystem : MonoBehaviour
{
    private PlayerUnit playerUnit;

    // �� ������Ʈ�� ���� ������ �����Ǹ� 
    // �ش� ������ ������ �� ������ 
    // ������ ��� ������ ���� ���� �ɷ��� �߰��� �ο��Ѵ�. 
    private void Awake()
    {
        playerUnit = GetComponent<PlayerUnit>();   
    }

    private void Start()
    {
       
    }

    // �� ��޺� �̺�Ʈ 
    private void OnAfterEffectByUnitInfo()
    {

    }



}
