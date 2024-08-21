using System;
using UnityEngine;
using WebSocketSharp;

public class OpenSuperMarket : MonoBehaviour
{

    public bool isSuperMarketOpen;
    private WebSocketChannel webSocketChannel;
    public GameObject envObj;
    private int flag = -1;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize new web socket connection
        WSConnectionInfoModel wSConnectionInfoModel = new WSConnectionInfoModel("ws://localhost:8888", "OBJECT", envObj.name);
        webSocketChannel = new WebSocketChannel(wSConnectionInfoModel, OnMessage);
        
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
        // supermarket is open and message has not been sent yet to JACaMo
        if (isSuperMarketOpen && flag != 0)
        {
            // Change door color to blue  
            GetComponent<Renderer>().material.color = Color.blue;
            // TODO: Signal agent that the supermarket is open
            webSocketChannel.sendMessage("supermarket_open");
            flag = 0;
        }
        // supermarket is closed and message has not been sent yet to JACaMo
        if (!isSuperMarketOpen && flag != 1)
        {
            // Stay closed
            GetComponent<Renderer>().material.color = Color.red;
            webSocketChannel.sendMessage("supermarket_closed");
            flag = 1;
        }
    }

    private void OnMessage(object sender, MessageEventArgs e) { }
}
