using System;
using System.Collections.Generic;
using WebSocketSharp;

public class FruitSellerScript : AbstractArtifact
{

    public List<FruitInfo> fruitSellerProperties;

    // Fruits that the fruit seller has 
    public List<FruitInfo> FruitSellerProperties
    {
        get
        {
            return fruitSellerProperties;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Type = AgentArtifactTypeEnum.FruitSeller;
        if(fruitSellerProperties != null){
            artifactProperties = EscapeJson(convertObjectIntoJson(fruitSellerProperties));
        }        
        // Initialize new web socket connection
        initializeWebSocketConnection(OnMessage);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMessage(object sender, MessageEventArgs e) { }
}