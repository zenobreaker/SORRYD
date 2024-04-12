using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{

    [SerializeField]
    GameObject upgradeGroupUI; 



    // Upgrade 버튼 이벤트 
    public void UpgradeButtonEventMethod()
    {
        OnOffUpgradeGroupUI(); 
    }

    // UpgradeGroup 열기
    public void OnOffUpgradeGroupUI()
    {
        if (upgradeGroupUI == null) return;

        if (upgradeGroupUI.activeInHierarchy)
            upgradeGroupUI.SetActive(false);
        else 
            upgradeGroupUI.SetActive(true); 
    }

}
