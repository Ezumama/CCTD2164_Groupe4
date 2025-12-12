using UnityEngine;
using System.Collections.Generic;

public class Nexus : MonoBehaviour
{
    [Header("Dégâts infligés aux ennemis")]
    [Tooltip("Dégâts continus que le Nexus inflige aux ennemis dans son rayon.")]
    public float damageToEnemiesPerSecond = 5f;

    private Health nexusHealth;

    private List<EnemyNexusDamage> attackersInRange = new List<EnemyNexusDamage>();

    private List<Health> enemiesToHit = new List<Health>();

    void Start()
    {
        nexusHealth = GetComponent<Health>();
    }

    void Update()
    {
        attackersInRange.RemoveAll(a => a == null);
        enemiesToHit.RemoveAll(e => e == null);

        foreach (var enemy in enemiesToHit)
        {
            if (enemy != null)
            {
                enemy.TakeDamage(damageToEnemiesPerSecond * Time.deltaTime);
            }
        }

        if (nexusHealth.IsAlive())
        {
            foreach (var attacker in attackersInRange)
            {
                if (attacker != null)
                {
                    attacker.TryDamageNexus(nexusHealth);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsTargetedEnemy(other.gameObject))
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null && enemyHealth != nexusHealth && !enemiesToHit.Contains(enemyHealth))
            {
                enemiesToHit.Add(enemyHealth);
            }
        }

        EnemyNexusDamage enemyAttacker = other.GetComponent<EnemyNexusDamage>();
        if (enemyAttacker != null && !attackersInRange.Contains(enemyAttacker))
        {
            attackersInRange.Add(enemyAttacker);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Health enemyHealth = other.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemiesToHit.Remove(enemyHealth);
        }

        EnemyNexusDamage enemyAttacker = other.GetComponent<EnemyNexusDamage>();
        if (enemyAttacker != null)
        {
            attackersInRange.Remove(enemyAttacker);
        }
    }

    private bool IsTargetedEnemy(GameObject obj)
    {
        foreach (string tag in Health.EnemyTags)
        {
            if (obj.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }
}

