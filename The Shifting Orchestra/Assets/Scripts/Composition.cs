using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Composition", menuName = "ScriptableObjects/Composition")]
public class Composition : ScriptableObject
{
    public string Name;
    public Enums.AudioClipKey Key;
    public float Tempo;
    public List<AudioClipData> AudioClips;
    public string AudioClipsDirectory;

    /// <summary>
    /// Applies the name of the audio clip data based on the instrument and musical role. This is useful for organizing and identifying audio clips in the Unity editor.
    /// </summary>
    public void OnValidate()
    {
        SetInspectorObjectNames();
    }

    private void SetInspectorObjectNames()
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
                    audioClipVariant.DisplayName = $"{audioClipVariant.ClipKey} - {audioClipVariant.ClipSpeed}";
                }
            }
        }
    }

    #if UNITY_EDITOR
    [Button("GetAllAudioClips")]
    public void GetAllAudioClips()
    {
        // clear current list
        if (AudioClips != null)
        {
            AudioClips.Clear();
        }
        AudioClips = new List<AudioClipData>();
        // get all audio clips from directory
        // string audioClipsDirectoryPath = "Assets/Compositions/AudioClips";
        List<AudioClip> allAudioClips = new List<AudioClip>();
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:AudioClip", new[] { AudioClipsDirectory });
        foreach (string guid in guids)
        {
            AudioClip audioClip = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
            if (audioClip != null)
            {
                Debug.Log(audioClip.name);
                allAudioClips.Add(audioClip);
            }
        }
        // iterate through each audio clip
        foreach (AudioClip audioClip in allAudioClips)
        {
            // get parts of name
            string[] nameParts = audioClip.name.Split('-');
            // first part is instrument group
            bool instrumentTypeParsed = Enum.TryParse(nameParts[0], out Enums.InstrumentType instrumentType);
            // second part is musical role
            bool musicalRoleParsed = Enum.TryParse(nameParts[1], out Enums.MusicalRole musicalRole);
            // third part is key
            bool audioClipKeyParsed = Enum.TryParse(nameParts[2], out Enums.AudioClipKey audioClipKey);
            // fourth part is tempo
            bool audioClipSpeedParsed = Enum.TryParse(nameParts[3], out Enums.AudioClipSpeed audioClipSpeed);
            // if all got parsed right save in audioclipdata
            if (instrumentTypeParsed && musicalRoleParsed && audioClipKeyParsed && audioClipSpeedParsed)
            {
                bool clipDataFound = false;
                foreach (AudioClipData audioClipData in AudioClips)
                {
                    // check if there already is an audioclipdata
                    if (audioClipData.instrument == instrumentType && audioClipData.musicalRole == musicalRole)
                    {
                        // if found, save audioclip in audioclipdata under the right key and speed
                        AudioClipVariant variant = new AudioClipVariant
                        {
                            Clip = audioClip,
                            ClipKey = audioClipKey,
                            ClipSpeed = audioClipSpeed
                        };
                        audioClipData.audioClips.Add(variant);
                        // clip data was found -> break out of loop
                        clipDataFound = true;
                        break;
                    }
                }
                // if not found create new audioclipdata and save audioclip inside
                if (!clipDataFound)
                {
                    // create audioclipvariant
                    AudioClipVariant variant = new AudioClipVariant
                    {
                        Clip = audioClip,
                        ClipKey = audioClipKey,
                        ClipSpeed = audioClipSpeed
                    };
                    // create new audioClipData
                    AudioClipData audioClipData = new AudioClipData
                    {
                        instrument = instrumentType,
                        musicalRole = musicalRole,
                        audioClips = new List<AudioClipVariant>{variant},
                    };
                    // save audioClipData in list
                    AudioClips.Add(audioClipData);
                }
            }
        }
        // save changes
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
        // set inspector object names
        SetInspectorObjectNames();
    }
    #endif
}