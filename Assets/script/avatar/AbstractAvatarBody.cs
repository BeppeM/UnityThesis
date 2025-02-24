using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractAvatarBody : MonoBehaviour
{
    protected GameObject root;
    protected AbstractAvatarWithEyesAndVoice mainAvatarScript;
    protected string artifactReached;

    // Start is called before the first frame update
    void Awake()
    {
        artifactReached = "";
        root = transform.parent.gameObject;
        mainAvatarScript = root.GetComponent<AbstractAvatarWithEyesAndVoice>();
    }

    protected void OnTriggerEnter(Collider other)
    {
        print("Agent " + root.name + " reached destination " + other.name.FirstCharacterToLower());
        // reached_destination(destName)
        if (other.gameObject.tag == "Artifact" && other.name == mainAvatarScript.ArtifactToReach)
        {
            print("Agent " + root.name + " reached destination " + other.name.FirstCharacterToLower());
            mainAvatarScript.SetBaloonText("Reached destination: " + other.name.FirstCharacterToLower());
            mainAvatarScript.SendMessageToJaCaMoBrain(UnityJacamoIntegrationUtil.createAndConvertJacamoMessageIntoJsonString("destinationReached", null,
                "reached_destination", null, other.name.FirstCharacterToLower()));
            mainAvatarScript.ArtifactToReach = "";
            artifactReached = other.name.FirstCharacterToLower();
            mainAvatarScript.EnableDisableVisionCone(false);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (!artifactReached.Equals("") && other.name.FirstCharacterToLower().Equals(artifactReached))
        {
            mainAvatarScript.EnableDisableVisionCone(true);
            artifactReached = "";
        }
    }

}