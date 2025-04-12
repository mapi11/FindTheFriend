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
    public GameObject spawnPrefab; // ������ ��� ������ ��� ������ (�����������)

    private Transform saveZonePoint; // ����� ��� ������������
    private CameraPointSystem cameraPointSystem;

    private void Start()
    {
        cameraPointSystem = GetComponent<CameraPointSystem>();
        currentHealth = maxHealth;
        UpdateHearts();

        // ������� ����� ���������� �� �����
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

    // ����� ����� ������� ���������
    public void FullHeal()
    {
        currentHealth = maxHealth;
        UpdateHearts();
        Debug.Log("�������� ��������� �������!");
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

        // ������������� ������
        transform.position = saveZonePoint.position;
        transform.rotation = saveZonePoint.rotation;

        // ������� �������������� ������ (���� �����)
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

    // ��������� ����������� ���� ��� ������������ ������� ���������
    [ContextMenu("Test Full Heal")]
    private void TestFullHeal()
    {
        FullHeal();
    }
}