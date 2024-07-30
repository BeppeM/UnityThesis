using System;
using UnityEngine;
using WebSocketSharp;

public class UnityWebSocketChannel : MonoBehaviour
{
    private WebSocket ws;
    public GameObject avatar;
    private WebSocketChannel webSocketChannel;

    void Start()
    {
        WSConnectionInfoModel wSConnectionInfoModel = new WSConnectionInfoModel("ws://localhost:8887", "AGENT");
        webSocketChannel = new WebSocketChannel(wSConnectionInfoModel, OnMessage);        
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        string data = e.Data;
        Debug.Log("Received message: " + data);
        if (data == "reachDest")
        {
            Debug.Log("I'm in");
            // Dispatch the move action to the main thread
            UnityMainThreadDispatcher.Instance()
            .Enqueue(() => avatar.GetComponent<ReachDestination>().reachDestination());
        }
    }
}
