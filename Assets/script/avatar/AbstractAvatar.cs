using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using WebSocketSharp;


public abstract class AbstractAvatar : AbstractMasElement
{

    public AgentBeliefs agentBeliefs;
    public GameObject[] focusedArtifacts;
    protected string agentFile;
    public List<GoalEnum> goals;
    protected TextMeshPro nameTextMeshPro;
    protected string jaCaMoAgentClassPath;
    NavMeshAgent agent;
    protected string artifacToReach;

    protected virtual void Awake()
    {
        artifacToReach = "";
        if (Application.IsPlaying(gameObject))
        {
            initializeWebSocketConnection(OnMessage);
        }
        // Find the TextMeshPro component in the children of the avatar
        nameTextMeshPro = GetComponentInChildren<TextMeshPro>();

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

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public GameObject[] FocusedArtifacts
    {
        get { return focusedArtifacts; }
        set { focusedArtifacts = value; }
    }

    public virtual AgentBeliefs AgentBeliefs
    {
        get { return agentBeliefs; }
    }

    public string AgentFile
    {
        get { return agentFile; }
    }

    public List<GoalEnum> Goals
    {
        get { return goals; }

    }

    public string JaCaMoAgentClassPath
    {
        get { return jaCaMoAgentClassPath; }
        set { jaCaMoAgentClassPath = value; }
    }

    public void stopWalking()
    {
        agent.isStopped = true;
    }

    protected void reachDestination(string dest)
    {
        agent.isStopped = false;
        agent.SetDestination(GameObject.Find(dest).transform.position);
    }

    // Unity avatar receives message from jacamo agent
    protected override void OnMessage(object sender, MessageEventArgs e)
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
                    print("Connection established for " + objInUse.name);
                    break;
                case "reachDestination":
                    print("Agent needs to reach destination.");
                    artifacToReach = message.MessagePayload;
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
        catch (Exception)
        {
            print("Message could not be converted.");
            return;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // reached_destination(destName)
        if (other.gameObject.tag == "Artifact" && artifacToReach == other.name)
        {
            print("Agent " +  objInUse.name + " reached the destination " + other.name);
            wsChannel.sendMessage(UnityJacamoIntegrationUtil.createAndConvertJacamoMessageIntoJsonString("destinationReached", null,
                "reached_destination", null, other.name.FirstCharacterToLower()));
            artifacToReach = "";
        }
    }
}