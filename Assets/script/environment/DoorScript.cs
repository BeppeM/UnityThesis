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
        Type = ArtifactTypeEnum.Door;
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
            wsChannel.sendMessage(UnityJacamoIntegrationUtil
                .createAndConvertJacamoMessageIntoJsonString(objInUse.name, "signal_shoppers", "", true));            
            flag = 0;
        }
        // supermarket is closed and message has not been sent yet to JACaMo
        if (!isSuperMarketOpen && flag != 1)
        {
            // Stay closed
            GetComponent<Renderer>().material.color = Color.red;
            wsChannel.sendMessage(UnityJacamoIntegrationUtil
                .createAndConvertJacamoMessageIntoJsonString(objInUse.name, "signal_shoppers", "", false));                    
            flag = 1;
        }
    }

    private void OnMessage(object sender, MessageEventArgs e) { }
}