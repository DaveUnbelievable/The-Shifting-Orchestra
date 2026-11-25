using System.Collections.Generic;
using UnityEngine;

public class MusicianGenerator : MonoBehaviour
{
    #region Singleton
    public static MusicianGenerator Instance { get; private set; }
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

    public List<string> MaleFirstNames;
    public List<string> FemaleFirstNames;
    public List<string> LastNames;

    public GameObject musicianPrefab;
    public GameObject instrumentPrefab;

    public Musician GenerateRandomMusician(Enums.InstrumentType instrumentType)
    {
        // generate gender and choose name accordingly
        Enums.MusicianGender gender = Enums.MusicianGender.MALE;
        if (Random.Range(0f, 1f) <= 0.5f)
        {
            gender = Enums.MusicianGender.FEMALE;
        }
        // complete musician
        return new Musician(instrumentType, gender);
    }

    public GameObject GenerateMusicianVisuals(Musician musician)
    {
        GameObject musicianVisual = new GameObject();
        // instantiate musician prefab
        GameObject musicianBody = Instantiate(musicianPrefab);
        musicianBody.transform.parent = musicianVisual.transform;
        musicianBody.transform.localPosition = Vector3.zero;
        MusicianVisual musicianBodyVisual = musicianBody.GetComponent<MusicianVisual>();
        musicianBodyVisual.SetMusician(musician);
        // instantiate instrument prefab
        GameObject musicianInstrument = Instantiate(instrumentPrefab);
        musicianInstrument.transform.parent = musicianVisual.transform;
        musicianInstrument.transform.localPosition = Vector3.zero;
        return musicianVisual;
    }
}
