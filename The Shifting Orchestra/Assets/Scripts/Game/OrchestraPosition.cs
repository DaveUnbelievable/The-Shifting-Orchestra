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
        // new list for generated seats
        this.generatedSeats = new List<GameObject>();
        int generatedSeats = 0;
        // generate rows
        for (int r = 0; r < positionDefinition.Rows; r++)
        {
            // calculate y position for each seat in row
            float posY = r * seatDistance;
            // get how many seats fit in this row (either remaining seats or full amount of columns)
            int seatsInThisRow = Math.Min(amountSeats - generatedSeats, positionDefinition.Columns);
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
                    // TODO: Error?
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

}
