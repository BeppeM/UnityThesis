using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class walk : MonoBehaviour
{

    private NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.hasPath)
        {
            WalkAround walkAround = GameObject.FindObjectOfType<WalkAround>();
            Vector3 min = walkAround.min.position;
            Vector3 max = walkAround.max.position;

            Vector3 randomPosition = new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z)
                );
            agent.SetDestination(randomPosition);
        }
    }
}
