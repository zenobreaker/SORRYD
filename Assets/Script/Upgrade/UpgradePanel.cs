using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{

    [SerializeField]
    GameObject upgradeGroupUI; 



    // Upgrade ��ư �̺�Ʈ 
    public void UpgradeButtonEventMethod()
    {
        OnOffUpgradeGroupUI(); 
    }

    // UpgradeGroup ����
    public void OnOffUpgradeGroupUI()
    {
        if (upgradeGroupUI == null) return;

        if (upgradeGroupUI.activeInHierarchy)
            upgradeGroupUI.SetActive(false);
        else 
            upgradeGroupUI.SetActive(true); 
    }

}
