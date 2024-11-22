using System;
using WebSocketSharp;
using UnityEngine;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;

public class WebSocketChannel
{

    private WebSocket ws;
    private WSConnectionInfoModel connectionInfo;
    public WSConnectionInfoModel ConnectionInfoModel { get { return connectionInfo; } }
    private bool isWebSocketConnected = false;

    public WebSocketChannel(WSConnectionInfoModel connectionInfo, System.EventHandler<WebSocketSharp.MessageEventArgs> onMessage)
    {
        ws = new WebSocket(connectionInfo.getUrl());
        ws.OnOpen += OnOpen;
        ws.OnMessage += onMessage;
        ws.OnClose += OnClose;
        ws.OnError += OnError;
        this.connectionInfo = connectionInfo;
    }

    public bool IsWebSocketConnected
    {
        get
        {
            // Return the value of the private field
            return isWebSocketConnected;
        }
        set
        {
            // Set the value of the private field
            isWebSocketConnected = value;
        }
    }

    public void connect()
    {
        // Connect to the WebSocket server
        ws.Connect();
    }

    public async void sendMessage(string message)
    {
        Debug.Log("Sending message " + message);
        while (!IsWebSocketConnected)
        {
            await Task.Delay(2000);
        }
        ws.Send(message);
    }

    private void OnOpen(object sender, System.EventArgs e)
    {
        Debug.Log("WebSocket connection opened successfully for: " + connectionInfo.getName());
        IsWebSocketConnected = true;
    }

    private async void OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket connection closed with reason: " + e + " and code: " + e.Code);
        // Check if connection is closed
        if (e.Code == 1006)
        {
            await Task.Delay(5000);            
            connect();
        }
    }

    private void OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError("WebSocket error: " + e.Message);
    }

}