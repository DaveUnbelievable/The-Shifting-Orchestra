using System;
using System.Collections.Generic;
using UnityEngine;

public class OrchestraPosition : MonoBehaviour
{
    private List<GameObject> generatedSeats;
    public OrchestraPositionDefinition OrchestraPositionDefinition;

    public void GenerateSeats(OrchestraPositionDefinition positionDefinition, int amountSeats, float seatDistance, GameObject seatPrefab)
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
        int rows = 0;
        int columns = 0;
        if (amountSeats > 0)
        {
            switch (positionDefinition.SeatLayout)
            {
                case Enums.OrchestraSeatLayout.SQUARE:
                    columns = Mathf.CeilToInt(Mathf.Sqrt(amountSeats));
                    rows = Mathf.CeilToInt((float)amountSeats / columns);
                    break;
                case Enums.OrchestraSeatLayout.WIDE_RECTANGLE:
                    rows = Mathf.Max(1, Mathf.CeilToInt(Mathf.Sqrt(amountSeats) / 2)); // TODO: try with 3? or 4?
                    columns = Mathf.CeilToInt((float)amountSeats / rows);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(positionDefinition.SeatLayout), positionDefinition.SeatLayout, "Unsupported orchestra seat layout.");
            }
        }
        // calculate anchor offset
        Vector2 anchorOffset = Vector2.zero;
        switch(positionDefinition.PositionType)
        {
            case Enums.OrchestraPositionType.FRONT_LEFT:
                anchorOffset.x = columns / 2 * seatDistance;
                break;
            case Enums.OrchestraPositionType.FRONT_RIGHT:
                anchorOffset.x = -(columns / 2 * seatDistance);
                break;
        }
        // new list for generated seats
        this.generatedSeats = new List<GameObject>();
        int generatedSeats = 0;
        // generate rows
        for (int r = 0; r < rows; r++)
        {
            // calculate y position for each seat in row
            float posY = r * seatDistance;
            // get how many seats fit in this row (either remaining seats or full amount of columns)
            int seatsInThisRow = Math.Min(amountSeats - generatedSeats, columns);
            float offset = seatDistance * (seatsInThisRow - 1) / 2;
            // generate seats
            for (int s = 0; s < seatsInThisRow; s++)
            {
                // calculate x position for seat
                float posX = s * seatDistance - offset;
                // instantiate seat prefab
                GameObject seat = Instantiate(seatPrefab, transform);
                // position seat prefab
                seat.transform.localPosition = new Vector3(posX, posY, 0);
                // save seat in list
                this.generatedSeats.Add(seat);
            }
            // add generated seats to counter
            generatedSeats += seatsInThisRow;
        }
        // calculate the offset of the instrument group dynamically
    }

    private Vector2 CalculateDynamicOffset()
    {
        return Vector2.zero;
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
