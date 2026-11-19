using System;
using System.Collections.Generic;
using UnityEngine;

public class AudienceGenerator : MonoBehaviour
{
    [SerializeField] private GameObject ViewerPrefab;
    [SerializeField] private int viewersPerRow = 10;
    [SerializeField] private float rowDistance = 0.75f;
    [SerializeField] private float viewerDistance = 0.5f;

    private List<GameObject> rows;

    public void Start()
    {
        rows = new List<GameObject>();
        // subscribe to runstate changed event
        RunManager.Instance.StateChanged += (state) =>
        {
            if (state == Enums.RunState.GIG)
            {
                GenerateAudience();
            }
        };
    }

    public void GenerateAudience()
    {
        // delete current rows
        foreach (GameObject row in rows)
        {
            Destroy(row);
        }
        // generate with current reputation in mind?
        int currentReputation = RunManager.Instance.CurrentRunData.CurrentReputation;
        // amount of viewers
        int amountOfViewers = Math.Max(5, currentReputation * 3);
        int amountOfRows = Mathf.CeilToInt((float)amountOfViewers / (float)viewersPerRow);
        // generate viewers
        int generatedViewers = 0;
        for (int r = 0; r < amountOfRows; r++)
        {
            // create row and set position
            GameObject row = new GameObject();
            row.transform.parent = transform;
            row.transform.localPosition = new Vector3(0, -r * rowDistance, 0);
            // get how much viewers will sit in this row
            int viewersThisRow = Math.Min(amountOfViewers - generatedViewers, viewersPerRow);
            // generate viewers of this row
            for (int vr = 0; vr < viewersThisRow; vr++)
            {
                GameObject viewer = Instantiate(ViewerPrefab);
                viewer.transform.parent = row.transform;
                float posX = vr * viewerDistance;
                posX -= viewerDistance * (viewersThisRow - 1) / 2;
                posX += viewersThisRow % 2 * (viewerDistance / 2);
                viewer.transform.localPosition = new Vector3(posX, 0, 0);
            }
            // add viewers of this row to the generated viewers
            generatedViewers += viewersThisRow;
            // save row
            rows.Add(row);
        }

    }
}
