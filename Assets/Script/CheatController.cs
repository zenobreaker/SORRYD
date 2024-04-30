using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatController : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField inputField;

    private UnitSpawner unitSpawner;

    private string inputContext;    // 인풋필드에 입력한 텍스트 저장 

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        
        unitSpawner = FindObjectOfType<UnitSpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // 입력한 텍스트 내용을 기반으로 해당 기능 실현
    public void CheckCheatCommand()
    {
        
        inputContext = inputField.text;
        Debug.Log("Writted inputContext : " + inputContext);
        string[] commandSplits = inputContext.Split(' ');

        if(commandSplits.Length <= 0)
        {
            return; 
        }


        string firstKey = commandSplits[0];

        switch(firstKey)
        {
            case "CREATE_UNIT":
            if(commandSplits.Length >= 3)
            {
                CheatCreateUnit(commandSplits[1], commandSplits[2]);
            }
            break; 
        }


        // 입력한 내용 지우기
        inputField.text = "";

    }

    void CheatKeyAnalys(params string[] strings)
    {
        foreach(string key in strings)
        {
            // TODO: 여기서부터 치트작업..

            // 치트 키를 입력받는다
            // 치트 키를 ' ' 로 분리한다
            // 분리한 리스트를 하나씩 가져온다
            // 가져온 키값을 조합하여 해당 함수를 호출한다
            // ex) create unit normal pierce 를 받으면
            // create / unit / normal.. 순으로 분리되며 
            // create 키를 받으면 create 관련 함수 준비 
        }
    }


    void CheatCreateUnit( string unitType, string grade)
    {
        Debug.Assert(unitSpawner != null, "Unit Spawner not finded");

        if (unitType == string.Empty || grade == string.Empty)
            return; 

        string typeCommand = string.Empty; 
        switch(unitType)
        {
            case "explosive":
            typeCommand = "Explosive";
            break;
            case "normal":
            typeCommand = "Normal";
            break;
            case "pirece":
            typeCommand = "Pierce";
            break;

        }

        string gradeCommand = string.Empty;
        switch(grade)
        {
            case "rare":
            gradeCommand = "Rare";
            break;
            case "unique":
            gradeCommand = "Unique";
            break;
            case "legend":
            gradeCommand = "Legend";
            break;
            case "myth":
            gradeCommand = "Myth";
            break;
            case "common":
            gradeCommand = "Common";
            break;
        }

        if (unitType == string.Empty || grade == string.Empty)
            return;

        string commandName = typeCommand + "_" + gradeCommand; 
        unitSpawner.CheatCreateUnit(commandName);
    }
}
