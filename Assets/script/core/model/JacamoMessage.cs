using System;
using Newtonsoft.Json;

[Serializable]
public class JacamoMessage
{
    [JsonProperty("jacamoEntityName")]
    public string JacamoEntityName { get; set; }

    [JsonProperty("actionType")]
    public string ActionType { get; set; }

    [JsonProperty("actionToPerform")]
    public string ActionToPerform { get; set; }

    [JsonProperty("param")]
    public object Param { get; set; }

    public JacamoMessage() { }

    public JacamoMessage(string jacamoEntityName, string actionType, string actionToPerform, object param)
    {
        JacamoEntityName = jacamoEntityName;
        ActionType = actionType;
        ActionToPerform = actionToPerform;
        Param = param;
    }
}