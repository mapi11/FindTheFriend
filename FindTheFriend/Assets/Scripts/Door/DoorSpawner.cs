using UnityEngine;

public class DoorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] doorPrefabs; // ������ �������� ������
    [SerializeField] private Transform[] spawnPoints;  // ������ ����� ������ (������ ����� ������ rotation)

    private void Awake()
    {
        SpawnRandomDoor();
    }

    private void SpawnRandomDoor()
    {
        // ���������, ���� �� ������� � ����� ������
        if (doorPrefabs == null || doorPrefabs.Length == 0)
        {
            Debug.LogError("No door prefabs assigned!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        // �������� ��������� ����� � ��������� �����
        GameObject randomDoor = doorPrefabs[Random.Range(0, doorPrefabs.Length)];
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // ������� ����� � �������� � rotation ��������� ����� 
        Instantiate(randomDoor, randomSpawnPoint.position, randomSpawnPoint.rotation);
    }
}
