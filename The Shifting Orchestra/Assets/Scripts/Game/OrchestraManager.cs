using System.Collections.Generic;
using UnityEngine;

public class OrchestraManager : MonoBehaviour
{
    #region Singleton
    public static OrchestraManager Instance { get; private set; }
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public Enums.AudioClipKey Key;
    public Enums.AudioClipSpeed Tempo;

    // TODO: possibility to not use an instrument group in a gig
    public Dictionary<Enums.InstrumentType, InstrumentGroup> InstrumentGroups;

    public void CreateStartingOrchestra()
    {
        InstrumentGroups = new Dictionary<Enums.InstrumentType, InstrumentGroup>();
        foreach (StartingInstrumentGroup startingGroup in GameDatabase.Instance.StartingInstrumentGroups)
        {
            InstrumentGroup instrumentGroup = CreateInstrumentGroup(startingGroup.InstrumentType, startingGroup.MusicalRole, startingGroup.Volume);
            // add instrument group to list
            InstrumentGroups.Add(startingGroup.InstrumentType, instrumentGroup);
            // create musicians for the instrument group
            for (int i = 0; i < startingGroup.Amount; i++)
            {
                instrumentGroup.Musicians.Add(MusicianGenerator.Instance.GenerateRandomMusician(startingGroup.InstrumentType));
            }
        }
        // show orchestra in layout
        OrchestraLayout.Instance.ShowOrchestra();
    }

    public InstrumentGroup CreateInstrumentGroup(Enums.InstrumentType instrumentType, Enums.MusicalRole role, float volume)
    {
        // create gameobject for instrument group
        GameObject instrumentGroupObject = new GameObject
        {
            name = instrumentType.ToString()
        };
        instrumentGroupObject.transform.parent = transform;
        // create instrument group
        InstrumentGroup instrumentGroup = instrumentGroupObject.AddComponent<InstrumentGroup>();
        instrumentGroup.InstrumentType = instrumentType;
        instrumentGroup.MusicalRole = role;
        instrumentGroup.Volume = volume;
        // return group
        return instrumentGroup;
    }

    public bool ChangeMusicalRoleAssignment(InstrumentGroup instrumentGroup, Enums.MusicalRole role)
    {
        if (!OrchestraLayout.Instance.CanSeatInstrumentGroupInRole(instrumentGroup, role))
        {
            Debug.LogWarning($"Cannot assign {instrumentGroup.InstrumentType} to {role}: there are not enough seats for all instrument groups assigned to this role.");
            return false;
        }

        instrumentGroup.MusicalRole = role;
        OrchestraLayout.Instance.ShowOrchestra();
        return true;
    }

    public void SetVolume(InstrumentGroup instrumentGroup, float newVolume)
    {
        instrumentGroup.Volume = newVolume;
    }

    public void HireMusician(Musician musician)
    {
        // create instrument group if there is none for this instrument type
        if (!InstrumentGroups.ContainsKey(musician.InstrumentType))
        {
            InstrumentGroups.Add(musician.InstrumentType, CreateInstrumentGroup(musician.InstrumentType, Enums.MusicalRole.MELODY, GameDatabase.Instance.VolumeBase));
        }
        // add musician to instrument group
        InstrumentGroups[musician.InstrumentType].Musicians.Add(musician);
        // show orchestra in layout
        OrchestraLayout.Instance.ShowOrchestra();
    }

    public void FireMusician(Musician musician)
    {
        if (InstrumentGroups.ContainsKey(musician.InstrumentType))
        {
            if (InstrumentGroups[musician.InstrumentType].Musicians.Contains(musician))
            {
                // remove musician
                InstrumentGroups[musician.InstrumentType].Musicians.Remove(musician);
                // show orchestra in layout
                OrchestraLayout.Instance.ShowOrchestra();
            }
        }
    }

    public InstrumentGroup GetInstrumentGroup(Enums.InstrumentType instrumentType)
    {
        foreach (KeyValuePair<Enums.InstrumentType, InstrumentGroup> instrumentGroup in InstrumentGroups)
        {
            if (instrumentGroup.Key == instrumentType)
            {
                return instrumentGroup.Value;
            }
        }
        return null;
    }
    
}