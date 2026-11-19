using System;
using System.Collections.Generic;
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
        foreach (KeyValuePair<Enums.InstrumentType, InstrumentGroup> group in orchestraManager.InstrumentGroups)
        {
            foreach (Musician musician in group.Value.Musicians)
            {
                orchestraSize++;
            }
        }
        // iterate through gig requirements
        List<GigRequirementResult> requirementScores = new List<GigRequirementResult>();
        // Dictionary<GigRequirement, int> requirementScores = new Dictionary<GigRequirement, int>();
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
                        foreach (KeyValuePair<Enums.InstrumentType, InstrumentGroup> group in orchestraManager.InstrumentGroups)
                        {
                            volumeAverage += group.Value.Volume;
                        }
                        volumeAverage /= orchestraManager.InstrumentGroups.Count;
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
                    if (requirement.PreferredInstrumentProminence.HasValue)
                    {
                        Enums.InstrumentType instrumentType = (Enums.InstrumentType)requirement.PreferredInstrumentProminence?.Key;
                        int preferredProminence = (int)requirement.PreferredInstrumentProminence?.Value;
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
                    }
                    else
                    {
                        Debug.LogWarning($"instrument prominence requirement has no value ({requirement.PreferredTempo})");
                    }
                    break;
                case Enums.GigRequirementType.ORCHESTRA_SIZE:
                    if (requirement.MinOrchestraSize.HasValue)
                    {
                        // deduct 25 performance points if orchestra size is too small
                        if (orchestraSize < requirement.MinOrchestraSize)
                        {
                            deduction = 25;
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"minimum orchestra size requirement has no value ({requirement.MinOrchestraSize})");
                    }
                    break;
            }
            requirementScores.Add(new GigRequirementResult(requirement, deduction));
        }
        performance.RequirementScores = requirementScores;
        // weigh scores and deduct performance points
        foreach (GigRequirementResult requirementResult in requirementScores)
        {
            // continue to next requirement if no value is found
            if (!EnumValues.GigRequirementImportanceValues.ContainsKey(requirementResult.Requirement.Importance))
            {
                Debug.LogWarning($"No value for requirement importance {requirementResult.Requirement.Importance} found!");
                continue;
            }
            // get real requirement weight
            float requirementImportance = EnumValues.GigRequirementImportanceValues[requirementResult.Requirement.Importance];
            // get weighed deduction
            requirementResult.WeighedDeduction  = Mathf.FloorToInt((float)requirementResult.Deduction * requirementImportance);
            // deduct weighed deduction from performance
            performance.OverallPerformance.AddValue(-requirementResult.WeighedDeduction);
        }
        // get money and reputation reward
        Rewards rewards = UpdateReputationAndMoney(performance.OverallPerformance.PercentageValue, gigOffer);
        performance.MoneyReward = rewards.Money;
        performance.RepuationReward = rewards.Reputation;
        // return performance score (as percentage)
        return performance;
    }

    public Rewards UpdateReputationAndMoney(int performanceInPercent, GigOffer gigOffer)
    {
        // seperate multiplier for money and reputation
        float moneyRewardMultiplier = 0;
        float performanceRewardMultiplier = 0;
        // switch between score percentage
        switch (performanceInPercent)
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
        // // add money
        // RunManager.Instance.AddMoney(Mathf.RoundToInt(Math.Max(0, gigOffer.MoneyReward * moneyRewardMultiplier)));
        // // add reputation
        // RunManager.Instance.AddReputation(Mathf.RoundToInt(Math.Max(0, gigOffer.ReputationReward * performanceRewardMultiplier)));

        return new Rewards
        {
            Money = Mathf.RoundToInt(Math.Max(0, gigOffer.MoneyReward * moneyRewardMultiplier)),
            Reputation = Mathf.RoundToInt(Math.Max(0, gigOffer.ReputationReward * performanceRewardMultiplier)),
        };
    }

    public struct Rewards
    {
        public int Money;
        public int Reputation;
    }
}
