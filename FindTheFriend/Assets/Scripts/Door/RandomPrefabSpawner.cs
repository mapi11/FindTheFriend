using UnityEngine;

public class RandomPrefabSpawner : MonoBehaviour
{
    [Header("Prefabs to Spawn")]
    public GameObject[] prefabsToSpawn; // ������ �������� ��� ������

    [Header("Spawn Points")]
    public Transform[] spawnPoints; // ������ ����� ��� ������

    [Header("Settings")]
    public bool spawnOnStart = true; // �������������� ����� ��� ������
    public bool destroyOldInstances = true; // ������� ���������� ������

    private GameObject _currentInstance; // ������� ������������ ������

    void Start()
    {
        if (spawnOnStart)
        {
            SpawnRandomPrefab();
        }
    }

    // ��������� ���� ����� ��� ������
    public void SpawnRandomPrefab()
    {
        // �������� �� ������� �������� � �����
        if (prefabsToSpawn == null || prefabsToSpawn.Length == 0)
        {
            Debug.LogError("No prefabs assigned in prefabsToSpawn array!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned in spawnPoints array!");
            return;
        }

        // ������� ������ ��������� ���� �����
        if (destroyOldInstances && _currentInstance != null)
        {
            Destroy(_currentInstance);
        }

        // �������� ��������� ������
        GameObject prefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];

        // �������� ��������� �����
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // ������� � �������� �����
        _currentInstance = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation,gameObject.transform);

        Debug.Log($"Spawned {prefab.name} at {spawnPoint.name}", this);
    }

    // ��� ������ �� Unity Events
    public void SpawnRandomPrefabExternal()
    {
        SpawnRandomPrefab();
    }
}
