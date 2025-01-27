using Newtonsoft.Json;
using System;
using WebSocketSharp;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ShopperAvatarScript : AbstractAvatarSocial
{
    public ShopperBeliefs shopperBeliefs;
    protected override void Awake()
    {
        base.Awake();   
        agentFile = "shopper.asl";
        JaCaMoAgentClassPath = "artifact.lib.maselements.AgentMasElement";
    }

    public override AgentBeliefs AgentBeliefs
    {
        get { return shopperBeliefs; }
    }

}
