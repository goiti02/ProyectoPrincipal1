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
        UpdateHealthUI(); // Llama a un m�todo para actualizar la UI si tienes una
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return; // Ya est� muerto

        currentHealth -= damageAmount;
        Debug.Log($"Jugador recibi� {damageAmount} de da�o. Salud actual: {currentHealth}");
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
        if (currentHealth <= 0) return; // No curar si est� muerto

        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log($"Jugador se cur� {healAmount}. Salud actual: {currentHealth}");
        OnPlayerHealed?.Invoke();
        UpdateHealthUI();
    }

    public void IncreaseMaxHealth(int amount, bool healToNewMax = true)
    {
        maxHealth += amount;
        if (healToNewMax)
        {
            currentHealth = maxHealth; // Opcionalmente curar al nuevo m�ximo
        }
        else if (currentHealth > maxHealth)
        { // En caso de que de alguna manera la salud actual sea mayor
            currentHealth = maxHealth;
        }

        Debug.Log($"Salud m�xima del jugador aumentada a {maxHealth}. Salud actual: {currentHealth}");
        OnPlayerMaxHealthIncreased?.Invoke();
        UpdateHealthUI();
    }

    void Die()
    {
        Debug.Log("�Jugador Muerto!");
        OnPlayerDied?.Invoke();
        // Aqu� puedes a�adir l�gica de Game Over, reiniciar escena, etc.
        // gameObject.SetActive(false); // Ejemplo: desactivar al jugador
    }

    // Este m�todo ser� llamado por el �rea de da�o del murci�lago
    public void GetHitByEnemy(int damageFromEnemy)
    {
        TakeDamage(damageFromEnemy);
    }

    void UpdateHealthUI()
    {
        // Aqu� actualiza tu UI de corazones/vida
        // Ejemplo: UIManager.Instance.UpdateHealthDisplay(currentHealth, maxHealth);
    }
}