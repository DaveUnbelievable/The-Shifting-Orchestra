using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MusicianVisualInstrument
{
    public Enums.InstrumentType Type;
    public GameObject ParentObject;
    public GameObject LeftHandTarget;
    public GameObject RightHandTarget;
}
