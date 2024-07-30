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
        // Connect to the WebSocket server
        ws.Connect();
    }


    private void OnOpen(object sender, System.EventArgs e)
    {
        Debug.Log("WebSocket connection opened for connection type: " + connectionInfo.getConnectionType());
        ws.Send("Connection established for URL: " + connectionInfo.getUrl());
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