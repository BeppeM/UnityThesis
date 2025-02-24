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
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    public override AgentBeliefs AgentBeliefs
    {
        get { return shopperBeliefs; }
    }
}
