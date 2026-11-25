using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
    public float SeatDistanceHorizontal;
    public float SeatDistanceVertical;
    public float MiddleDistance;
    public GameObject SeatPrefab;
    public GameObject Conductor;
    public List<OrchestraPositionDefinition> PositionDefinitions;

    private Dictionary<Enums.OrchestraPositionType, GameObject> orchestraPositionObjects;

    public void GenerateLayout()
    {
        // delete current positions
        if (orchestraPositionObjects != null)
        {
            foreach (KeyValuePair<Enums.OrchestraPositionType, GameObject> orchestraPosition in orchestraPositionObjects)
            {
                Destroy(orchestraPosition.Value);
            }
        }
        // new list for generated orchestra positions
        orchestraPositionObjects = new Dictionary<Enums.OrchestraPositionType, GameObject>();
        // create positions
        foreach (OrchestraPositionDefinition positionDefinition in PositionDefinitions)
        {
            // create game object
            GameObject orchestraPositionObject = new GameObject
            {
                name = positionDefinition.PositionType.ToString()
            };
            orchestraPositionObject.transform.parent = transform;
            // get sorting order for seating group
            int sortingOrder = 0;
            // set offset position and rotation
            Vector2 positionOffset = Vector2.zero;
            Vector3 rotation = Vector3.zero;
            // set x
            switch (positionDefinition.PositionType)
            {
                case Enums.OrchestraPositionType.FRONT_LEFT:
                    positionOffset.x = -MiddleDistance * 3;
                    sortingOrder = 3;
                    break;
                case Enums.OrchestraPositionType.MIDDLE_LEFT:
                    positionOffset.x = -MiddleDistance * 2;
                    sortingOrder = 2;
                    break;
                case Enums.OrchestraPositionType.BACK_LEFT:
                    positionOffset.x = -MiddleDistance;
                    sortingOrder = 1;
                    break;
                case Enums.OrchestraPositionType.FRONT_RIGHT:
                    positionOffset.x = MiddleDistance * 3;
                    sortingOrder = 3;
                    break;
                case Enums.OrchestraPositionType.MIDDLE_RIGHT:
                    positionOffset.x = MiddleDistance * 2;
                    sortingOrder = 2;
                    break;
                case Enums.OrchestraPositionType.BACK_RIGHT:
                    positionOffset.x = MiddleDistance;
                    sortingOrder = 1;
                    break;
            }
            // set sorting order for renderer
            SortingGroup sortingGroup = orchestraPositionObject.AddComponent<SortingGroup>();
            sortingGroup.sortingOrder = sortingOrder;
            // set y
            if (positionDefinition.DependsOn != Enums.OrchestraPositionType.NONE)
            {
                // get position of dependent seating group
                if (orchestraPositionObjects.ContainsKey(positionDefinition.DependsOn))
                {
                    OrchestraPosition dependentOrchestraPosition = orchestraPositionObjects[positionDefinition.DependsOn].GetComponent<OrchestraPosition>();
                    positionOffset.y = orchestraPositionObjects[positionDefinition.DependsOn].transform.position.y;
                    positionOffset.y += (dependentOrchestraPosition.generatedRows + 2) * SeatDistanceVertical;
                } else
                {
                    Debug.LogWarning($"Sitzgruppe mit Position {positionDefinition.DependsOn} (für {positionDefinition.PositionType}) nicht gefunden");
                }
            }
            orchestraPositionObject.transform.localPosition = positionOffset;
            orchestraPositionObject.transform.localEulerAngles = rotation;
            // add orchestraposition
            OrchestraPosition orchestraPosition = orchestraPositionObject.AddComponent<OrchestraPosition>();
            // generate seats
            orchestraPosition.GenerateSeats(positionDefinition, PositionsSeats, SeatDistanceHorizontal, SeatDistanceVertical, SeatPrefab);
            // add to list
            orchestraPositionObjects.Add(positionDefinition.PositionType, orchestraPositionObject);
        }
    }

    public void ShowOrchestra(bool regenerateLayout = false)
    {
        // regenerate layout if wanted
        if (regenerateLayout)
        {
            GenerateLayout();
        }
        // clear all seats
        foreach (KeyValuePair<Enums.OrchestraPositionType, GameObject> orchestraPositionObject in orchestraPositionObjects)
        {
            OrchestraPosition orchestraPosition = orchestraPositionObject.Value.GetComponent<OrchestraPosition>();
            orchestraPosition.ClearSeats();
        }
        // seat all instrument groups
        foreach (KeyValuePair<Enums.InstrumentType, InstrumentGroup> instrumentGroup in OrchestraManager.Instance.InstrumentGroups)
        {
            // show in random position for now
            foreach (KeyValuePair<Enums.OrchestraPositionType, GameObject> orchestraPositionObject in orchestraPositionObjects)
            {
                // get a position with the right musical role
                OrchestraPosition orchestraPosition = orchestraPositionObject.Value.GetComponent<OrchestraPosition>();
                if (orchestraPosition.OrchestraPositionDefinition.MusicalRole == instrumentGroup.Value.MusicalRole)
                {
                    bool seated = orchestraPosition.SeatInstrumentGroup(instrumentGroup.Value, Conductor);
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
