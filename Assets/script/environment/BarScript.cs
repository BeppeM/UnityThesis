using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;

public class BarScript : AbstractArtifact
{
    public List<CoffeeInfo> barProperties;

    // Fruits that the fruit seller has 
    public List<CoffeeInfo> BarProperties
    {
        get
        {
            return barProperties;
        }
    }

    private void Awake()
    {
        Type = ArtifactTypeEnum.Bar;
        if (barProperties != null)
        {
            artifactProperties = EscapeJson(convertObjectIntoJson(barProperties));
        }
        //Initialize new web socket connection
        initializeWebSocketConnection(OnMessage);
    }

    private void OnMessage(object sender, MessageEventArgs e) { }
}
