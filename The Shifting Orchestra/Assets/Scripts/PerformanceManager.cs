using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PerformanceManager
{
    public Performance EvaluatePerformance(OrchestraManager orchestraManager, GigOffer gigOffer)
    {
        Performance performance = new Performance(gigOffer)
        {
            OverallPerformance = new Percentage()
        };
        // start with 100% and deduct later
        performance.OverallPerformance.AddValue(100);
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
            performance.OverallPerformance.AddValue(-25);
        }
        // iterate through gig requirements
        Dictionary<GigRequirement, int> requirementScores = new Dictionary<GigRequirement, int>();
        foreach (GigRequirement requirement in gigOffer.Requirements)
        {
            // grace area 
            int graceArea = 5;
            int deduction = 0;
            switch (requirement.RequirementType)
            {
                case Enums.GigRequirementType.TEMPO:
                    if (requirement.PreferredTempo.HasValue)
                    {
                        int preferredTempo = requirement.PreferredTempo.Value;
                        // get value of the selected speed enum
                        int speedValue = EnumValues.AudioClipSpeedValues[orchestraManager.Tempo];
                        // check if tempo is below or above preferred tempo
                        // bool isInGraceArea = (speedValue - graceArea) < preferredTempo && preferredTempo < (speedValue + graceArea);
                        bool tempoIsInGraceArea = (preferredTempo - graceArea) <= speedValue && speedValue <= (preferredTempo + graceArea);
                        if (!tempoIsInGraceArea)
                        {
                            // subtract difference between tempos (account for grace area)
                            // TODO: check math
                            deduction = Math.Max(Math.Abs(preferredTempo - speedValue) - graceArea, 1);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"tempo requirement has no value ({requirement.PreferredTempo})");
                    }
                    break;
                case Enums.GigRequirementType.VOLUME:
                    if (requirement.PreferredVolume.HasValue)
                    {
                        float preferredVolume = requirement.PreferredVolume.Value;
                        // calculate volume average
                        float volumeAverage = 0;
                        foreach (InstrumentGroup group in orchestraManager.Orchestra)
                        {
                            volumeAverage += group.Volume;
                        }
                        volumeAverage /= orchestraManager.Orchestra.Count;
                        // check if volume is below or above preferred tempo
                        bool volumeIsInGraceArea = (preferredVolume - graceArea) <= volumeAverage && volumeAverage <= (preferredVolume + graceArea);
                        if (!volumeIsInGraceArea)
                        {
                            // subtract difference between volumes (account for grace area)
                            // TODO: check math
                            deduction = Mathf.FloorToInt(Mathf.Max(Mathf.Abs(preferredVolume - volumeAverage) - graceArea, 1));
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"volume requirement has no value ({requirement.PreferredTempo})");
                    }
                    break;
                case Enums.GigRequirementType.INSTRUMENTS:
                    Enums.InstrumentType instrumentType = requirement.PreferredInstrumentProminence.Item1;
                    int preferredProminence = requirement.PreferredInstrumentProminence.Item2;
                    // start with 0 prominence
                    int prominence = 0;
                    // get instrument group
                    InstrumentGroup instrumentGroup = OrchestraManager.Instance.GetInstrumentGroup(instrumentType);
                    //  -> if there is none prominence is 0
                    if (instrumentGroup != null)
                    {
                        // get volume factor (calculate factor where volume is between 0 and 1 from min volume to max volume)
                        float volumeFactor = (instrumentGroup.Volume - GameDatabase.Instance.VolumeMin) / (GameDatabase.Instance.VolumeMax - GameDatabase.Instance.VolumeMin);
                        // multiply musical role prominence value by volume 
                        prominence = Mathf.RoundToInt(EnumValues.MusicalRoleProminence[instrumentGroup.MusicalRole] * volumeFactor);
                    }
                    else
                    {
                        prominence = 0;
                    }
                    // check if prominence is below or above preferred prominence (with grace area)
                    bool prominenceIsInGraceArea = (preferredProminence - graceArea) <= prominence && prominence <= (preferredProminence + graceArea);
                    if (!prominenceIsInGraceArea)
                    {
                        // set deduction to difference between preferred and set prominence (subtract grace area)
                        deduction = Math.Max(Math.Abs(preferredProminence - prominence) - graceArea, 1);
                    }
                    break;
            }
            requirementScores.Add(requirement, deduction);
        }
        performance.RequirementScores = requirementScores;
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
            float realRequirementWeight = (float)requirementScore.Key.Weight / (float)totalWeight;
            // get weighed deduction
            int weighedDeduction = Mathf.FloorToInt((float)requirementScore.Value * realRequirementWeight);

            Debug.Log($"{requirementScore.Key.RequirementType} {requirementScore.Key.Weight} -> {requirementScore.Value} -> {weighedDeduction}");

            // deduct weighed deduction from performance
            performance.OverallPerformance.AddValue(-weighedDeduction);
        }
        // return performance score (as percentage)
        return performance;
    }

    public void UpdateReputationAndMoney(Performance performance, GigOffer gigOffer)
    {
        // seperate multiplier for money and reputation
        float moneyRewardMultiplier = 0;
        float performanceRewardMultiplier = 0;
        // switch between score percentage
        switch (performance.OverallPerformance.PercentageValue)
        {
            case < 25: // 0%-24%: disaster: very reduces rewards (money, significant reputation loss)
                moneyRewardMultiplier = 0.25f;
                performanceRewardMultiplier = -0.5f;
                break;
            case < 50: // 25%-49%: poor: reduces rewards (money, reputation loss)
                moneyRewardMultiplier = 0.5f;
                performanceRewardMultiplier = 0.2f;
                break;
            case < 75: // 50%-74%: mediocre: partial rewards (money)
                moneyRewardMultiplier = 0.8f;
                performanceRewardMultiplier = 0.6f;
                break;
            case < 95: // 75%-94%: good: full rewards (money and reputation)
                moneyRewardMultiplier = 1f;
                performanceRewardMultiplier = 1f;
                break;
            case < 100: // 95%-99%: very good: extra rewards (money and reputation)
                moneyRewardMultiplier = 1.1f;
                performanceRewardMultiplier = 1.1f;
                break;
            case 100: // 100%: best performance there ever was: extra rewards (money and reputation)
                moneyRewardMultiplier = 1.2f;
                performanceRewardMultiplier = 1.2f;
                break;
        }
        // add money
        RunManager.Instance.AddMoney(Mathf.RoundToInt(Math.Max(0, gigOffer.MoneyReward * moneyRewardMultiplier)));
        // add reputation
        RunManager.Instance.AddReputation(Mathf.RoundToInt(Math.Max(0, gigOffer.ReputationReward * performanceRewardMultiplier)));
    }
}
