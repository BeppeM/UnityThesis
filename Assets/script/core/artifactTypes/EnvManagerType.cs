using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvManagerType : GenericArtifactType
{
    public override ArtifactTypeEnum GetArtifactType()
    {
        return ArtifactTypeEnum.EnvManager;
    }
}
