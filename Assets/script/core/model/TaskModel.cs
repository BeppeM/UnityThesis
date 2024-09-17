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

    // Optional: Override ToString() method for easier debugging and logging
    public override string ToString()
    {
        return $"TaskToPerform: {taskToPerform}, ShoppingList: {string.Join(", ", shoppingList)}";
    }

    // Format object into taskToPerform([item1, item2, ....])
    public string ToLiteralBelief()
    {
        return $"{taskToPerform}([{string.Join(", ", ShoppingList.Select(item => item.ToString().ToLowerInvariant()))}])";
    }
}