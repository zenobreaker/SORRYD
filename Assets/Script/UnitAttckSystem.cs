using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 이 컴포넌트 유닛의 공격의 특별한 이벤트를 부여해준다. 
/// </summary>
public class UnitAttckSystem : MonoBehaviour
{
    private PlayerUnit playerUnit;

    // 이 컴포넌트가 붙은 유닛이 생성되면 
    // 해당 유닛이 공격을 할 때마다 
    // 유닛의 등급 정보에 따라서 공격 능력을 추가로 부여한다. 
    private void Awake()
    {
        playerUnit = GetComponent<PlayerUnit>();   
    }

    private void Start()
    {
       
    }

    // 각 등급별 이벤트 
    private void OnAfterEffectByUnitInfo()
    {

    }



}
