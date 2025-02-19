using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ConeCollider : MonoBehaviour
{
       
    GameObject root;
    AbstractAvatarWithEyesAndVoice mainAvatarScript;


    private void Awake()
    {
        // Retrieve the root gameobject that represent the avatar
        root = transform.parent.transform.parent.gameObject;
        // Retrieve the avatar baloon
        mainAvatarScript = root.GetComponent<AbstractAvatarWithEyesAndVoice>();

    }

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        if (obj.layer == LayerMask.NameToLayer("artifact"))
        {
            Debug.Log("Agent " + root.name + " has seen the artifact " + other.name);            
            mainAvatarScript.SetBaloonText("Artifact seen: " + other.name.FirstCharacterToLower());
            mainAvatarScript.SendMessageToJaCaMoBrain(UnityJacamoIntegrationUtil.createAndConvertJacamoMessageIntoJsonString("eyes", null,
                "artifactSeen", null, other.name.FirstCharacterToLower()));
        }
        else if (obj.layer == LayerMask.NameToLayer("agent"))
        {            
            Debug.Log("Agent " + root.name + " has met another avatar: " + obj.transform.parent.name);
            mainAvatarScript.SetBaloonText("Agent seen: " + obj.transform.parent.name);
            mainAvatarScript.SendMessageToJaCaMoBrain(UnityJacamoIntegrationUtil.createAndConvertJacamoMessageIntoJsonString("eyes", null,
                "agentSeen", null, obj.transform.parent.name));
        }
    }

}
