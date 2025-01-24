using Newtonsoft.Json;
using System;
using WebSocketSharp;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ShopperAvatarScript : AbstractAutonomousAvatarScript
{    

    private void Awake()
    {
        agentFile = "shopper.asl";
        JaCaMoAgentClassPath = "artifact.lib.maselements.AgentMasElement";
        // Retrieve avatar parts
        avatarBody = transform.Find("Body").gameObject;
        avatarEyes = transform.Find("anchorVisionCone").gameObject;
        agent = GetComponent<NavMeshAgent>();
        // Set waypoints to follow
        autonomousWalking = GetComponent<AutonomousWalking>();
        autonomousWalking.Waypoints = waypoints;

        if (Application.IsPlaying(gameObject))
        {
            initializeWebSocketConnection(OnMessage);
        }
        // Find the TextMeshPro component in the children of the avatar
        nameTextMeshPro  = transform.Find("avatarName").GetComponent<TextMeshPro>();
        nameTextMeshPro.text = name;

        // Initiating the baloon
        baloonText = gameObject.transform.Find("Canvas/BaloonBg/BaloonTxt").GetComponent<TextMeshProUGUI>();
        baloonText.text = "start";
    }

}
