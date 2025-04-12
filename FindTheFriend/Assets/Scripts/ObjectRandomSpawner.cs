using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomSpawner : MonoBehaviour
{
    [Header("Prefabs to Spawn")]
    public GameObject[] Prefabs; // ������� �����

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    [Range(0f, 1f)] public float spawnChance = 0.3f; // 30% ���� ������
    public bool spawnOnStart = true;
    [Tooltip("Min and max objects to spawn (inclusive)")]
    public Vector2Int spawnCountRange = new Vector2Int(0, 5);
    public bool destroyOldInstances = true;

    private List<GameObject> _spawnedInstances = new List<GameObject>();

    void Start()
    {
        if (spawnOnStart)
        {
            SpawnRandomGlasses();
        }
    }

    public void SpawnRandomGlasses()
    {
        if (!ValidateArrays()) return;

        // ������� ���������� �������
        if (destroyOldInstances)
        {
            ClearAllSpawned();
        }

        // ���������� ������� ��������
        int spawnCount = Mathf.Clamp(
            Random.Range(spawnCountRange.x, spawnCountRange.y + 1),
            0,
            spawnPoints.Length
        );

        if (spawnCount == 0) return;

        // ������� ����� ������ ����� ��� ������
        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < spawnCount; i++)
        {
            if (availablePoints.Count == 0) break;

            // ��������� ���� ������ ��� ������� �������
            if (Random.value > spawnChance) continue;

            // �������� ��������� ������ �����
            GameObject prefab = Prefabs[Random.Range(0, Prefabs.Length)];

            // �������� ��������� ����� �� ���������
            int randomPointIndex = Random.Range(0, availablePoints.Count);
            Transform spawnPoint = availablePoints[randomPointIndex];
            availablePoints.RemoveAt(randomPointIndex);

            // ������� ������
            GameObject newInstance = Instantiate(
                prefab,
                spawnPoint.position,
                spawnPoint.rotation,
                transform
            );

            _spawnedInstances.Add(newInstance);
        }

        Debug.Log($"Spawned {_spawnedInstances.Count} glasses objects");
    }

    private bool ValidateArrays()
    {
        if (Prefabs == null || Prefabs.Length == 0)
        {
            Debug.LogError("No glasses prefabs assigned!", this);
            return false;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!", this);
            return false;
        }

        return true;
    }

    public void ClearAllSpawned()
    {
        foreach (var obj in _spawnedInstances)
        {
            if (obj != null) Destroy(obj);
        }
        _spawnedInstances.Clear();
    }

    // ��� ������������ ����� � ���������
    private void OnDrawGizmosSelected()
    {
        if (spawnPoints != null)
        {
            Gizmos.color = Color.blue;
            foreach (var point in spawnPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawWireSphere(point.position, 0.3f);
                    Gizmos.DrawIcon(point.position + Vector3.up * 0.5f, "GlassesIcon");
                }
            }
        }
    }
}
