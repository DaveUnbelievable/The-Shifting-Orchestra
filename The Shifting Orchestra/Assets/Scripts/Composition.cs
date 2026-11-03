using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Composition", menuName = "ScriptableObjects/Composition")]
public class Composition : ScriptableObject
{
    public string Name;
    public Enums.AudioClipKey Key;
    public float Tempo;
    public List<AudioClipData> AudioClips;

    /// <summary>
    /// Applies the name of the audio clip data based on the instrument and musical role. This is useful for organizing and identifying audio clips in the Unity editor.
    /// </summary>
    public void OnValidate()
    {
        if (AudioClips == null )
        {
            return;
        }
        foreach (AudioClipData audioClipData in AudioClips)
        {
            if (audioClipData != null)
            {
                audioClipData.DisplayName = $"{audioClipData.instrument} - {audioClipData.musicalRole}";

                foreach (AudioClipVariant audioClipVariant in audioClipData.audioClips)
                {
                    audioClipVariant.DisplayName = $"{audioClipVariant.audioClipKey} - {audioClipVariant.audioClipSpeed}";
                }
            }
        }
    }
}