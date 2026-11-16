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

    public GameObject musicianPrefab;
    public GameObject instrumentPrefab;

    public Musician GenerateRandomMusician(Enums.InstrumentType instrumentType)
    {
        Musician musician = new Musician
        {
            InstrumentType = instrumentType,
            Name = GenerateRandomName(),
        };
        return musician;
    }

    public GameObject GenerateMusicianVisuals(Musician musician)
    {
        GameObject musicianVisual = new GameObject();
        // instantiate musician prefab
        GameObject musicianBody = Instantiate(musicianPrefab);
        musicianBody.transform.parent = musicianVisual.transform;
        musicianBody.transform.localPosition = Vector3.zero;
        // instantiate instrument prefab
        GameObject musicianInstrument = Instantiate(instrumentPrefab);
        musicianInstrument.transform.parent = musicianVisual.transform;
        musicianInstrument.transform.localPosition = Vector3.zero;
        return musicianVisual;
    }

    private string GenerateRandomName()
    {
        // temp random generation TODO: implement proper name generation
        string[] names = { "Alice", "Bob", "Charlie" };
        int randomIndex = Random.Range(0, names.Length);
        string firstname = names[randomIndex];
        string[] lastnames = { "Smith", "Johnson", "Williams" };
        int randomIndex2 = Random.Range(0, lastnames.Length);
        string lastname = lastnames[randomIndex2];
        return firstname + " " + lastname;
    }
}
