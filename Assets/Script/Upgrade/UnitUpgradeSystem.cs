using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgradeSystem : MonoBehaviour
{
    // ha.. 
    [SerializeField]
    Button explodeUpgradeButton;
    [SerializeField]
    Text explodeUpgradeText; 
    [SerializeField]
    Button pierceUpgradeButton;
    [SerializeField]
    Text pierceUpgradeText; 
    [SerializeField]
    Button normalUpgradeButton;
    [SerializeField]
    Text normalUpgradeText; 

    int upgradeLevel_Explode = 0;
    int upgradeLevel_pierce = 0;
    int upgradeLevel_normal = 0;

    public double upgrade_rate =  0.1;
    public int upgrade_start_cost = 100;
    public int MAX_LEVEL = 99;

    int usedCost = 0; // ���� �ڽ�Ʈ


    private void OnEnable()
    {
        DrawUpgradeText();
    }

    bool CheckMaxUpgradeLevel(int level)
    {
        if(level >= MAX_LEVEL)
        {
            return true; 
        }

        return false; 
    }

    int CalcUpgradeValue(int prevLevel)
    {
        //���׷��̵� ��� = ���� ������ ��롿(1 + ���׷��̵� ����)
        int cost = (int)(prevLevel  + 1 * (1 + upgrade_rate)) * upgrade_start_cost;

        return cost;
    }

    int CalcUpgradeValueWithType(UnitType type)
    {
        int cost =0; 
        switch(type)
        {
            case UnitType.EXPLOSIVE:
                cost = CalcUpgradeValue(upgradeLevel_Explode);
                break;
            case UnitType.PIERCE:
                cost = CalcUpgradeValue(upgradeLevel_Explode);
                break;
            case UnitType.NORMAL:
                cost = CalcUpgradeValue(upgradeLevel_normal);
                break; 
        }

        return cost; 
    }

    bool CheckUpgradable(UnitType type)
    {
        bool isMaxLevel = false; 
        switch(type)
        {
            case UnitType.EXPLOSIVE:
                isMaxLevel = CheckMaxUpgradeLevel(upgradeLevel_Explode);
                break;
            case UnitType.PIERCE:
                isMaxLevel = CheckMaxUpgradeLevel(upgradeLevel_pierce);
                break;
            case UnitType.NORMAL:
                isMaxLevel = CheckMaxUpgradeLevel(upgradeLevel_normal);
                break;
        }

        // ���� ���� ���� 
        usedCost = CalcUpgradeValueWithType(type);

        int money = GameManager.instance.money;
        if(usedCost > money || isMaxLevel == true)
        {
            if(usedCost > money )
            {
                Debug.Log("�ڽ�Ʈ ����");
            }

            usedCost = 0;
            return false; 
        }

        return true; 
    }

    void IncreseUpgradeValue(UnitType type)
    {
        switch (type)
        {
            case UnitType.EXPLOSIVE:
                upgradeLevel_Explode++;
                break;
            case UnitType.PIERCE:
                upgradeLevel_pierce++;
                break;
            case UnitType.NORMAL:
                upgradeLevel_normal++;
                break;
        }
    }

    public void UpgradeAbility(int type)
    {
        bool isUpgrade = CheckUpgradable((UnitType)type);

        if(isUpgrade)
        {
            // ��� ���� 
            GameManager.instance.UseMoney(usedCost);
            usedCost = 0; // ��������� �ʱ�ȭ
            IncreseUpgradeValue((UnitType)type);
            DrawUpgradeText();
        }
    }


    public void DrawUpgradeText()
    {
        if (explodeUpgradeText != null)
            explodeUpgradeText.text = "+" + upgradeLevel_Explode.ToString();
        if (pierceUpgradeText != null)
            pierceUpgradeText.text = "+" + upgradeLevel_pierce.ToString();
        if (normalUpgradeText != null)
            normalUpgradeText.text = "+" + upgradeLevel_normal.ToString();
    }
}
