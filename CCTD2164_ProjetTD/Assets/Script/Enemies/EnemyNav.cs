using UnityEngine;
using UnityEngine.AI;
using System.Linq;


[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNav : EnemyBase 
{
    private NavMeshAgent agent;

    private Transform[] currentPath;
    private int currentIndex = 0;
    private bool pathAssigned = false;

    protected override void Start() 
    {
        base.Start(); 

        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("[EnemyNav] NavMeshAgent manquant !");
            enabled = false;
            return;
        }

        if (pathAssigned && currentPath != null && currentPath.Length > 0)
        {
            agent.SetDestination(currentPath[0].position);
        }
    }

    void Update()
    {
        if (!agent || currentPath == null || currentPath.Length == 0) return;
        if (agent.pathPending) return;

        if (currentIndex < currentPath.Length && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentIndex++; 

            if (currentIndex < currentPath.Length)
            {
                agent.SetDestination(currentPath[currentIndex].position);
            }
            else
            {
                ReachDestination();
            }
        }
    }

    public void SetPath(Transform[] points)
    {
        currentPath = points;
        pathAssigned = true;

        if (agent != null && currentPath.Length > 0)
        {
            currentIndex = 0;
            agent.SetDestination(currentPath[0].position);
        }
    }

    protected override void ReachDestination()
    {
        base.ReachDestination(); 
    }
}




