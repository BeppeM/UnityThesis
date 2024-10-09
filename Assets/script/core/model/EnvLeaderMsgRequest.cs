using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvLeaderMsgRequest
{
    public string Action { get; set; }
    public string ActionType { get; set; }
    public string ResourceType { get; set; }
    public string AgentName { get; set; }

    public EnvLeaderMsgRequest() { }

    public EnvLeaderMsgRequest(string action, string actionType, string resourceType, string agentName)
    {
        Action = action;
        ActionType = actionType;
        ResourceType = resourceType;
        AgentName = agentName;
    }

    public override string ToString()
    {
        return $"EnvLeaderMsgRequest [Action={Action}, ActionType={ActionType}, ResourceType={ResourceType}, AgentName={AgentName}]";
    }
}
