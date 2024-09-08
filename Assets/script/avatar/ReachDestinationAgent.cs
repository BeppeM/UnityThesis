using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReachDestination : MonoBehaviour
{
    NavMeshAgent agent;
    public float walkRadius = 10f; // Radius for random walking

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Start by moving to a random point on the NavMesh surface
        // MoveToRandomPoint();
    }

    void MoveToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1))
        {
            Vector3 finalPosition = hit.position;
            agent.SetDestination(finalPosition);
        }
    }
    // Update is called once per frame

    public void reachDestination(string dest)
    {
        agent.isStopped = false;
        agent.SetDestination(GameObject.Find(dest).transform.position);
    }

    public void stopWalking()
    {
        agent.isStopped = true;
    }
}
