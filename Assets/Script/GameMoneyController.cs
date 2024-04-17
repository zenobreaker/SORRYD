using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMoneyController : MonoBehaviour
{
    [SerializeField]
    Text gameMoneyText; 

    void LateUpdate()
    {
        gameMoneyText.text = GameManager.instance.money.ToString();
    }

}
