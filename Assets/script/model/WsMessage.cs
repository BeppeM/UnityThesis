
using System;

[Serializable]
public class WsMessage
{

    public String environmentArtifactName;
    public String actionToPerform;
    public String agentInvolved;


    public WsMessage(String environmentArtifactName, String actionToPerform, String agentInvolved){
        this.environmentArtifactName = environmentArtifactName;
        this.actionToPerform = actionToPerform;
        this.agentInvolved = agentInvolved;
    }

    public void setEnvironmentArtifactName(string environmentArtifactName)
    {
        this.environmentArtifactName = environmentArtifactName;
    }

    public void setActionToPerform(string actionToPerform)
    {
        this.actionToPerform = actionToPerform;
    }

    public void setAgentInvolved(string agentInvolved)
    {
        this.agentInvolved = agentInvolved;
    }

    public string getEnvironmentArtifactName()
    {
        return environmentArtifactName;
    }

    public string getActionToPerform()
    {
        return actionToPerform;
    }

    public string getAgentInvolved()
    {
        return agentInvolved;
    }

}