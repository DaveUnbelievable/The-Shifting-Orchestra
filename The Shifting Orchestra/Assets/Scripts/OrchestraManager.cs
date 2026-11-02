using System.Collections.Generic;
using UnityEngine;

public class OrchestraManager
{
    public List<InstrumentGroup> orchestra;
    public int KeyChange;
    public int Tempo;

    public void CreateStartingOrchestra()
    {
        orchestra = new List<InstrumentGroup>();
        foreach (StartingInstrumentGroup startingGroup in GameDatabase.Instance.StartingInstrumentGroups)
        {
            // create instrument group
            InstrumentGroup instrumentGroup = new InstrumentGroup
            {
                InstrumentType = startingGroup.InstrumentType,
                MusicalRole = startingGroup.MusicalRole,
                Volume = startingGroup.Volume,
            };
            // create musicians for the instrument group
            for (int i = 0; i < startingGroup.Amount; i++)
            {
                instrumentGroup.Musicians.Add(MusicianGenerator.GenerateRandomMusician(startingGroup.InstrumentType));
            }
            // add to orchestra
            orchestra.Add(instrumentGroup);
        }
    }

    public void ChangeMusicalRoleAssignment(Enums.MusicalRoles role, InstrumentGroup instrumentGroup)
    {
        instrumentGroup.MusicalRole = role;
    }

    public void ChangeKey(int addKey)
    {
        KeyChange += addKey;
    }

    public void SetTempo(int newTempo)
    {
        Tempo = newTempo;
    }

    public void SetVolume(InstrumentGroup instrumentGroup, int newVolume)
    {
        instrumentGroup.Volume = newVolume;
    }

    // TODO:
    // recruit musician
    // fire musician
    // change position of instrument group in orchestra
    
}