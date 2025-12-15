using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class EnemyBase : MonoBehaviour
{
    protected Health health;

    protected virtual void Start()
    {
        health = GetComponent<Health>();

    }

    protected virtual void ReachDestination()
    {
        if (health != null)
        {
            health.Die();
        }
    }
}
