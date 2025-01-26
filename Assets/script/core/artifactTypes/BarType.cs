using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarType : GenericArtifactType
{
    public override ArtifactTypeEnum GetArtifactType()
    {
        return ArtifactTypeEnum.Bar;
    }
}
