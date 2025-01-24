using System.Collections.Generic;
using TMPro;
using UnityEngine;


public abstract class AbstractAvatar : AbstractMasElement
{

    public AgentBeliefs agentBeliefs;
    public GameObject[] focusedArtifacts;
    protected string agentFile;
    public List<GoalEnum> goals;
    protected TextMeshPro nameTextMeshPro;
    protected string jaCaMoAgentClassPath;

    public GameObject[] FocusedArtifacts
    {
        get { return focusedArtifacts; }
        set { focusedArtifacts = value; }
    }

    public virtual AgentBeliefs AgentBeliefs
    {
        get { return agentBeliefs; }
    }

    public string AgentFile
    {
        get { return agentFile; }
    }

    public List<GoalEnum> Goals
    {
        get { return goals; }

    }

    public string JaCaMoAgentClassPath
    {
        get { return jaCaMoAgentClassPath; }
        set { jaCaMoAgentClassPath = value; }
    }
}