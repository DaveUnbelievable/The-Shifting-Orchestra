using System;
using System.Collections.Generic;
using UnityEngine;

public class GigOffer
{
    public List<GigRequirement> Requirements { get; private set; }
    public int MoneyReward { get; private set; }
    public int ReputationReward { get; private set; }

    public GigOffer()
    {
        // create list of requirements for the gig offer
        Requirements = new List<GigRequirement>
        {
            // add orchestra size requirement
            new GigRequirement(Enums.GigRequirementType.ORCHESTRA_SIZE, Enums.GigRequirementImportance.VERY_IMPORTANT)
        };

        // 50% chance of even having a tempo/volume requirement -> only one of them can be requirement at a time, not both
        if (UnityEngine.Random.value < 0.5f)
        {
            Enums.GigRequirementImportance importance = GetRandomImportance();
            // add tempo requirement
            if (UnityEngine.Random.value < 0.5f)
            {
                Requirements.Add(new GigRequirement(Enums.GigRequirementType.TEMPO, importance));
            }
            // add volume requirement
            else
            {
                Requirements.Add(new GigRequirement(Enums.GigRequirementType.VOLUME, importance));
            }
        }
        // amount of instrument requirements
        int instrumentRequirementAmount = UnityEngine.Random.Range(GameDatabase.Instance.GigMinInstrumentRequirementAmount, GameDatabase.Instance.GigMaxInstrumentRequirementAmount + 1);
        // list of unused instrument types
        List<Enums.InstrumentType> unusedInstrumentTypes = new List<Enums.InstrumentType>();
        foreach (Enums.InstrumentType instrumentType in Enum.GetValues(typeof(Enums.InstrumentType)))
        {
            unusedInstrumentTypes.Add(instrumentType);
        }
        instrumentRequirementAmount = Mathf.Min(instrumentRequirementAmount, unusedInstrumentTypes.Count);
        // create requirements
        for (int i = 0; i < instrumentRequirementAmount; i++)
        {
            Enums.GigRequirementImportance importance = GetRandomImportance();
            // create a new gig requirement and add it to the list of requirements
            GigRequirement requirement = new GigRequirement(unusedInstrumentTypes, importance);
            Enums.InstrumentType instrumentType = (Enums.InstrumentType)requirement.PreferredInstrumentProminence?.Key;
            // add the unique requirement to the list of requirements
            Requirements.Add(requirement);
            // remove instrument type from unused types
            unusedInstrumentTypes.Remove(instrumentType);
        }
        // sort requirements by importane
        Requirements.Sort((left, right) => EnumValues.GigRequirementImportanceValues[right.Importance].CompareTo(EnumValues.GigRequirementImportanceValues[left.Importance]));
        // create money reward, reputation reward, and minimum orchestra size for the gig offer according to current reputation of the player
        int currentReputation = RunManager.Instance.CurrentRunData.CurrentReputation;
        // TODO: adjust values to be balanced
        MoneyReward = UnityEngine.Random.Range(10 * currentReputation, 20 * currentReputation + 1);
        ReputationReward = UnityEngine.Random.Range(1 * currentReputation,  5 * currentReputation + 1);
    }

    private Enums.GigRequirementImportance GetRandomImportance()
    {
        float randomValue = Mathf.Round(UnityEngine.Random.Range(0f, 1f) * 10) / 10; // -> should be only values with one digit after the comma
        Enums.GigRequirementImportance importance = Enums.GigRequirementImportance.NICE_TO_HAVE;
        foreach (KeyValuePair<Enums.GigRequirementImportance, float> gigRequirementImportanceValue in EnumValues.GigRequirementImportanceValues)
        {
            if (randomValue >= gigRequirementImportanceValue.Value)
            {
                importance = gigRequirementImportanceValue.Key;
            }
        }
        return importance;
    }
}
