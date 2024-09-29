using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClothesInfo
{
    [JsonConverter(typeof(StringEnumConverter))]
    public ClothesEnum itemName;
    public int price;
    public int quantity;
}