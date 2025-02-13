using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AbstractArtifact : AbstractMasElement
{
    // Properties in JSON format to configure .jcm file
    protected string artifactProperties;
    protected ArtifactTypeEnum artifactType;
    // List of all property names
    protected List<string> propertyNames = new List<string>();

    protected virtual void Awake()
    {
        objInUse = gameObject;
        artifactType = gameObject.GetComponent<GenericArtifactType>().GetArtifactType();
        initializeWebSocketConnection(OnMessage);
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

    public string ArtifactProperties
    {
        get { return artifactProperties; }
        set { artifactProperties = value; }
    }
    
    public ArtifactTypeEnum ArtifactType
    {
        get { return artifactType; }
    }

    public List<string> PropertyNames
    {
        get { return propertyNames; }
    }
}
