using System;
using WebSocketSharp;
using UnityEngine;

public class WebSocketChannel
{

    private WebSocket ws;
    private WSConnectionInfoModel connectionInfo;

    public WebSocketChannel(WSConnectionInfoModel connectionInfo, System.EventHandler<WebSocketSharp.MessageEventArgs> onMessage)
    {
        ws = new WebSocket(connectionInfo.getUrl());
        ws.OnOpen += OnOpen;
        ws.OnMessage += onMessage;
        ws.OnClose += OnClose;
        ws.OnError += OnError;
        this.connectionInfo = connectionInfo;        
    }

    public void connect(){        
        // Connect to the WebSocket server
        ws.Connect();
    }

    public void sendMessage(string message){
        ws.Send(message);
    }

    private void OnOpen(object sender, System.EventArgs e)
    {
        Debug.Log("WebSocket connection opened for: " + connectionInfo.getName() + " of type: " 
        + connectionInfo.getConnectionType());
    }

    private void OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket connection closed with reason: " + e.Reason);
    }

    private void OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError("WebSocket error: " + e.Message);
    }

}