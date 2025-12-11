using UnityEngine;

public class Health : MonoBehaviour
{
    public static event System.Action OnNexusDied;

    [Header("Configuration des Tags")]
    public static readonly string[] EnemyTags = { "EnemyAir", "EnemyGround" };

    [Header("Stats")]
    [SerializeField] public float maxHealth = 100f;
    public float currentHealth;

    // Ajout d'un événement pour que d'autres scripts puissent réagir à la mort
    public event System.Action OnDie;

    void Awake()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return; // Évite les calculs si l'objet est déjà mort

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Changé en public virtual pour que les enfants (Nexus, Enemy) puissent l'étendre
    public virtual void Die()
    {
        OnDie?.Invoke();

        // 2. Vérification et Désenregistrement de l'ennemi (Logique inchangée)
        if (WaveManager.instance != null)
        {
            // Vérifie si l'objet qui meurt a un des tags d'ennemi définis
            bool isEnemy = false;
            foreach (string tag in EnemyTags)
            {
                if (gameObject.CompareTag(tag))
                {
                    isEnemy = true;
                    break;
                }
            }

            if (isEnemy)
            {
                WaveManager.instance.UnregisterEnemy();
            }
            else if (gameObject.CompareTag("Nexus")) 
            {
                OnNexusDied?.Invoke(); // Déclencher l'événement de Game Over
            }
        }

        Destroy(gameObject);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    public bool IsAlive() => currentHealth > 0;
}

