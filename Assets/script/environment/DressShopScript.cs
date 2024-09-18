using System;
using System.Collections.Generic;
using WebSocketSharp;

public class DressShopScript : AbstractArtifact
{

    public List<ClothesInfo> dressShopProperties;

    // Fruits that the fruit seller has 
    public List<ClothesInfo> DressShopProperties
    {
        get
        {
            return dressShopProperties;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        type = AgentArtifactTypeEnum.DressShop;
        if(dressShopProperties != null){
            artifactProperties = EscapeJson(convertObjectIntoJson(dressShopProperties));
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