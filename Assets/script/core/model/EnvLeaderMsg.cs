using System;

public class EnvLeaderMsg
{
    public string Action { get; set; }
    public string ActionType { get; set; }
    public string ResourceType { get; set; }
    public string AgentName { get; set; }

    // No-args constructor
    public EnvLeaderMsg() { }

    // All-args constructor
    public EnvLeaderMsg(string action, string actionType, string resourceType, string agentName)
    {
        Action = action;
        ActionType = actionType;
        ResourceType = resourceType;
        AgentName = agentName;
    }

    // ToString method override
    public override string ToString()
    {
        return $"EnvLeaderMsg [Action={Action}, ActionType={ActionType}, ResourceType={ResourceType}, AgentName={AgentName}]";
    }
}
