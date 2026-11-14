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

    // are there active/inactive instrument groups? currently all would be active
    public List<InstrumentGroup> Orchestra;
    public Enums.AudioClipKey Key;
    public Enums.AudioClipSpeed Tempo;

    public Dictionary<Enums.InstrumentType, GameObject> InstrumentGroupObjects;
    
    // TODO: save musicians better

    public void CreateStartingOrchestra()
    {
        Orchestra = new List<InstrumentGroup>();
        InstrumentGroupObjects = new Dictionary<Enums.InstrumentType, GameObject>();
        foreach (StartingInstrumentGroup startingGroup in GameDatabase.Instance.StartingInstrumentGroups)
        {
            // create gameobject for instrument group
            GameObject instrumentGroupObject = new GameObject
            {
                name = startingGroup.InstrumentType.ToString()
            };
            instrumentGroupObject.transform.parent = transform;
            // add gameobject to list
            InstrumentGroupObjects.Add(startingGroup.InstrumentType, instrumentGroupObject);
            // create instrument group
            InstrumentGroup instrumentGroup = instrumentGroupObject.AddComponent<InstrumentGroup>();
            instrumentGroup.InstrumentType = startingGroup.InstrumentType;
            instrumentGroup.MusicalRole = startingGroup.MusicalRole;
            instrumentGroup.Volume = startingGroup.Volume;
            // create musicians for the instrument group
            for (int i = 0; i < startingGroup.Amount; i++)
            {
                instrumentGroup.Musicians.Add(MusicianGenerator.GenerateRandomMusician(startingGroup.InstrumentType));
            }
            // add to orchestra
            Orchestra.Add(instrumentGroup);
        }
        // show orchestra in layout
        OrchestraLayout.Instance.ShowOrchestra();
    }

    public void ChangeMusicalRoleAssignment(InstrumentGroup instrumentGroup, Enums.MusicalRole role)
    {
        // TODO: check if there are enough seats for the instrument group in the musical role
        instrumentGroup.MusicalRole = role;
        OrchestraLayout.Instance.ShowOrchestra();
    }

    public void SetVolume(InstrumentGroup instrumentGroup, float newVolume)
    {
        instrumentGroup.Volume = newVolume;
    }

    // TODO:
    // recruit musician
    // fire musician

    public InstrumentGroup GetInstrumentGroup(Enums.InstrumentType instrumentType)
    {
        foreach (InstrumentGroup instrumentGroup in Orchestra)
        {
            if (instrumentGroup.InstrumentType == instrumentType)
            {
                return instrumentGroup;
            }
        }
        return null;
    }
    
}