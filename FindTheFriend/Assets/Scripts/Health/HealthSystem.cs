using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Heart References")]
    public AnimatedHeart[] hearts;

    [Header("Death Settings")]
    public GameObject spawnPrefab; // ������ ��� ������ ��� ������ (�����������)

    private Transform saveZonePoint;
    private CameraPointSystem cameraPointSystem;
    private static int savedRoomCount = 0;
    private static int savedHealth = 0; // ��������� ���������� ��������

    //Gnome attack
    [Header("Gnomes attack")]
    [SerializeField] private GameObject prefabToSpawn; // ������ ��� ������
    [SerializeField] private Transform spawnParent;    // �������� � ��������
    [SerializeField] private Vector3 spawnOffset;      // �������� �� ������� ������

    [Header("Timing Settings")]
    [SerializeField] private float destroyDelay = 2f;  // �������� ����� ���������

    FlashlightSystem flashlightSystem;


    private void Start()
    {
        flashlightSystem = FindAnyObjectByType<FlashlightSystem>();
        cameraPointSystem = GetComponent<CameraPointSystem>();

        // ��������������� ����������� ��������
        if (TryGetComponent<RoomsCounter>(out var roomsCounter))
        {
            roomsCounter.RoomCount = savedRoomCount;
        }
        currentHealth = savedHealth > 0 ? savedHealth : maxHealth; // ��������������� �������� ��� ������ ��������

        UpdateHearts();
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

    // ����� ��� ������������ ����� � ����������� RoomCount
    private void ReloadRevive()
    {
        // ��������� ������� �������� RoomCount
        if (TryGetComponent<RoomsCounter>(out var roomsCounter))
        {
            savedRoomCount = roomsCounter.RoomCount;
        }

        // ������������� �����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //private void ReloadScene()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    // ����� ����� ������� ���������
    public void FullHealRevive()
    {
        currentHealth = maxHealth;
        UpdateHearts();

        // ��������� �������� ����� �������������
        if (TryGetComponent<RoomsCounter>(out var roomsCounter))
        {
            savedRoomCount = roomsCounter.RoomCount;
        }
        savedHealth = maxHealth;

        //ReloadScene();
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

    public void HealRevive(int amount = 1)
    {
        if (currentHealth >= maxHealth) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHearts();

        // ��������� �������� ����� �������������
        if (TryGetComponent<RoomsCounter>(out var roomsCounter))
        {
            savedRoomCount = roomsCounter.RoomCount;
        }
        savedHealth = 1; // ������ 1 HP ����� �������

        //ReloadScene();
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

    public void Die()
    {
        flashlightSystem.batteryCharge = 0;
        GmomeAttack();

        GameObject saveZone = GameObject.Find("SaveZonePoint");
        if (saveZone != null)
        {
            saveZonePoint = saveZone.transform;
        }
        else
        {
            Debug.LogError("SaveZonePoint not found in scene!");
        }

        // ���������� ����������� ��������
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

        //ReloadScene(); // ������������� ����� ����� ������
    }

    [ContextMenu("Test Damage")]
    private void TestDamage()
    {
        TakeDamage();
    }

    [ContextMenu("Test Heal")]
    private void TestHeal()
    {
        HealRevive();
    }

    [ContextMenu("Test Full Heal")]
    private void TestFullHeal()
    {
        FullHealRevive();
    }

    // ����� ��� ������ �� ������ �������� ��� ��������
    public void GmomeAttack()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("Prefab to spawn is not assigned!", this);
            return;
        }

        // ���������� ������� ������
        //Vector3 spawnPosition = transform.position + spawnOffset;

        //// ������� ������
        //GameObject spawnedObject = Instantiate(
        //    prefabToSpawn,
        //    spawnPosition,
        //    Quaternion.identity,
        //    spawnParent);

        // ���������� ������� ������
        //Vector3 spawnPosition = transform.position + spawnOffset;

        // ������� ������
        GameObject spawnedObject = Instantiate(prefabToSpawn,spawnParent.transform);

        // ���������� ����� �������� �����
        Destroy(spawnedObject, destroyDelay);
    }
}