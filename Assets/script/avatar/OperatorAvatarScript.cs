using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.AI;

public class OperatorAvatarScript : AbstractBasicAvatar
{
    public OperatorBeliefs operatorBeliefs;

    protected override void Awake()
    {
        base.Awake();
        agentFile = "operator.asl";
        JaCaMoAgentClassPath = "artifact.lib.maselements.AgentMasElement";
    }

    public override AgentBeliefs AgentBeliefs
    {
        get { return operatorBeliefs; }
    }
}
