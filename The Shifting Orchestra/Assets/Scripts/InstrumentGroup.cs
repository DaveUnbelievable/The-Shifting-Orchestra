using System;
using System.Collections.Generic;

[Serializable]
public class InstrumentGroup
{
    public Guid ID;
    public Enums.InstrumentType InstrumentType;
    public Enums.MusicalRoles MusicalRole;
    public List<Musician> Musicians;
    public int Volume;

    public InstrumentGroup()
    {
        ID = Guid.NewGuid();
        Musicians = new List<Musician>();
    }
}