using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class Counter : MonoBehaviour
{


    private WebSocketChannel webSocketChannel;
    public GameObject envObj;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize new web socket connection
        WSConnectionInfoModel wSConnectionInfoModel = new WSConnectionInfoModel("ws://localhost:8885", "OBJECT", envObj.name);
        webSocketChannel = new WebSocketChannel(wSConnectionInfoModel, OnMessage);
    }

    // Update is called once per frame
    void Update() { }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Print the name of the object that entered the trigger
            Debug.Log("Trigger detected with " + other.gameObject.name);
            // Send message to JaCaMo
            sendMessageToJaCaMo("increment", other.gameObject.name);
        }
    }

    private void sendMessageToJaCaMo(string actionToPerform, string agentName)
    {
        WsMessage wsMessage = PrepareMessageUtil.prepareMessage(envObj.name, actionToPerform, agentName);
        string jsonString = JsonUtility.ToJson(wsMessage);
        Debug.Log(wsMessage.getActionToPerform());
        Debug.Log(jsonString);
        webSocketChannel.sendMessage(jsonString);
    }

    private void OnMessage(object sender, MessageEventArgs e) { }
}
