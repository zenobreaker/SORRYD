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

        if (commandSplits.Length <= 0)
        {
            return;
        }

        // �Է� ���� ���� �빮�ڷ� ������
        commandSplits[0].ToUpper();
        if (commandSplits.Length > 1)
            commandSplits[1] = commandSplits[1].ToUpper();

        if (commandSplits.Length > 2)
            commandSplits[2] = commandSplits[2].ToUpper();

        string firstKey = commandSplits[0];
        firstKey = firstKey.ToUpper();

        switch (firstKey)
        {
            case "CREATE_UNIT":
                if (commandSplits.Length >= 3)
                {
                    CheatCreateUnit(commandSplits[1], commandSplits[2]);
                }
                break;
            case "MONEY":
                {
                    CheatCreateMoney();
                }
                break;
        }


        // �Է��� ���� �����
        inputField.text = "";
    }


    void CheatKeyAnalys(params string[] strings)
    {
        foreach (string key in strings)
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
    void CheatCreateMoney()
    {
        GameManager.instance.IncreaseMoney(100);

        // Gamble 
        GameManager.instance.IncreaseGambleMoney(0, 100);
        GameManager.instance.IncreaseGambleMoney(1, 100);
        GameManager.instance.IncreaseGambleMoney(2, 100);
    }

    void CheatCreateUnit(string unitType, string grade)
    {
        Debug.Assert(unitSpawner != null, "Unit Spawner not finded");

        if (unitType == string.Empty || grade == string.Empty)
            return;

        // ���� �̸����� ����ϴ� ���ڿ��� ġȯ
        string typeCommand = string.Empty;
        switch (unitType)
        {
            case "EXPLOSIVE":
                typeCommand = "Explosive";
                break;
            case "NORMAL":
                typeCommand = "Normal";
                break;
            case "PIRECE":
                typeCommand = "Pierce";
                break;

        }

        string gradeCommand = string.Empty;
        switch (grade)
        {
            case "RARE":
                gradeCommand = "Rare";
                break;
            case "UNIQUE":
                gradeCommand = "Unique";
                break;
            case "LEGEND":
                gradeCommand = "Legend";
                break;
            case "MYTH":
                gradeCommand = "Myth";
                break;
            case "COMMON":
                gradeCommand = "Common";
                break;
        }

        if (unitType == string.Empty || grade == string.Empty)
            return;

        string commandName = typeCommand + "_" + gradeCommand;
        unitSpawner.CheatCreateUnit(commandName);
    }
}
