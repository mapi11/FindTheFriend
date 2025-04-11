using UnityEngine;

public class DoorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] doorPrefabs; // Массив префабов дверей
    [SerializeField] private Transform[] spawnPoints;  // Массив точек спавна (должны иметь нужный rotation)

    private void Awake()
    {
        SpawnRandomDoor();
    }

    private void SpawnRandomDoor()
    {
        // Проверяем, есть ли префабы и точки спавна
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

        // Выбираем случайную дверь и случайную точку
        GameObject randomDoor = doorPrefabs[Random.Range(0, doorPrefabs.Length)];
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Спавним дверь с позицией и rotation выбранной точки 
        Instantiate(randomDoor, randomSpawnPoint.position, randomSpawnPoint.rotation);
    }
}
