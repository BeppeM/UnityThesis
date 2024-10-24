using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutonomousWalking : MonoBehaviour
{

    private NavMeshAgent agent;
    private Vector3 min, max;
    private Vector3 targetPosition = Vector3.zero;  // Store the target position for Gizmos

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        WalkAround walkAround = GameObject.FindObjectOfType<WalkAround>();
        min = walkAround.min.position;
        max = walkAround.max.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Walk()
    {
        if (!agent.hasPath || agent.velocity.magnitude < 0.1f)
        {
            // Random point given max and min point representing the map size
            Vector3 randomPosition = new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z)
                );

            // Check if the random position is on the NavMesh
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPosition, out hit, 2.0f, NavMesh.AllAreas);
            agent.SetDestination(hit.position);
            targetPosition = hit.position;
        }
    }

    // Draw the direction the agent is going using Gizmos
    private void OnDrawGizmos()
    {
        if (agent != null && targetPosition != Vector3.zero)
        {
            // Set the color of the Gizmos
            Gizmos.color = Color.green;

            // Draw a line from the agent's current position to its destination
            Gizmos.DrawLine(agent.transform.position, targetPosition);

            // Draw a small sphere at the destination point
            Gizmos.DrawSphere(targetPosition, 0.2f);
        }
    }
}
