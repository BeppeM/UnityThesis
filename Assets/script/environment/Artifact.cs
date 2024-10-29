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
public class Artifact : MASAbstract
{
    // List of all property names
    private List<string> propertyNames = new List<string>();
    
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
        propertyNames.Clear();
        // Retrieve all fields
        FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (field.Name != "port" && field.Name != "objInUse")
            {
                propertyNames.Add(field.Name);
            }
        }
        objInUse = gameObject;
        artifactType = gameObject.GetComponent<GenericArtifactType>().GetShopType();

        if (Application.IsPlaying(gameObject))
        {
            // Play logic            
            // Retrieve the property that belongs to the artifact       
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
            foreach (string prop in propertyNames)
            {
                print(prop);
            }
        }
    }

    protected virtual void OnMessage(object sender, MessageEventArgs e) { }


}
