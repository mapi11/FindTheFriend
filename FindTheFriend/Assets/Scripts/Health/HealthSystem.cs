using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    [SerializeField] private int currentHealth;

    [Header("Heart References")]
    public AnimatedHeart[] hearts;

    private void Start()
    {
        currentHealth = maxHealth;
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

    public void TakeDamage(int amount = 1)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHearts();

        Debug.Log($"Damage taken! Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount = 1)
    {
        if (currentHealth >= maxHealth)
        {
            Debug.Log("Already at full health!");
            return;
        }

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHearts();

        Debug.Log($"Healed! Health: {currentHealth}/{maxHealth}");
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
        Debug.Log("Player died!");
        // Здесь можно добавить логику смерти:
        // Time.timeScale = 0; // Пауза игры
        // ShowGameOverScreen();
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
}