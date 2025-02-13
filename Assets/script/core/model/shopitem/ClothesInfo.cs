using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClothesInfo : BasicItemInfo
{
    // annotation used to send the enum name and not the integer index
    [JsonConverter(typeof(StringEnumConverter))]
    public ClothesEnum itemName;    
}