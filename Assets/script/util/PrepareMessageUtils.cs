using System;
using System.Diagnostics;

class PrepareMessageUtil{

    public static WsMessage prepareMessage(string environmentArtifactName, string actionToPerform, string agentInvolved){
        
        return new WsMessage(environmentArtifactName, actionToPerform, agentInvolved);
    }

}