using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{

    [SerializeField]
    Text gameMoneyText; 


    private void LateUpdate()
    {
        DrawGameMoney();
    }


    void DrawGameMoney()
    {
        if (gameMoneyText == null)
            return;


        gameMoneyText.text = GameManager.instance.money.ToString();
    }
}
