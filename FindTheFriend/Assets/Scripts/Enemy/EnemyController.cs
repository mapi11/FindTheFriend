using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Settings")]
    public float damageAmount = 1f;
    public GameObject deathEffect;
    public AudioClip deathSound;

    private bool _wasDefeated = false;
    private EnemyManager _enemyManager;

    private void Start()
    {
        _enemyManager = FindObjectOfType<EnemyManager>();
        if (_enemyManager != null)
        {
            _enemyManager.RegisterEnemy(this);
        }
    }

    private void OnMouseDown()
    {
        DefeatEnemy();
    }

    public void DefeatEnemy()
    {
        if (_wasDefeated) return;

        _wasDefeated = true;

        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);

        if (_enemyManager != null)
        {
            _enemyManager.UnregisterEnemy(this);
        }

        gameObject.SetActive(false);
    }

    public bool IsActiveEnemy()
    {
        return !_wasDefeated && gameObject.activeInHierarchy;
    }

    private void OnDisable()
    {
        if (_enemyManager != null)
        {
            _enemyManager.UnregisterEnemy(this);
        }
    }
}
