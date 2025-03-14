using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GambleSlot : UIBase
{
    [SerializeField]
    private int RequireValue;

    [SerializeField]
    int type = 0;

    [SerializeField]
    private Button button;


    protected override void Start()
    {
        base.Start();

        // 먼저 내부에서 호출 
        OnGambleMoneyChange(); 
        GameManager.instance.OnGambleMoneyChange += OnGambleMoneyChange;
    }



    public void OnGameble()
    {
        if (GameManager.instance.CanGambleMoney(type, RequireValue) == false)
            return;

        if (type == 0)
            GambleManager.instance.OnGamble_Normal();
        else if(type == 1)
            GambleManager.instance.OnGamble_Special();
        else 
            GambleManager.instance.OnGamble_Mythtic();
    }



    public void OnGambleMoneyChange()
    {
        int currentValue = GameManager.instance.gamble_Normal_Money;
        if (type == 1)
            currentValue = GameManager.instance.gamble_Rare_Money;
        else if(type == 2)
            currentValue = GameManager.instance.gamble_Legend_Money;

        if (currentValue < RequireValue)
            button.interactable = false;
        else
            button.interactable = true;
    }
}
