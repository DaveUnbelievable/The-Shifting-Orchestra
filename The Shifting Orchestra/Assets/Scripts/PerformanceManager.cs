using System;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceManager
{
    public Percentage EvaluatePerformance(OrchestraManager orchestraManager, GigOffer gigOffer)
    {
        Percentage performance = new Percentage();
        // start with 100% and deduct later
        performance.AddValue(100);
        // met size requirement
        int orchestraSize = 0;
        foreach (InstrumentGroup group in orchestraManager.Orchestra)
        {
            foreach (Musician musician in group.Musicians)
            {
                orchestraSize++;
            }
        }
        // deduct 25 performance points if orchestra size is too small
        if (orchestraSize < gigOffer.MinimumOrchestraSize)
        {
            performance.AddValue(-25);
        }
        // iterate through gig requirements
        Dictionary<GigRequirement, int> requirementScores = new Dictionary<GigRequirement, int>();
        foreach (GigRequirement requirement in gigOffer.Requirements)
        {
            // grace area 
            int graceArea = 5;
            int score = 0;
            switch (requirement.RequirementType)
            {
                case Enums.GigRequirementType.TEMPO:
                    // get value of the selected speed enum
                    int speedValue = EnumValues.AudioClipSpeedValues[orchestraManager.Tempo];
                    // check if tempo is below or above preferred tempo
                    if ((speedValue - graceArea) < (int)requirement.PreferredTempo && (int)requirement.PreferredTempo < (speedValue + graceArea))
                    {
                        // subtract difference between tempos (account for grace area)
                        // TODO: check math
                        score = -Math.Max(Math.Abs((int)requirement.PreferredTempo - speedValue) - graceArea, 1);
                    }
                    break;
                case Enums.GigRequirementType.VOLUME:
                    // calculate volume average
                    float volumeAverage = 0;
                    foreach (InstrumentGroup group in orchestraManager.Orchestra)
                    {
                        volumeAverage += group.Volume;
                    }
                    volumeAverage /= orchestraManager.Orchestra.Count;
                    // check if volume is below or above preferred tempo
                    if ((volumeAverage - graceArea) < (int)requirement.PreferredVolume && (int)requirement.PreferredVolume < (volumeAverage + graceArea))
                    {
                        // subtract difference between volumes (account for grace area)
                        // TODO: check math
                        score = -Mathf.FloorToInt(Mathf.Max(Mathf.Abs((float)requirement.PreferredVolume - volumeAverage) - graceArea, 1));
                    }
                    break;
                case Enums.GigRequirementType.INSTRUMENTS:
                    // get instrument group
                    //  -> if there is none prominence is 0
                    // get volume
                    // multiply musical role prominence value by volume 
                    // compare value to preferred prominence
                    // if in grace area -> no deduction
                    // else: subtract difference (minus grace area) in performance points from total
                    break;
            }
            requirementScores.Add(requirement, score);
        }
        // get total weight
        int totalWeight = 0;
        foreach (GigRequirement requirement in gigOffer.Requirements)
        {
            totalWeight += requirement.Weight;
        }
        if (totalWeight == 0)
        {
            Debug.LogWarning("totalWeight of requirements is 0!");
            return performance;
        }
        // weigh scores and deduct performance points
        foreach (KeyValuePair<GigRequirement, int> requirementScore in requirementScores)
        {
            // get real requirement weight
            float realRequirementWeight = requirementScore.Key.Weight / totalWeight;
            // get weighed score
            int weighedScore = Mathf.FloorToInt((float)requirementScore.Value * realRequirementWeight);
            // deduct weighed score from performance
            performance.AddValue(weighedScore);
        }
        // return performance score (as percentage)
        return performance;
    }

    public void UpdateReputationAndMoney(int performanceScore)
    {
        // TODO: update reputation and money based on performance score
    }
}
