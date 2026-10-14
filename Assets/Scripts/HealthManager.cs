using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [Header("UI References")]
    [SerializeField] private Slider healthSlider;

    private bool isGameOver = false;

    private void OnEnable()
    {
        GameEventManager.OnItemCaught += HandleItemCaught;
        GameEventManager.OnGoodItemMissed += HandleGoodItemMissed;
    }

    private void OnDisable()
    {
        GameEventManager.OnItemCaught -= HandleItemCaught;
        GameEventManager.OnGoodItemMissed -= HandleGoodItemMissed;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void HandleItemCaught(FallingObject item)
    {
        // Penalty if player catches a Bad item (Orca)
        if (item.itemType == FallingObject.ObjectType.Bad)
        {
            TakeDamage(1);
        }
    }

    private void HandleGoodItemMissed(FallingObject item)
    {
        // Penalty if a Good item (Seal) misses the gloves and hits the bottom
        if (item.itemType == FallingObject.ObjectType.Good)
        {
            TakeDamage(1);
        }
    }

    private void TakeDamage(int amount)
    {
        if (isGameOver) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        Debug.Log("Health lost! Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            TriggerGameOver();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER!");
        Time.timeScale = 0f; // Freezes the game loop
    }
}