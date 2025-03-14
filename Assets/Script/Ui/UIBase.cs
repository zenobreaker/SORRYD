using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    [SerializeField]
    private bool isInit = true; 

    protected virtual void Start()
    {
        if (this.gameObject == null)
            return;

        if (isInit == false)
            return;

        if(this.gameObject.activeSelf == true)
        { this.gameObject.SetActive(false); }
    }


    public void OnOffUI()
    {
        if (this.gameObject == null)
            return;

        if (this.gameObject.activeSelf == true)
        { this.gameObject.SetActive(false); }
        else
        { this.gameObject.SetActive(true); }
    }

}
