using UnityEngine;

public class OrchestraSeat : MonoBehaviour
{
    
    public GameObject MusicianPrefab;
    public GameObject SeatingPosition;
    [HideInInspector]
    public bool SeatFilled = false;

    private Musician musician;
    private GameObject musicianObject;

    public void OccupySeat(Musician musician)
    {
        // show musician
        this.musician = musician;
        musicianObject = MusicianGenerator.Instance.GenerateMusicianVisuals(musician);
        musicianObject.name = $"Musician {musician.Name}";
        musicianObject.transform.parent = transform;
        musicianObject.transform.localPosition = SeatingPosition.transform.localPosition;
        musicianObject.transform.localScale = new Vector3(1, 1, 1);
        // TODO: show body parts of musician and instrument
        SeatFilled = true;
    }

    public void ClearSeat()
    {
        // clear musician
        musician = null;
        Destroy(musicianObject);
        SeatFilled = false;
    }
}
