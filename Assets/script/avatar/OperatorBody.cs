using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OperatorBody : AbstractAvatarBody
{    

    protected override void Awake()
    {
        base.Awake();
        root = gameObject;
        mainAvatarScript = root.GetComponent<AbstractAvatar>();
    }


    protected override void OnTriggerEnter(Collider other)
    {
        // reached_destination(destName)
        if (other.gameObject.tag == "Artifact" && mainAvatarScript.ArtifactToReach == other.name)
        {
            print("Agent " + root.name + " reached the destination " + other.name);
            mainAvatarScript.SendMessageToJaCaMoBrain(UnityJacamoIntegrationUtil.createAndConvertJacamoMessageIntoJsonString("destinationReached", null,
                "reached_destination", null, other.name.FirstCharacterToLower()));
            mainAvatarScript.ArtifactToReach = "";
        }
    }
}
