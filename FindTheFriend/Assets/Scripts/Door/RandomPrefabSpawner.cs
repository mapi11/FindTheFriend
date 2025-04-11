using UnityEngine;

public class RandomPrefabSpawner : MonoBehaviour
{
    [Header("Prefabs to Spawn")]
    public GameObject[] prefabsToSpawn; // Массив префабов для спавна

    [Header("Spawn Points")]
    public Transform[] spawnPoints; // Массив точек для спавна

    [Header("Settings")]
    public bool spawnOnStart = true; // Автоматический спавн при старте
    public bool destroyOldInstances = true; // Удалять предыдущие спавны

    private GameObject _currentInstance; // Текущий заспавненный объект

    void Start()
    {
        if (spawnOnStart)
        {
            SpawnRandomPrefab();
        }
    }

    // Вызывайте этот метод для спавна
    public void SpawnRandomPrefab()
    {
        // Проверка на наличие префабов и точек
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

        // Удаляем старый экземпляр если нужно
        if (destroyOldInstances && _currentInstance != null)
        {
            Destroy(_currentInstance);
        }

        // Выбираем случайный префаб
        GameObject prefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];

        // Выбираем случайную точку
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Спавним с ротацией точки
        _currentInstance = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation,gameObject.transform);

        Debug.Log($"Spawned {prefab.name} at {spawnPoint.name}", this);
    }

    // Для вызова из Unity Events
    public void SpawnRandomPrefabExternal()
    {
        SpawnRandomPrefab();
    }
}
