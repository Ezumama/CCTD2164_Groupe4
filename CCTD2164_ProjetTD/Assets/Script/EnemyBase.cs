using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class EnemyBase : MonoBehaviour
{
    protected Health health;

    protected virtual void Start()
    {
        health = GetComponent<Health>();

        health.OnDie += OnDeath;

        if (WaveManager.instance != null)
            WaveManager.instance.RegisterEnemy();
    }

    protected virtual void ReachDestination()
    {
        UnregisterAndDestroy();
    }

    private void OnDeath()
    {
        health.OnDie -= OnDeath;
        UnregisterAndDestroy();
    }

    protected void UnregisterAndDestroy()
    {
        if (WaveManager.instance != null)
            WaveManager.instance.UnregisterEnemy();

        if (health.IsAlive() && gameObject != null) 
            return;
    }
}
