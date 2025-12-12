using UnityEngine;

public class Health : MonoBehaviour
{
    public static event System.Action OnNexusDied;

    [Header("Configuration des Tags")]
    public static readonly string[] EnemyTags = { "EnemyAir", "EnemyGround" };

    [Header("Stats")]
    [SerializeField] public float maxHealth = 100f;
    public float currentHealth;

    public event System.Action OnDie;

    void Awake()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        OnDie?.Invoke();

        if (WaveManager.instance != null)
        {
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
                OnNexusDied?.Invoke();
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

