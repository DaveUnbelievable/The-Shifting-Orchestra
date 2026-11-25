using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OrchestraPosition : MonoBehaviour
{
    private List<GameObject> generatedSeats;
    public OrchestraPositionDefinition OrchestraPositionDefinition;

    public int generatedRows = 0;

    public void GenerateSeats(OrchestraPositionDefinition positionDefinition, int amountSeats, float seatDistanceHorizontal, float seatDistanceVertical, GameObject seatPrefab)
    {
        // save definition
        OrchestraPositionDefinition = positionDefinition;
        // delete current seats
        if (this.generatedSeats != null)
        {
            foreach (GameObject seat in this.generatedSeats)
            {
                Destroy(seat);
            }
        }
        int columns = Mathf.CeilToInt(Mathf.Sqrt(amountSeats));
        int rows = Mathf.CeilToInt((float)amountSeats / columns);
        // save amount of generated rows
        generatedRows = rows;
        // new list for generated seats
        this.generatedSeats = new List<GameObject>();
        int generatedSeats = 0;
        // generate rows
        for (int r = 0; r < rows; r++)
        {
            // get how many seats fit in this row (either remaining seats or full amount of columns)
            int seatsInThisRow = Math.Min(amountSeats - generatedSeats, columns);
            // create row
            GameObject row = new GameObject();
            row.name = $"row {r}";
            SortingGroup rowSortingGroup = row.AddComponent<SortingGroup>();
            rowSortingGroup.sortingOrder = rows - r;
            row.transform.parent = transform;
            // calculate x position for row
            float rowPosX = 0;
            if (seatsInThisRow != columns || positionDefinition.PositionType == Enums.OrchestraPositionType.BACK_FULL)
            {
                rowPosX = -((columns - seatsInThisRow) * seatDistanceHorizontal) / 2;
                if (
                    positionDefinition.PositionType == Enums.OrchestraPositionType.FRONT_RIGHT ||
                    positionDefinition.PositionType == Enums.OrchestraPositionType.MIDDLE_RIGHT ||
                    positionDefinition.PositionType == Enums.OrchestraPositionType.BACK_RIGHT
                )
                {
                    rowPosX *= -1;
                }
            }
            if (positionDefinition.PositionType == Enums.OrchestraPositionType.BACK_FULL)
            {
                rowPosX = (seatsInThisRow - 1) * seatDistanceHorizontal / 2;
            }
            // calculate y position for row
            row.transform.localPosition = new Vector3(rowPosX, r * seatDistanceVertical, 0);
            // generate seats
            for (int s = 0; s < seatsInThisRow; s++)
            {
                // calculate x position for seat
                float seatPosX = -(s * seatDistanceHorizontal);
                if (
                    positionDefinition.PositionType == Enums.OrchestraPositionType.FRONT_RIGHT ||
                    positionDefinition.PositionType == Enums.OrchestraPositionType.MIDDLE_RIGHT ||
                    positionDefinition.PositionType == Enums.OrchestraPositionType.BACK_RIGHT
                )
                {
                    seatPosX *= -1;
                }
                // instantiate seat prefab
                GameObject seat = Instantiate(seatPrefab, row.transform);
                seat.name = $"seat {r} - {s}";
                // rotate if right side, TODO: can i just use the global x position and if that is > 0 turn it arround?
                switch (positionDefinition.PositionType)
                {
                    case Enums.OrchestraPositionType.FRONT_RIGHT:
                    case Enums.OrchestraPositionType.MIDDLE_RIGHT:
                    case Enums.OrchestraPositionType.BACK_RIGHT:
                        seat.transform.localScale = new Vector3(-1, 1, 1);
                        break;
                    case Enums.OrchestraPositionType.BACK_FULL:
                        // TODO: doesnt work with the row game object... -> get position in seating group?
                        if (seatPosX > 0)
                        {
                            seat.transform.localScale = new Vector3(-1, 1, 1);
                        }
                        break;
                }
                // position seat prefab
                seat.transform.localPosition = new Vector3(seatPosX, 0, 0);
                // save seat in list
                this.generatedSeats.Add(seat);
            }
            // add generated seats to counter
            generatedSeats += seatsInThisRow;
        }
    }

    public bool SeatInstrumentGroup(InstrumentGroup instrumentGroup, GameObject conductor)
    {
        int freeSeats = 0;
        // check how many seats are free
        foreach (GameObject generatedSeat in generatedSeats)
        {
            OrchestraSeat orchestraSeat = generatedSeat.GetComponent<OrchestraSeat>();
            if (!orchestraSeat.SeatFilled)
            {
                freeSeats++;
            }
        }
        // check if enough seats are available for the instrument group
        if (instrumentGroup.Musicians.Count <= freeSeats)
        {
            // set each musician down on a seat
            foreach (Musician musician in instrumentGroup.Musicians)
            {
                GameObject nearestSeat = null;
                float smallestDistance = float.PositiveInfinity;
                foreach (GameObject generatedSeat in generatedSeats)
                {
                    // only unoccupied seats
                    OrchestraSeat orchestraSeat = generatedSeat.GetComponent<OrchestraSeat>();
                    if (!orchestraSeat.SeatFilled)
                    {
                        // get distance to conductor
                        float distance = Vector3.Distance(generatedSeat.transform.position, conductor.transform.position);
                        // set seat if distance is smaller
                        if (distance < smallestDistance)
                        {
                            nearestSeat = generatedSeat;
                            smallestDistance = distance;
                        }
                    }
                }
                if (nearestSeat != null)
                {
                    OrchestraSeat orchestraSeat = nearestSeat.GetComponent<OrchestraSeat>();
                    orchestraSeat.OccupySeat(musician);
                }
                else
                {
                    // no seat found
                    return false;
                }
            }
            // musicians seated
            return true;
        }
        // not enough seats for instrument group
        return false;
    }

    public void ClearSeats()
    {
        foreach (GameObject generatedSeat in generatedSeats)
        {
            OrchestraSeat orchestraSeat = generatedSeat.GetComponent<OrchestraSeat>();
            orchestraSeat.ClearSeat();
        }
    }

    /*
    
    TODO:
    - create a function GetAnchorPoint(corner) -> Vector2? Enum? need corners and middle points..
        -> get the corner of the seating group
    - create a field where i can put a reference seating group to orientate at
    - create a field where the anchor point of the object should be (corner, middle points...)
    - create a field where it should position itself
    - position the object at the given position with the position from the reference seating group (if none is given use conductor? or just zero?)

    */

}
