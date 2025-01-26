using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


public abstract class AbstractAvatar : AbstractMasElement
{

    public AgentBeliefs agentBeliefs;
    public GameObject[] focusedArtifacts;
    protected string agentFile;
    public List<GoalEnum> goals;
    protected TextMeshPro nameTextMeshPro;
    protected string jaCaMoAgentClassPath;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

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

    public void stopWalking()
    {
        agent.isStopped = true;
    }

    protected void reachDestination(string dest)
    {
        agent.isStopped = false;
        agent.SetDestination(GameObject.Find(dest).transform.position);
    }
}