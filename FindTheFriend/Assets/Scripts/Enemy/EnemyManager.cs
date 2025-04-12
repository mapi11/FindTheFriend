using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<EnemyController> _activeEnemies = new List<EnemyController>();

    public void RegisterEnemy(EnemyController enemy)
    {
        if (!_activeEnemies.Contains(enemy))
        {
            _activeEnemies.Add(enemy);
            Debug.Log($"Enemy registered. Total: {_activeEnemies.Count}");
        }
    }

    public void UnregisterEnemy(EnemyController enemy)
    {
        if (_activeEnemies.Contains(enemy))
        {
            _activeEnemies.Remove(enemy);
            Debug.Log($"Enemy unregistered. Total: {_activeEnemies.Count}");
        }
    }

    public bool HasActiveEnemies()
    {
        // ќчистка null-ссылок на случай если враги были удалены
        _activeEnemies.RemoveAll(enemy => enemy == null);
        return _activeEnemies.Count > 0;
    }
}
