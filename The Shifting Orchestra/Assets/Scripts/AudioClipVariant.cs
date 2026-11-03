using System;
using UnityEngine;

[Serializable]
public class AudioClipVariant
{
    [HideInInspector]
    public string DisplayName;
    public AudioClip audioClip;
    public Enums.AudioClipKey audioClipKey;
    public Enums.AudioClipSpeed audioClipSpeed;
}
