using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;

[System.Serializable]
public class TaskModel
{
    // Property to store the type of task to perform
    public TaskToPerformEnum taskToPerform;

    // Property to store the list of items to buy
    public List<ShopItemEnum> shoppingList;

    public TaskToPerformEnum TaskToPerform
    {
        get
        {
            return taskToPerform;
        }
        set
        {
            taskToPerform = value;
        }
    }


    public List<ShopItemEnum> ShoppingList
    {
        get
        {
            return shoppingList;
        }
        set
        {
            shoppingList = value;
        }
    }


    // Constructor to initialize the properties
    public TaskModel(TaskToPerformEnum taskToPerform, List<string> shoppingList)
    {
        TaskToPerform = taskToPerform;
        shoppingList = shoppingList ?? new List<string>();        
    }
    
    public override string ToString()
    {
        return $"TaskToPerform: {taskToPerform}, ShoppingList: {string.Join(", ", shoppingList)}";
    }

    // Format object into taskToPerform(artifactTypeToReach, [item1, item2, ....])
    // e.g. reach_fruit_seller(fruitshop,[mango, pineapple])
    public string ToLiteralBelief()
    {
        string payload = $"{taskToPerform}";

        // Retrieve dictionary to map each task to each artifact type
        Dictionary<TaskToPerformEnum, AgentArtifactTypeEnum>  taskToArtifactType= 
            UnityJacamoIntegrationUtil.ArtifactTypeFromTaskToPerform;

        // Retrieve artifact type
        AgentArtifactTypeEnum artifactType = taskToArtifactType[taskToPerform];
        payload += $"({artifactType}";

        if(shoppingList != null && shoppingList.Count != 0){
            payload += $",[{string.Join(", ", ShoppingList.Select(item => item.ToString().ToLowerInvariant()))}]";
        }
        payload += ")";
        return payload;
    }
}