using System;
using UnityEngine;


[Serializable]
public struct OrchestraPositionDefinition
{
    public Enums.OrchestraPositionType PositionType;
    public Vector2 Offset;
    public Quaternion Rotation;
    public int Rows;
    public int Columns;
    public Enums.MusicalRole MusicalRole;
}
