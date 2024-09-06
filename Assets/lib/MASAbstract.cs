// Define an interface named IDamageable
using System;
using UnityEngine;

public abstract class MASAbstract : MonoBehaviour
{
    public GameObject objInUse;
    public string port;
    protected WebSocketChannel webSocketChannel;
    protected bool isConnected = false;

    protected void initializeWebSocketConnection(System.EventHandler<WebSocketSharp.MessageEventArgs> OnMessage)
    {
        print("Initializing connection for " + objInUse.name);
        // Initialize new web socket connection
        string url = "ws://localhost:" + port;
        WSConnectionInfoModel wSConnectionInfoModel = new WSConnectionInfoModel(url, "AGENT", objInUse.name);
        webSocketChannel = new WebSocketChannel(wSConnectionInfoModel, OnMessage);
    }

    // Method to connect to the websocket channel
    public void connectWs(){
        webSocketChannel.connect();
        print("Connected " + objInUse.name + " to port: " + port);
        isConnected = true;
    }
}