using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[System.Serializable]
public class InitialShopperAgentBeliefs
{
    public List<string> shoppingList;
    public float budget;

    public float Budget
    {
        get
        {
            return budget;
        }
        set
        {
            budget = value;
        }
    }

    public List<string> ShoppingList
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
    public InitialShopperAgentBeliefs(List<string> shoppingList, float budget)
    {
        ShoppingList = shoppingList;
        Budget = budget;
    }

    // Default constructor
    public InitialShopperAgentBeliefs()
    {
        ShoppingList = new List<string>();
        Budget = 0.0f;
    }

    // Method to generate beliefs as string to fill .jcm file
    public string GetBeliefsString()
    {
        StringBuilder beliefs = new StringBuilder();

        // Add the budget belief
        beliefs.Append($"budget({Budget}), ");

        // Add the shopping list belief
        beliefs.Append("shoppingList([");
        for (int i = 0; i < ShoppingList.Count; i++)
        {
            beliefs.Append(ShoppingList[i]);
            if (i < ShoppingList.Count - 1)
            {
                beliefs.Append(", ");
            }
        }
        beliefs.Append("])");

        return beliefs.ToString();
    }
}
