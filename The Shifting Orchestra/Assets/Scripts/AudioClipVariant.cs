using System;
using UnityEngine;

[Serializable]
public class AudioClipVariant
{
    [HideInInspector]
    public string DisplayName;
    public AudioClip Clip;
    public Enums.AudioClipKey ClipKey;
    public Enums.AudioClipSpeed ClipSpeed;
}
