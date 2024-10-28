using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using WebSocketSharp;

[ExecuteAlways]
public class Artifact : MonoBehaviour
{
    public string port;
    public string Port
    {
        get { return port; }
    }
    protected GameObject objInUse;
    // List of all property names
    private List<string> propertyNames = new List<string>();
    protected WebSocketChannel wsChannel;
    
    private ArtifactTypeEnum artifactType;
    public ArtifactTypeEnum ArtifactType
    {
        get { return artifactType; }
    }

    // All artifact properties
    public List<CoffeeInfo> barProperties;
    public List<FruitInfo> fruitShopProperties;
    public List<ClothesInfo> dressShopProperties;
    public bool doorProperties;
    // Properties in JSON format to configure .jcm file
    private string artifactProperties;
    public string ArtifactProperties
    {
        get { return artifactProperties; }
        set { artifactProperties = value; }
    }

    public List<string> PropertyNames
    {
        get { return propertyNames; }
    }

    protected virtual void Awake()
    {
        // Retrieve all fields
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (field.Name != "port")
            {
                propertyNames.Add(field.Name);
            }
        }
        objInUse = gameObject;
        artifactType = gameObject.GetComponent<GenericArtifactType>().GetShopType();

        if (Application.IsPlaying(gameObject))
        {
            // Play logic            
            // Retrieve and evaluate artifact type            
            string artifactPropertyName = artifactType.ToString();
            artifactPropertyName = char.ToLower(artifactPropertyName[0]) + artifactPropertyName.Substring(1) + "Properties";

            // Find the field with the specified name
            FieldInfo filteredField = Array.Find(fields, f => f.Name == artifactPropertyName);
            if (filteredField != null)
            {
                // Map in JSON the artifact property        
                artifactProperties = EscapeJson(convertObjectIntoJson(filteredField.GetValue(this)));
                Debug.Log("Artifact property: " + artifactProperties.ToString());

            }
            initializeWebSocketConnection(OnMessage);
        }
        else
        {
            // Editor logic            
        }
    }

    protected void initializeWebSocketConnection(System.EventHandler<WebSocketSharp.MessageEventArgs> OnMessage)
    {
        print("Initializing connection for " + objInUse.name);
        // Initialize new web socket connection
        string url = "ws://localhost:" + port;
        WSConnectionInfoModel wSConnectionInfoModel = new WSConnectionInfoModel(url, "AGENT", objInUse.name);
        wsChannel = new WebSocketChannel(wSConnectionInfoModel, OnMessage);
    }

    // Method to connect to the websocket channel
    public async Task connectWs()
    {
        int maxRetryAttempt = 3;
        await Task.Run((Func<Task>)(async () =>
        {
            int currentAttempt = 1;
            while (!wsChannel.IsWebSocketConnected && currentAttempt < maxRetryAttempt)
            {
                print("Connecting attemp n°: " + currentAttempt);
                wsChannel.connect();
                if (wsChannel.IsWebSocketConnected)
                {
                    break;
                }
                currentAttempt++;
                // Wait 5 seconds before retrying
                await Task.Delay(5000);
            }
        }));
    }

    public string convertObjectIntoJson<T>(T objToConvert)
    {
        // Convert any object to JSON
        return JsonConvert.SerializeObject(objToConvert);
    }

    public string EscapeJson(string json)
    {
        // Escape double quotes and backslashes
        return json.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }

    protected virtual void OnMessage(object sender, MessageEventArgs e) { }


}
