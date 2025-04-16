using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float damageAmount = 1f;

    [Header("Death Effects")]
    public GameObject deathEffect;
    public AudioClip deathSound;
    [Range(0f, 1f)] public float deathSoundVolume = 0.2f;
    public float effectDestroyDelay = 2f;

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

        // Спавн эффекта частиц с нужным поворотом
        if (deathEffect != null)
        {
            // Создаём кватернион с поворотом 90 градусов по X
            Quaternion effectRotation = Quaternion.Euler(90f, 0f, 0f);

            // Альтернативно, можно комбинировать с текущим поворотом:
            // Quaternion effectRotation = transform.rotation * Quaternion.Euler(90f, 0f, 0f);

            GameObject effect = Instantiate(deathEffect, transform.position, effectRotation);
            Destroy(effect, effectDestroyDelay);
        }

        // Остальной код без изменений
        if (deathSound != null)
        {
            PlayDeathSound(transform.position);
        }

        if (_enemyManager != null)
        {
            _enemyManager.UnregisterEnemy(this);
        }

        gameObject.SetActive(false);
    }

    private void PlayDeathSound(Vector3 position)
    {
        GameObject soundObject = new GameObject("DeathSound");
        soundObject.transform.position = position;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = deathSound;
        audioSource.volume = deathSoundVolume;
        audioSource.spatialBlend = 1f; // 3D звук
        audioSource.Play();

        Destroy(soundObject, deathSound.length);
    }

    public bool IsActiveEnemy()
    {
        return !_wasDefeated && gameObject.activeInHierarchy;
    }

    private void OnDisable()
    {
        if (_enemyManager != null && !_wasDefeated)
        {
            _enemyManager.UnregisterEnemy(this);
        }
    }
}
