using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    [SerializeField] public int currentHealth;

    [Header("Heart References")]
    public AnimatedHeart[] hearts;

    [Header("Death Settings")]
    public GameObject spawnPrefab; // Префаб для спавна при смерти (опционально)

    private Transform saveZonePoint; // Точка для телепортации
    private CameraPointSystem cameraPointSystem;

    private void Start()
    {
        cameraPointSystem = GetComponent<CameraPointSystem>();
        currentHealth = maxHealth;
        UpdateHearts();

        // Находим точку сохранения по имени
        GameObject saveZone = GameObject.Find("SaveZonePoint");
        if (saveZone != null)
        {
            saveZonePoint = saveZone.transform;
        }
        else
        {
            Debug.LogError("SaveZonePoint not found in scene!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Heal();
        }
    }

    // Новый метод полного исцеления
    public void FullHeal()
    {
        currentHealth = maxHealth;
        UpdateHearts();
        Debug.Log("Персонаж полностью исцелен!");
    }

    public void TakeDamage(int amount = 1)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        UpdateHearts();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount = 1)
    {
        if (currentHealth >= maxHealth) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
            {
                hearts[i].SetActive(i < currentHealth);
            }
        }
    }

    private void Die()
    {
        if (saveZonePoint == null)
        {
            Debug.LogError("Cannot teleport - SaveZonePoint not found!");
            return;
        }

        cameraPointSystem.StopAllMovement();

        // Телепортируем игрока
        transform.position = saveZonePoint.position;
        transform.rotation = saveZonePoint.rotation;

        // Спавним дополнительный префаб (если задан)
        if (spawnPrefab != null)
        {
            Instantiate(spawnPrefab, saveZonePoint.position, saveZonePoint.rotation);
        }

        Debug.Log("Player teleported to save zone!");
    }

    [ContextMenu("Test Damage")]
    private void TestDamage()
    {
        TakeDamage();
    }

    [ContextMenu("Test Heal")]
    private void TestHeal()
    {
        Heal();
    }

    // Добавляем контекстное меню для тестирования полного исцеления
    [ContextMenu("Test Full Heal")]
    private void TestFullHeal()
    {
        FullHeal();
    }
}