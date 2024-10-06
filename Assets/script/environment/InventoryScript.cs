using WebSocketSharp;

public class InventoryScript : AbstractArtifact
{

    private void Awake()
    {
        Type = ArtifactTypeEnum.Inventory;
        //if (barProperties != null)
        //{
        //    artifactProperties = EscapeJson(convertObjectIntoJson(barProperties));
        //}
        // Initialize new web socket connection
        initializeWebSocketConnection(OnMessage);
    }

    private void OnMessage(object sender, MessageEventArgs e) { }
}
