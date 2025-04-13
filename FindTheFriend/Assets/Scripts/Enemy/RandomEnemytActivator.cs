using System.Linq;
using UnityEngine;

public class RandomEnemytActivator : MonoBehaviour
{
    [Header("���������")]
    [Tooltip("����������� ���������� ���������� ��������")]
    public int minActiveObjects = 1;
    [Tooltip("������������ ���������� ���������� ��������")]
    public int maxActiveObjects = 5;

    [Header("������ ��������")]
    [Tooltip("�������� ���� �������, ������� ����� �������� ��������� �������")]
    public GameObject[] objectsToActivate;

    private void Start()
    {
        // �������� �� ���������� ������� ������
        if (objectsToActivate == null || objectsToActivate.Length == 0)
        {
            Debug.LogError("������ �������� ����!", this);
            return;
        }

        if (minActiveObjects < 0 || maxActiveObjects > objectsToActivate.Length || minActiveObjects > maxActiveObjects)
        {
            Debug.LogError("������������ �������� min/max �������� ��������!", this);
            return;
        }

        // ��������� ��� ������� � ������
        foreach (var obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // ���������� ������� �������� �������� (��������� ����� � �������� ���������)
        int objectsToEnable = Random.Range(minActiveObjects, maxActiveObjects + 1);

        // ������������ ������
        System.Random rng = new System.Random();
        var shuffledObjects = objectsToActivate.OrderBy(a => rng.Next()).ToArray();

        // �������� ������ ���������� ��������
        for (int i = 0; i < objectsToEnable; i++)
        {
            if (shuffledObjects[i] != null)
                shuffledObjects[i].SetActive(true);
        }
    }
}
