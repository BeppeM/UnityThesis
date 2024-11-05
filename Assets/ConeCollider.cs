using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConeCollider : MonoBehaviour
{

    private GameObject avatar;
    private NavMeshAgent agent;
    bool reachedArtifact = false;
    public bool ReachedArtifact
    {
        get { return reachedArtifact; }
        set { reachedArtifact = value; }
    }

    void Start()
    {
        // Retrieve the parent object -> the avatar which sensor belongs
        avatar = transform.parent.gameObject;
        agent = avatar.GetComponent<NavMeshAgent>();        
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        Debug.Log("Collision detected with: " + obj.name);
        if (obj.layer == LayerMask.NameToLayer("pippo"))
        {
            obj.GetComponent<Renderer>().material.color = Color.blue;
        }else if (obj.layer == LayerMask.NameToLayer("shops"))
        {
            Debug.Log("Found a shop: " + obj.name);
            reachedArtifact = true;
            // Stop the agent
            agent.isStopped = true;
        }
    }

}
