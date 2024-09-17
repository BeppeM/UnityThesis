using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;

[System.Serializable]
public class InitialShopperAgentBeliefs
{
    public List<TaskModel> taskModels;
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


    public List<TaskModel> TaskModels
    {
        get { return taskModels; }
        set { taskModels = value; }
    }

    // Constructor to initialize the properties
    public InitialShopperAgentBeliefs(List<TaskModel> taskModels, float budget)
    {
        TaskModels = taskModels;
        Budget = budget;
    }

    // Default constructor
    public InitialShopperAgentBeliefs()
    {
        TaskModels = new List<TaskModel>();
        Budget = 0.0f;
    }

    // Method to generate beliefs as string to fill .jcm file
    public string GetBeliefsString()
    {
        StringBuilder beliefs = new StringBuilder();
        // Add the budget belief
        beliefs.Append($"budget({Budget}), ");
        // Retrive all tasks to perform
        if (TaskModels != null && TaskModels.Count != 0)
        {
            string temp = string.Join(", ", TaskModels.Select(task => task.ToLiteralBelief()));
            beliefs.Append($"my_goals([{temp}])");
        }

        return beliefs.ToString();
    }
}
