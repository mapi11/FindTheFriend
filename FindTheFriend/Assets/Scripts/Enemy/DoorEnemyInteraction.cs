using System.Collections.Generic;
using UnityEngine;

public class DoorEnemyInteraction : MonoBehaviour
{
    [Header("Settings")]
    public int damageAmount = 1;
    public ParticleSystem enterEffect;

    public EnemyManager _enemyManager;
    private HealthSystem _healthSystem;

    private void Awake()
    {
        _enemyManager = FindObjectOfType<EnemyManager>();
        _healthSystem = FindObjectOfType<HealthSystem>();

        Debug.Log($"DoorInteraction initialized. " +
                 $"EnemyManager: {_enemyManager != null}, " +
                 $"HealthSystem: {_healthSystem != null}");
    }

    public void OnPlayerEntered()
    {
        Debug.Log("--- Door Entered Event Triggered ---");

        if (_enemyManager == null)
        {
            Debug.LogWarning("EnemyManager not found!");
            return;
        }

        if (_healthSystem == null)
        {
            Debug.LogWarning("HealthSystem not found!");
            return;
        }

        bool hasEnemies = _enemyManager.HasActiveEnemies();
        Debug.Log($"Active enemies present: {hasEnemies}");

        if (hasEnemies)
        {
            _healthSystem.TakeDamage(damageAmount);

            if (enterEffect != null)
            {
                enterEffect.Play();
            }

            Debug.Log($"Damage applied! Current HP: {_healthSystem.currentHealth}");
        }
    }

    // Для триггерного варианта
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered door trigger");
            OnPlayerEntered();
        }
    }
}
