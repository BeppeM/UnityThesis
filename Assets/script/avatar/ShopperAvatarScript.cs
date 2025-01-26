using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;
using TMPro;

public class ShopperAvatarScript : AbstractAvatar
{
    public ShopperBeliefs shopperBeliefs;

    protected override void Awake()
    {
        base.Awake();
        agentFile = "shopper.asl";
        JaCaMoAgentClassPath = "artifact.lib.maselements.AgentMasElement";
    }

    // When Player enters into supermarket
    void OnTriggerEnter(Collider other)
    {
        // reached_destination(destName)
        if (!other.gameObject.name.Contains("counter") && other.gameObject.tag == "Artifact")
        {                     
            wsChannel.sendMessage(UnityJacamoIntegrationUtil.createAndConvertJacamoMessageIntoJsonString("destinationReached", null, 
                "reached_destination", null, other.name.FirstCharacterToLower()));          
        }
        if (other.gameObject.name.Contains("exitDoor"))
        {
            Destroy(this);
        }
    }

    public override AgentBeliefs AgentBeliefs
    {
        get { return shopperBeliefs; }
    }
}
