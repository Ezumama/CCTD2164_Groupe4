using UnityEngine;
using System.Linq;

public class EnemyAir : EnemyBase
{
    [SerializeField] private float speed = 5f;
    private Transform nexusTarget; 

    protected override void Start()
    {
        base.Start();

        GameObject wpParent = GameObject.Find("AirWaypoints");
        if (wpParent != null && wpParent.transform.childCount > 0)
        {
            nexusTarget = wpParent.transform.GetChild(0); 
        }

        if (nexusTarget == null)
        {
            Debug.LogError("AirWaypoint/Nexus Target non trouvé");
            enabled = false;
        }
    }

    void Update()
    {
        if (nexusTarget == null) return;

        Vector3 dir = (nexusTarget.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, nexusTarget.position) < 0.5f)
        {
            ReachDestination();
        }
    }

    protected override void ReachDestination()
    {
        base.ReachDestination();
    }
}
