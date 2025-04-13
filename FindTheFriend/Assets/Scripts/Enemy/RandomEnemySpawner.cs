using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyActivator : MonoBehaviour
{
    [Header("Settings")]
    [Range(0, 100)] public float spawnChance = 50f;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public bool spawnOnStart = true;

    private List<Transform> _availablePoints = new List<Transform>();
    private GameObject _currentEnemy;

    private void Start()
    {
        if (spawnOnStart) TrySpawnEnemy();
    }

    public void TrySpawnEnemy()
    {
        // Очищаем предыдущего врага
        if (_currentEnemy != null)
        {
            Destroy(_currentEnemy);
        }

        // Обновляем доступные точки
        UpdateAvailablePoints();

        // Проверяем шанс и наличие точек
        if (Random.Range(0f, 100f) > spawnChance || _availablePoints.Count == 0)
        {
            Debug.Log("No enemy spawned");
            return;
        }

        // Выбираем случайную точку
        Transform spawnPoint = _availablePoints[Random.Range(0, _availablePoints.Count)];
        _availablePoints.Remove(spawnPoint);

        // Спавним врага
        _currentEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation,gameObject.transform);
        Debug.Log($"Enemy spawned at {spawnPoint.name}");
    }

    private void UpdateAvailablePoints()
    {
        _availablePoints.Clear();
        foreach (var point in spawnPoints)
        {
            if (point != null) _availablePoints.Add(point);
        }
    }

    public bool IsEnemyActive()
    {
        return _currentEnemy != null && _currentEnemy.activeSelf;
    }

    public EnemyController GetActiveEnemy()
    {
        return _currentEnemy?.GetComponent<EnemyController>();
    }
}