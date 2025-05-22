// PlayerHealth.cs
using UnityEngine;
using UnityEngine.Events; // Para eventos si los necesitas (ej. OnPlayerDied)

public class PlayerHealth : MonoBehaviour
{
    [Header("Salud")]
    public int currentHealth = 3;
    public int maxHealth = 3;

    [Header("Eventos (Opcional)")]
    public UnityEvent OnPlayerDamaged;
    public UnityEvent OnPlayerHealed;
    public UnityEvent OnPlayerMaxHealthIncreased;
    public UnityEvent OnPlayerDied;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI(); // Llama a un método para actualizar la UI si tienes una
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return; // Ya está muerto

        currentHealth -= damageAmount;
        Debug.Log($"Jugador recibió {damageAmount} de daño. Salud actual: {currentHealth}");
        OnPlayerDamaged?.Invoke();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        UpdateHealthUI();
    }

    public void Heal(int healAmount)
    {
        if (currentHealth <= 0) return; // No curar si está muerto

        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log($"Jugador se curó {healAmount}. Salud actual: {currentHealth}");
        OnPlayerHealed?.Invoke();
        UpdateHealthUI();
    }

    public void IncreaseMaxHealth(int amount, bool healToNewMax = true)
    {
        maxHealth += amount;
        if (healToNewMax)
        {
            currentHealth = maxHealth; // Opcionalmente curar al nuevo máximo
        }
        else if (currentHealth > maxHealth)
        { // En caso de que de alguna manera la salud actual sea mayor
            currentHealth = maxHealth;
        }

        Debug.Log($"Salud máxima del jugador aumentada a {maxHealth}. Salud actual: {currentHealth}");
        OnPlayerMaxHealthIncreased?.Invoke();
        UpdateHealthUI();
    }

    void Die()
    {
        Debug.Log("¡Jugador Muerto!");
        OnPlayerDied?.Invoke();
        // Aquí puedes añadir lógica de Game Over, reiniciar escena, etc.
        // gameObject.SetActive(false); // Ejemplo: desactivar al jugador
    }

    // Este método será llamado por el área de daño del murciélago
    public void GetHitByEnemy(int damageFromEnemy)
    {
        TakeDamage(damageFromEnemy);
    }

    void UpdateHealthUI()
    {
        // Aquí actualiza tu UI de corazones/vida
        // Ejemplo: UIManager.Instance.UpdateHealthDisplay(currentHealth, maxHealth);
    }
}