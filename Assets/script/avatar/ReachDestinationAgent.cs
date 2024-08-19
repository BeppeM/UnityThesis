using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReachDestination : MonoBehaviour
{

    public GameObject destination;
    public GameObject destWhite;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame

    public void reachDestination(string dest){
        agent.isStopped = false;
        if(dest == "blue")
            agent.SetDestination(destination.transform.position);
        else
            agent.SetDestination(destWhite.transform.position);
    }

    public void stopWalking(){
        agent.isStopped = true;
    }
}
