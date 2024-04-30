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

    private string inputContext;    // ��ǲ�ʵ忡 �Է��� �ؽ�Ʈ ���� 

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        
        unitSpawner = FindObjectOfType<UnitSpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // �Է��� �ؽ�Ʈ ������ ������� �ش� ��� ����
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


        // �Է��� ���� �����
        inputField.text = "";

    }

    void CheatKeyAnalys(params string[] strings)
    {
        foreach(string key in strings)
        {
            // TODO: ���⼭���� ġƮ�۾�..

            // ġƮ Ű�� �Է¹޴´�
            // ġƮ Ű�� ' ' �� �и��Ѵ�
            // �и��� ����Ʈ�� �ϳ��� �����´�
            // ������ Ű���� �����Ͽ� �ش� �Լ��� ȣ���Ѵ�
            // ex) create unit normal pierce �� ������
            // create / unit / normal.. ������ �и��Ǹ� 
            // create Ű�� ������ create ���� �Լ� �غ� 
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
