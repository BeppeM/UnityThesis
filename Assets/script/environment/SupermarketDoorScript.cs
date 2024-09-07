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

    // When Player enters into supermarket
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Print the name of the object that entered the trigger
            print("Trigger detected with " + other.gameObject.name);
            // Corrected line - ensure the 'gameObject' property is accessed correctly
            other.gameObject.GetComponent<AvatarScript>().CheckDestinationReached();
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
