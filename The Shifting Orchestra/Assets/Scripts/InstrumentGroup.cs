using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InstrumentGroup : MonoBehaviour
{
    public Guid ID;
    public Enums.InstrumentType InstrumentType;
    public Enums.MusicalRole MusicalRole;
    public List<Musician> Musicians;
    public int Volume;

    public InstrumentGroup()
    {
        ID = Guid.NewGuid();
        Musicians = new List<Musician>();
    }
}