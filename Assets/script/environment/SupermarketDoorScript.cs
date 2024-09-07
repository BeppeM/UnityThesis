using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;

public class SupermarketDoorScript : MASAbstract
{

    public bool isSuperMarketOpen;
    private int flag = -1;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize new web socket connection
        initializeWebSocketConnection(OnMessage);

        if (isSuperMarketOpen)
        {
            // Change door color to blue  
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            // Stay closed
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
            sendMessageToJaCaMo("supermarket_open");
            flag = 0;
        }
        // supermarket is closed and message has not been sent yet to JACaMo
        if (!isSuperMarketOpen && flag != 1)
        {
            // Stay closed
            GetComponent<Renderer>().material.color = Color.red;
            sendMessageToJaCaMo("supermarket_closed");
            flag = 1;
        }
    }

    private void sendMessageToJaCaMo(string actionToPerform)
    {
        WsMessage wsMessage = UnityJacamoIntegrationUtil.prepareMessage(objInUse.name, actionToPerform, "all");
        string jsonString = JsonUtility.ToJson(wsMessage);
        Debug.Log(wsMessage.getActionToPerform());
        Debug.Log(jsonString);
        wsChannel.sendMessage(jsonString);
    }

    private void OnMessage(object sender, MessageEventArgs e) { }
}
