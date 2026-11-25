using System;
using UnityEngine;


[Serializable]
public struct OrchestraPositionDefinition
{
    public Enums.OrchestraPositionType PositionType;
    public Enums.MusicalRole MusicalRole;
    public Enums.OrchestraPositionType DependsOn;
}
