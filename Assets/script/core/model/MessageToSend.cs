
using System;

[Serializable]
public class MessageToSend
{

    public string environmentArtifactName;
    public string actionToPerform;
    public string agentInvolved;
    public string param;


    public MessageToSend(string environmentArtifactName, string actionToPerform, string agentInvolved, string param){
        this.environmentArtifactName = environmentArtifactName;
        this.actionToPerform = actionToPerform;
        this.agentInvolved = agentInvolved;
        this.param = param;
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