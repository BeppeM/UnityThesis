using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;

[System.Serializable]
public class InitialShopperAgentBeliefs
{
    public List<ItemToBuy> itemsToBuy;
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


    public List<ItemToBuy> ItemsToBuy
    {
        get { return itemsToBuy; }
        set { itemsToBuy = value; }
    }

    // Constructor to initialize the properties
    public InitialShopperAgentBeliefs(List<ItemToBuy> itemsToBuy, float budget)
    {
        ItemsToBuy = itemsToBuy;
        Budget = budget;
    }

    // Default constructor
    public InitialShopperAgentBeliefs()
    {
        ItemsToBuy = new List<ItemToBuy>();
        Budget = 0.0f;
    }

    // Method to generate beliefs as string to fill .jcm file
    public string GetBeliefsAsLiterals()
    {
        StringBuilder beliefs = new StringBuilder();
        // Add the budget belief
        beliefs.Append($"budget({Budget})");

        if (itemsToBuy != null && itemsToBuy.Count != 0)
        {            
            string temp = "[" + string.Join(", ", itemsToBuy.Select(item => item.GetItemAsLiteral())) + "]";
            beliefs.Append($", shoppingList({temp})");
        }
        return beliefs.ToString();
    }
}
