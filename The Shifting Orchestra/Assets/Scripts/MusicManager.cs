using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UIElements;

public class MusicManager : MonoBehaviour
{
    #region Singleton
    public static MusicManager Instance { get; private set; }
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

    public Composition composition;

    private List<AudioSource> audioSources;

    [Button("Play Music")]
    public void PlayComposition( )
    {
        // remove previous audio sources
        RemoveAllAudioSources();

        // get the orchestra arrangement; which instrument plays which musical role
        // set volume per audio clip
        // set tempo and key of the composition
        // play audio clips according to the arrangement

        audioSources = new List<AudioSource>();

        foreach (InstrumentGroup instrumentGroup in OrchestraManager.Instance.Orchestra)
        {
            if (OrchestraManager.Instance.InstrumentGroupObjects.ContainsKey(instrumentGroup.InstrumentType))
            {
                AudioSource audioSource = OrchestraManager.Instance.InstrumentGroupObjects[instrumentGroup.InstrumentType].AddComponent<AudioSource>();
                // set correct audio clip
                audioSource.clip = GetCorrectAudioClip(instrumentGroup.InstrumentType, instrumentGroup.MusicalRole, OrchestraManager.Instance.Key, OrchestraManager.Instance.Tempo);
                audioSource.volume = instrumentGroup.Volume / GameDatabase.Instance.VolumeMax;
                audioSources.Add(audioSource);
            }
        }


        // get position of instruments in the orchestra
        // place audio sources on the scene on the instruments
        // audio listener on the conductor
        // play all audio at the same time


        double startTime = AudioSettings.dspTime + 0.1;
        for (int i = 0; i < audioSources.Count; i++)
        {
            audioSources[i].PlayScheduled(startTime);
        }
        

    }

    private AudioClip GetCorrectAudioClip(Enums.InstrumentType instrumentType, Enums.MusicalRole musicalRole, Enums.AudioClipKey key, Enums.AudioClipSpeed speed)
    {
        AudioClip clip = null;
        // get audipclipdata with right instrument type and musical role
        foreach (AudioClipData audioClipData in composition.AudioClips)
        {
            if (audioClipData.instrument == instrumentType && audioClipData.musicalRole == musicalRole)
            {
                // get clip with the right key and speed
                foreach (AudioClipVariant variant in audioClipData.audioClips)
                {
                    if (variant.ClipKey == key && variant.ClipSpeed == speed)
                    {
                        clip = variant.Clip;
                    }
                }
            }
        }
        if (clip == null)
        {
            Debug.LogWarning($"AudioClip für {instrumentType} - {musicalRole} - {key} - {speed} fehlt!");
        }
        return clip;
    }

    private void RemoveAllAudioSources()
    {
        if (audioSources != null)
        {
            foreach (AudioSource audioSource in audioSources)
            {
                Destroy(audioSource);
            }
        }
    }

}
