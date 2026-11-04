using UnityEngine;

public class OrchestraSeat : MonoBehaviour
{
    
    public GameObject MusicianPrefab;
    [HideInInspector]
    public bool SeatFilled = false;

    private Musician musician;
    private GameObject musicianObject;

    public void OccupySeat(Musician musician)
    {
        // show musician
        this.musician = musician;
        musicianObject = Instantiate(MusicianPrefab);
        musicianObject.transform.parent = transform;
        musicianObject.transform.localPosition = Vector3.zero;
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
