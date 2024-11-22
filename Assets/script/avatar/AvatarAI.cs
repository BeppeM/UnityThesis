using Newtonsoft.Json;
using System;
using WebSocketSharp;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class AvatarAI : AbstractAvatar
{
    NavMeshAgent agent;
    private GameObject avatarBody;
    private GameObject avatarEyes;

    private void Awake()
    {
        agentFile = "shopper.asl";
        // Retrieve avatar parts
        avatarBody = transform.Find("Body").gameObject;
        avatarEyes = transform.Find("anchorVisionCone").gameObject;
        agent = GetComponent<NavMeshAgent>();

        if (Application.IsPlaying(gameObject))
        {
            initializeWebSocketConnection(OnMessage);
        }
        // Find the TextMeshPro component in the children of the avatar
        nameTextMeshPro  = transform.Find("avatarName").GetComponent<TextMeshPro>();

        // Check if we found the TextMeshPro component
        if (nameTextMeshPro != null)
        {
            // Set the text of the TextMeshPro to the avatar's name
            nameTextMeshPro.text = name;
        }
        else
        {
            Debug.LogWarning("TextMeshPro component not found in the avatar's children.");
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

    private void reachDestination(string dest)
    {
        agent.isStopped = false;
        agent.SetDestination(GameObject.Find(dest).transform.position);
    }

    protected void evaluateAndSendMessage()
    {
        print("Evaluate and send message");

    }

    public void sendMessage(string message)
    {
        wsChannel.sendMessage(message);
    }



}
