using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class OrchestraLayout : MonoBehaviour
{
    #region Singleton
    public static OrchestraLayout Instance { get; private set; }
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

    public int PositionsSeats;
    public float SeatDistance;
    public GameObject SeatPrefab;
    public GameObject Conductor;
    public List<OrchestraPositionDefinition> PositionDefinitions;

    private List<GameObject> orchestraPositionObjects;

    public void GenerateLayout()
    {
        // delete current positions
        if (orchestraPositionObjects != null)
        {
            foreach (GameObject orchestraPosition in orchestraPositionObjects)
            {
                Destroy(orchestraPosition);
            }
        }
        // new list for generated orchestra positions
        orchestraPositionObjects = new List<GameObject>();
        // create positions
        foreach (OrchestraPositionDefinition positionDefinition in PositionDefinitions)
        {
            // create game object
            GameObject orchestraPositionObject = new GameObject
            {
                name = positionDefinition.PositionType.ToString()
            };
            orchestraPositionObject.transform.parent = transform;
            // set offset position and rotation
            orchestraPositionObject.transform.SetLocalPositionAndRotation(positionDefinition.Offset, positionDefinition.Rotation);
            // add orchestraposition
            OrchestraPosition orchestraPosition = orchestraPositionObject.AddComponent<OrchestraPosition>();
            // generate seats
            int seats = PositionsSeats;
            if (positionDefinition.MusicalRole == Enums.MusicalRole.SOLO)
            {
                seats = 1;
            }
            orchestraPosition.GenerateSeats(positionDefinition, seats, SeatDistance, SeatPrefab);
            // add to list
            orchestraPositionObjects.Add(orchestraPositionObject);
        }
    }

    [Button("ShowOrchestra")]
    public void ShowOrchestra()
    {
        // clear all seats
        foreach (GameObject orchestraPositionObject in orchestraPositionObjects)
        {
            OrchestraPosition orchestraPosition = orchestraPositionObject.GetComponent<OrchestraPosition>();
            orchestraPosition.ClearSeats();
        }
        // seat all instrument groups
        foreach (InstrumentGroup instrumentGroup in OrchestraManager.Instance.Orchestra)
        {
            // show in random position for now
            foreach (GameObject orchestraPositionObject in orchestraPositionObjects)
            {
                // get a position with the right musical role
                OrchestraPosition orchestraPosition = orchestraPositionObject.GetComponent<OrchestraPosition>();
                if (orchestraPosition.OrchestraPositionDefinition.MusicalRole == instrumentGroup.MusicalRole)
                {
                    bool seated = orchestraPosition.SeatInstrumentGroup(instrumentGroup, Conductor);
                    // end foreach if instrument group is seated successfully (else continue to next position)
                    if (seated)
                    {
                        break;
                    }
                }
            }
        }
    }
}
