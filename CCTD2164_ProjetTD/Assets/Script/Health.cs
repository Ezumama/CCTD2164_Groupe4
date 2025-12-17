using UnityEngine;

public class Health : MonoBehaviour
{
    public static event System.Action OnNexusDied;

    [Header("Configuration des Tags")]
    public static readonly string[] EnemyTags = { "EnemyAir", "EnemyGround" };

    [Header("Stats")]
    [SerializeField] public float maxHealth = 100f;
    public float currentHealth;

    [Header("VFX")]
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private GameObject DeathVFXSpawnPoint;
    public event System.Action OnDie;

    public int goldDrop;

    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDead || currentHealth <= 0) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        currentHealth = 0; 

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
                WaveManager.instance.UnregisterEnemy(gameObject.name);
                GameManager.Instance.GainMoney(goldDrop);
            }
            else if (gameObject.CompareTag("Nexus"))
            {
                OnNexusDied?.Invoke();
            }
        }

        if (deathVFX != null)
        {
            if (DeathVFXSpawnPoint != null)
            {
                Instantiate(deathVFX, DeathVFXSpawnPoint.transform.position, DeathVFXSpawnPoint.transform.rotation);
            }
            else
            {
                Instantiate(deathVFX, transform.position, transform.rotation);
            }
        }

        Destroy(gameObject);
    }

    public void Heal(float amount)
    {
        if (isDead) return; 
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    public bool IsAlive() => !isDead && currentHealth > 0;

    private void Update() {
        if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            if (Input.GetKeyDown(KeyCode.K)) {
                TakeDamage(1000f);
            }

        if (gameObject.CompareTag("Nexus"))
            if (Input.GetKeyDown(KeyCode.L)) {
                Heal(10f);
            }
    }
}