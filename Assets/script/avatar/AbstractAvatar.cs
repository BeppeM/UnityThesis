using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public abstract class AbstractAvatar : MASAbstract
{

    public InitialAgentBeliefs initialAgentBeliefs;
    public GameObject[] focusedArtifacts;
    protected string agentFile;
    public List<GoalEnum> goals;
    protected TextMeshPro nameTextMeshPro;

    public GameObject[] FocusedArtifacts
    {
        get { return focusedArtifacts; }
        set { focusedArtifacts = value; }
    }

    public InitialAgentBeliefs InitialAgentBeliefs
    {
        get { return initialAgentBeliefs; }
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