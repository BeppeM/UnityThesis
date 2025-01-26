using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryType : GenericArtifactType
{
    public override ArtifactTypeEnum GetArtifactType()
    {
        return ArtifactTypeEnum.Inventory;
    }
}
