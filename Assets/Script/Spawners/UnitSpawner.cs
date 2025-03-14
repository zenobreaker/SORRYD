using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �����ϴ� ������
public class UnitSpawner : MonoBehaviour
{
    public static UnitSpawner Instance;

    public List<string> unitIDList = new List<string>();
    public List<GameObject> spawnPoints = new List<GameObject>();


    public int SPAWN_UNIT_COST = 100;

    public DataController dataController;
    public PlayerUnit playerUnitPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        if (dataController == null)
            dataController = FindObjectOfType<DataController>();
    }


   public void ButtonEventCreateUnit()
    {
        if (dataController == null)
            return;


        var unitID = UnitManager.instance.GetRandomUnitID();
        Debug.Log($"Appear Unit ID : {unitID}");

        CreateUnit(unitID);
    }


    public void CheatCreateUnit(string name)
    {
        CreateUnit(name);
    }



    Vector3 GetEmptySeat(PlayerUnit unit)
    {
        foreach (var spawnPoint in spawnPoints)
        {
            if (IsOccupied(spawnPoint.transform.position) == false)
                return spawnPoint.transform.position;
        }

        return Vector3.zero;
    }

    bool IsOccupied(Vector3 position)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, 0.5f);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Unit"))
            {
                Debug.Log($"{hit.transform.name} is Exist");
                return true;
            }
        }

        return false;
    }

    public void CreateUnit(string name)
    {
        //var unit = Instantiate(playerUnit); 
        int money = GameManager.instance.money;
        if (SPAWN_UNIT_COST > money)
        {
            Debug.Log("�ڽ�Ʈ�� �����Ͽ� ������ ���� �� �����ϴ�.");
            return;
        }

        GameManager.instance.UseMoney(SPAWN_UNIT_COST);

        var unit = Manager.Instance.Spawn(name);

        if (unit.TryGetComponent<PlayerUnit>(out PlayerUnit playerUnit))
        {

            playerUnit.unitInfo = dataController.GetUnitStatInfo(name);

            // ��ȯ�� ����Ʈ�� �ش� ������ �̹� �ִ��� �˻�
            PlayerUnit findUnit = UnitManager.instance.FindUnit(ref playerUnit);
            if (findUnit != null)
            {
                // �����Ѵٸ� �ش� ����Ʈ�� �߰��ϰ� ��
                UnitManager.instance.AddMyUnit(playerUnit);

                // ������ ��ġ�� ��ġ
                // ������ ��ġ ������ ���� (��ġ�� �� ����)
                Vector2 randomOffset;
                do
                {
                    randomOffset = Random.insideUnitCircle * 0.5f; // ������ 0.5 �ȿ��� ���� ��ġ
                } while (Physics2D.OverlapCircle(findUnit.transform.position + (Vector3)randomOffset, 0.2f, 1 << LayerMask.NameToLayer("Unit")));

                playerUnit.transform.position = findUnit.transform.position + (Vector3)randomOffset;
                return;
            }

            Vector3 spawnPosition = GetEmptySeat(playerUnit);
            if (spawnPosition != Vector3.zero)
            {
                playerUnit.transform.position = spawnPosition;
                // ���ָŴ����� �� ���� �߰�
                UnitManager.instance.AddMyUnit(playerUnit);
            }
            else
            {
                Debug.Log("�� �ڸ��� ����");
            }
        }

    }

}
