using UnityEngine;

public abstract class AbstractAvatar : MASAbstract
{

    public InitialShopperAgentBeliefs initialShopperAgentBeliefs;
    public GameObject[] focusedArtifacts;
    protected string agentFile;

    public GameObject[] FocusedArtifacts
    {
        get { return focusedArtifacts; }
        set { focusedArtifacts = value; }
    }

    public string AgentFile{
        get{return agentFile;}
    }

}