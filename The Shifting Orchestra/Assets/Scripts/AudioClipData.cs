using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class AudioClipData
{
    [HideInInspector]
    public string DisplayName;
    public Enums.InstrumentType instrument;
    public Enums.MusicalRole musicalRole;
    public List<AudioClipVariant> audioClips;
}
