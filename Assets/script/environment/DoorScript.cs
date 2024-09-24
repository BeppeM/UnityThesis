using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;

public class DoorScript : AbstractArtifact
{

    // Flag changed by the user
    public bool isSuperMarketOpen;
    private int flag = -1;

    void Awake()
    {
        Type = AgentArtifactTypeEnum.Door;
        // Initialize new web socket connection
        initializeWebSocketConnection(OnMessage);
        // Initialize property
        artifactProperties = isSuperMarketOpen.ToString();
        
        if (isSuperMarketOpen)
        {
            // Change door color to blue  
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            // Stay closed - color red
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if(!wsChannel.IsWebSocketConnected){
            return;
        }
        // supermarket is open and message has not been sent yet to JACaMo
        if (isSuperMarketOpen && flag != 0)
        {
            // Change door color to blue  
            GetComponent<Renderer>().material.color = Color.blue;
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(objInUse.name, "true", "all", null);
            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
            flag = 0;
        }
        // supermarket is closed and message has not been sent yet to JACaMo
        if (!isSuperMarketOpen && flag != 1)
        {
            // Stay closed
            GetComponent<Renderer>().material.color = Color.red;
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(objInUse.name, "false", "all", null);
            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);            
            flag = 1;
        }
    }

    private void OnMessage(object sender, MessageEventArgs e) { }
}