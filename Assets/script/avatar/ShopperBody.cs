using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopperBody : AbstractAvatarBody
{
    AbstractAvatarWithEyesAndVoice abstractAvatarWithEyesAndVoice;
    protected override void Awake()
    {
        base.Awake();
        root = transform.parent.gameObject;
        mainAvatarScript = root.GetComponent<AbstractAvatar>();
        abstractAvatarWithEyesAndVoice = (AbstractAvatarWithEyesAndVoice) mainAvatarScript;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        print("Agent " + root.name + " reached destination " + other.name.FirstCharacterToLower());
        // reached_destination(destName)
        if (other.gameObject.tag == "Artifact" && other.name == mainAvatarScript.ArtifactToReach)
        {
            print("Agent " + root.name + " reached destination " + other.name.FirstCharacterToLower());
            abstractAvatarWithEyesAndVoice.SetBaloonText("Reached destination: " + other.name.FirstCharacterToLower());
            mainAvatarScript.SendMessageToJaCaMoBrain(UnityJacamoIntegrationUtil.createAndConvertJacamoMessageIntoJsonString("destinationReached", null,
                "reached_destination", null, other.name.FirstCharacterToLower()));
            mainAvatarScript.ArtifactToReach = "";
            artifactReached = other.name.FirstCharacterToLower();
            abstractAvatarWithEyesAndVoice.EnableDisableVisionCone(false);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (!artifactReached.Equals("") && other.name.FirstCharacterToLower().Equals(artifactReached))
        {
            abstractAvatarWithEyesAndVoice.EnableDisableVisionCone(true);
            artifactReached = "";
        }
    }

}
