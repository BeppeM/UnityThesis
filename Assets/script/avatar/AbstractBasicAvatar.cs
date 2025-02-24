using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbstractBasicAvatar : AbstractAvatar
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        // reached_destination(destName)
        if (other.gameObject.tag == "Artifact" && artifacToReach == other.name)
        {
            print("Agent " + objInUse.name + " reached the destination " + other.name);
            wsChannel.sendMessage(UnityJacamoIntegrationUtil.createAndConvertJacamoMessageIntoJsonString("destinationReached", null,
                "reached_destination", null, other.name.FirstCharacterToLower()));
            artifacToReach = "";
        }
    }
}
