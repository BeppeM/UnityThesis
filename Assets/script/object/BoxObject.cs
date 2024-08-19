using System;
using UnityEngine;
using WebSocketSharp;

public class BoxObject : MonoBehaviour
{
    private WebSocketChannel webSocketChannel;

    void Start()
    {
        // Initialize new web socket connection
        WSConnectionInfoModel wSConnectionInfoModel = new WSConnectionInfoModel("ws://localhost:8888", "OBJECT", "");
        webSocketChannel = new WebSocketChannel(wSConnectionInfoModel, OnMessage);
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        // string data = e.Data;
        // Debug.Log("Received message: " + data);
        // if (data == "reachDest")
        // {
        //     Debug.Log("I'm in");
        //     // Dispatch the move action to the main thread
        //     UnityMainThreadDispatcher.Instance()
        //     .Enqueue(() => Debug.Log("Hola!"));
        // }
    }

    // This method is called when another collider enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.name == "Avatar")
        {
            Debug.Log("Avatar has entered the trigger zone!");
            // Example action: change the color of the trigger zone on enter
            GetComponent<Renderer>().material.color = Color.red;
            webSocketChannel.sendMessage("pedana_pressed");
        }
    }
}
