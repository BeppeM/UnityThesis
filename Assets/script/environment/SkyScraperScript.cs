using WebSocketSharp;

public class SkyScraperScript : AbstractArtifact
{
    private void Awake()
    {
        Type = ArtifactTypeEnum.SkyScraper;
        //if (barProperties != null)
        //{
        //    artifactProperties = EscapeJson(convertObjectIntoJson(barProperties));
        //}
        // Initialize new web socket connection
        initializeWebSocketConnection(OnMessage);
    }

    private void OnMessage(object sender, MessageEventArgs e) { }
}
