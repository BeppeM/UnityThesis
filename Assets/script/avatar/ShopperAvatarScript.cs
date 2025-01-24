using Newtonsoft.Json;
using System;
using WebSocketSharp;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ShopperAvatarScript : AbstractAvatarWithEyes
{
    public ShopperBeliefs shopperBeliefs;
    private void Awake()
    {
        agentFile = "shopper.asl";
        JaCaMoAgentClassPath = "artifact.lib.maselements.AgentMasElement";
        agent = GetComponent<NavMeshAgent>();

        if (Application.IsPlaying(gameObject))
        {
            initializeWebSocketConnection(OnMessage);
            initializeAvatarWithEyes();
        }
    }

    // Unity avatar receives message from jacamo agent
    private void OnMessage(object sender, MessageEventArgs e)
    {
        string data = e.Data;
        print("Received message: " + data);
        WsMessage message = null;
        try
        {
            message = JsonConvert.DeserializeObject<WsMessage>(data);
            switch (message.MessageType)
            {
                case "wsInitialization":
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        print("Connection established for " + objInUse.name);
                    });
                    break;
                case "reachDestination":
                    print("Agent needs to reach destination.");
                    // Avatar receives the type of artifact to reach
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        SetBaloonText("New destination: " + message.MessagePayload);
                        reachDestination(message.MessagePayload);                        
                    });
                    break;
                default:
                    print("Unknown message type for " + objInUse.name);
                    break;
            }
        }
        catch (Exception ex)
        {
            print("Error: " + ex.Message);
            print("Message could not be converted.");
            return;
        }
    }

    public override AgentBeliefs AgentBeliefs
    {
        get { return shopperBeliefs; }
    }

}
