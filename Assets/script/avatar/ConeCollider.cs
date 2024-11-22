using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ConeCollider : MonoBehaviour
{
   
    private NavMeshAgent agent;
    bool reachedArtifact = false;
    GameObject root;
    public bool ReachedArtifact
    {
        get { return reachedArtifact; }
        set { reachedArtifact = value; }
    }

    private void Awake()
    {
        // Retrieve the root gameobject that represent the avatar
        root = transform.parent.transform.parent.gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.layer == LayerMask.NameToLayer("artifact"))
        {
            Debug.Log("Agent " + root.name + " has seen the artifact " + other.name);
            root.GetComponent<AvatarAI>().sendMessage(UnityJacamoIntegrationUtil.createAndConvertJacamoMessageIntoJsonString("eyes", null,
                "artifactSeen", null, other.name.FirstCharacterToLower()));
        }
        else if (obj.layer == LayerMask.NameToLayer("agent"))
        {            
            Debug.Log("Agent " + root.name + " has met another avatar: " + obj.transform.parent.name);
            root.GetComponent<AvatarAI>().sendMessage(UnityJacamoIntegrationUtil.createAndConvertJacamoMessageIntoJsonString("eyes", null,
                "agentSeen", null, obj.transform.parent.name));
        }
    }

}
