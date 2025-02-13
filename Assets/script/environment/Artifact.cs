using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using WebSocketSharp;

[ExecuteAlways]
public class Artifact : AbstractArtifact
{
    // All artifact properties
    public List<CoffeeInfo> barProperties;
    public List<FruitInfo> fruitShopProperties;
    public List<ClothesInfo> dressShopProperties;
    public bool doorProperties;

    protected override void Awake()
    {
        base.Awake();
    }

}
