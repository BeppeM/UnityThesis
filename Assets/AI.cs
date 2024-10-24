using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    NavMeshAgent agent;
    State currentState;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = new IdleState(gameObject, agent);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState = currentState.Process();
        }        
    }
}
