using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;

public interface AgentBeliefs
{

    // Method to generate beliefs as string to fill .jcm file
    public string GetBeliefsAsLiterals();

}
