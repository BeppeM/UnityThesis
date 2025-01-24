using System.Collections.Generic;
using TMPro;
using UnityEngine;


public abstract class AbstractAvatar : AbstractMasElement
{

    public AgentBeliefs initialShopperAgentBeliefs;
    public GameObject[] focusedArtifacts;
    protected string agentFile;
    public List<GoalEnum> goals;
    protected TextMeshPro nameTextMeshPro;

    public GameObject[] FocusedArtifacts
    {
        get { return focusedArtifacts; }
        set { focusedArtifacts = value; }
    }

    public AgentBeliefs InitialShopperAgentBeliefs
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