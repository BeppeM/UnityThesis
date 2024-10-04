using System.Collections.Generic;
using UnityEngine;


public abstract class AbstractAvatar : MASAbstract
{

    public InitialShopperAgentBeliefs initialShopperAgentBeliefs;
    public GameObject[] focusedArtifacts;
    protected string agentFile;
    public List<GoalEnum> goals;

    public GameObject[] FocusedArtifacts
    {
        get { return focusedArtifacts; }
        set { focusedArtifacts = value; }
    }

    public InitialShopperAgentBeliefs InitialShopperAgentBeliefs
    {
        get { return initialShopperAgentBeliefs; }
    }

    public string AgentFile
    {
        get { return agentFile; }
    }

    public List<GoalEnum> Goals
    {
        get { return goals; }

    }
}