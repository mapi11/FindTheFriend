using System.Collections.Generic;
using UnityEngine;

public class BatteryRandomSpawner : MonoBehaviour
{
    [Header("Prefabs to Spawn")]
    public GameObject[] prefabsToSpawn;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    public bool spawnOnStart = true;
    [Tooltip("Min and max objects to spawn (inclusive)")]
    public Vector2Int spawnCountRange = new Vector2Int(0, 5);
    public bool destroyOldInstances = true;

    private List<GameObject> _spawnedInstances = new List<GameObject>();

    void Start()
    {
        if (spawnOnStart)
        {
            SpawnRandomPrefabs();
        }
    }

    public void SpawnRandomPrefabs()
    {
        if (!ValidateArrays()) return;

        // Очищаем предыдущие объекты
        if (destroyOldInstances)
        {
            ClearAllSpawned();
        }

        // Определяем сколько спавнить
        int spawnCount = Mathf.Clamp(
            Random.Range(spawnCountRange.x, spawnCountRange.y + 1),
            0,
            spawnPoints.Length // Не больше чем точек спавна
        );

        if (spawnCount == 0) return;

        // Создаем копию списка точек для работы
        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < spawnCount; i++)
        {
            if (availablePoints.Count == 0) break;

            // Выбираем случайный префаб
            GameObject prefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];

            // Выбираем случайную точку из доступных
            int randomPointIndex = Random.Range(0, availablePoints.Count);
            Transform spawnPoint = availablePoints[randomPointIndex];
            availablePoints.RemoveAt(randomPointIndex); // Удаляем использованную точку

            // Создаем объект
            GameObject newInstance = Instantiate(
                prefab,
                spawnPoint.position,
                spawnPoint.rotation,
                transform
            );

            _spawnedInstances.Add(newInstance);
        }

        Debug.Log($"Spawned {spawnCount} objects at unique positions");
    }

    private bool ValidateArrays()
    {
        if (prefabsToSpawn == null || prefabsToSpawn.Length == 0)
        {
            Debug.LogError("No prefabs assigned in prefabsToSpawn array!", this);
            return false;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned in spawnPoints array!", this);
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

    // Для визуализации точек в редакторе
    private void OnDrawGizmosSelected()
    {
        if (spawnPoints != null)
        {
            Gizmos.color = Color.cyan;
            foreach (var point in spawnPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawWireSphere(point.position, 0.3f);
                }
            }
        }
    }
}
