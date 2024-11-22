using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AvatarBody : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject root;    

    // Start is called before the first frame update
    void Awake()
    {
        root = transform.parent.gameObject;

        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reachDestination(string dest)
    {
        agent.isStopped = false;
        agent.SetDestination(GameObject.Find(dest).transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Agent " + root.name + " reached " + other.name);
        print("Agent " + root.name + " reached destination " + other.name.FirstCharacterToLower());
        // reached_destination(destName)
        if (!other.gameObject.name.Contains("counter") && (other.gameObject.tag == "Artifact"))
        {
            print("Agent " + root.name + " reached destination " + other.name.FirstCharacterToLower());
            root.GetComponent<AvatarAI>().sendMessage(UnityJacamoIntegrationUtil.createAndConvertJacamoMessageIntoJsonString("destinationReached", null,
                "reached_destination", null, other.name.FirstCharacterToLower()));
        }
        if (other.gameObject.name.Contains("exitDoor"))
        {
            Destroy(this);
        }
    }
}
