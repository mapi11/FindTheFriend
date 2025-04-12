using UnityEngine;
using UnityEngine.SceneManagement;
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

    private Transform saveZonePoint;
    private CameraPointSystem cameraPointSystem;
    private static int savedRoomCount = 0;
    private static int savedHealth = 0; // Добавляем сохранение здоровья

    private void Start()
    {
        cameraPointSystem = GetComponent<CameraPointSystem>();

        // Восстанавливаем сохраненные значения
        if (TryGetComponent<RoomsCounter>(out var roomsCounter))
        {
            roomsCounter.RoomCount = savedRoomCount;
        }
        currentHealth = savedHealth > 0 ? savedHealth : maxHealth; // Восстанавливаем здоровье или ставим максимум

        UpdateHearts();

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

    // Метод для перезагрузки сцены с сохранением RoomCount
    private void ReloadRevive()
    {
        // Сохраняем текущее значение RoomCount
        if (TryGetComponent<RoomsCounter>(out var roomsCounter))
        {
            savedRoomCount = roomsCounter.RoomCount;
        }

        // Перезагружаем сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Новый метод полного исцеления
    public void FullHeal()
    {
        currentHealth = maxHealth;
        UpdateHearts();

        // Сохраняем значения перед перезагрузкой
        if (TryGetComponent<RoomsCounter>(out var roomsCounter))
        {
            savedRoomCount = roomsCounter.RoomCount;
        }
        savedHealth = maxHealth;

        ReloadScene();
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

        // Сохраняем значения перед перезагрузкой
        if (TryGetComponent<RoomsCounter>(out var roomsCounter))
        {
            savedRoomCount = roomsCounter.RoomCount;
        }
        savedHealth = 1; // Всегда 1 HP после лечения

        ReloadScene();
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

    public void Die()
    {
        // Сбрасываем сохраненные значения
        savedRoomCount = 0;
        savedHealth = 0;

        if (saveZonePoint == null)
        {
            Debug.LogError("Cannot teleport - SaveZonePoint not found!");
            return;
        }

        cameraPointSystem.StopAllMovement();
        transform.position = saveZonePoint.position;
        transform.rotation = saveZonePoint.rotation;

        if (spawnPrefab != null)
        {
            Instantiate(spawnPrefab, saveZonePoint.position, saveZonePoint.rotation);
        }

        //ReloadScene(); // Перезагружаем сцену после смерти
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

    [ContextMenu("Test Full Heal")]
    private void TestFullHeal()
    {
        FullHeal();
    }
}