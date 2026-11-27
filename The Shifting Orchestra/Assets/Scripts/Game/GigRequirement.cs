using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GigRequirement
{
    public Enums.GigRequirementType RequirementType;
    public KeyValuePair<Enums.InstrumentType, int>? PreferredInstrumentProminence { get; private set; }
    public int? PreferredTempo { get; private set; }
    public float? PreferredVolume { get; private set; }
    public int? MinOrchestraSize {get; private set;}
    public Enums.GigRequirementImportance Importance;

    public GigRequirement(Enums.GigRequirementType requirementType, Enums.GigRequirementImportance importance)
    {
        RequirementType = requirementType;
        Importance = importance;
        switch (requirementType) {
            case Enums.GigRequirementType.TEMPO:
                // Random between 70 and 130
                PreferredTempo = Random.Range(GameDatabase.Instance.GigPreferredTempoMin, GameDatabase.Instance.GigPreferredTempoMax);
                break;
            case Enums.GigRequirementType.VOLUME:
                // Random between 50 and 100
                PreferredVolume = Random.Range(GameDatabase.Instance.GigPreferredVolumeMin, GameDatabase.Instance.GigPreferredVolumeMax);
                break;
            case Enums.GigRequirementType.ORCHESTRA_SIZE:
                int currentReputation = RunManager.Instance.CurrentRunData.CurrentReputation;
                // min orchestra size is 1/3 of reputation
                MinOrchestraSize = Mathf.RoundToInt((float)currentReputation / 3f);
                // random difference between minus 1/5 reputationa nd plus 1/5 reputation
                MinOrchestraSize += Mathf.RoundToInt(Random.Range(-(float)currentReputation / 5f, (float)currentReputation / 5f));
                break;
        }
    }

    public GigRequirement(List<Enums.InstrumentType> unusedInstrumentTypes, Enums.GigRequirementImportance importance)
    {
        RequirementType = Enums.GigRequirementType.INSTRUMENTS;
        Importance = importance;
        // get random instrument type and musical role
        Enums.InstrumentType instrumentType = unusedInstrumentTypes[Random.Range(0, unusedInstrumentTypes.Count)];
        int preferredProminence = Random.Range(0, 101);
        // create a tuple of the instrument type and musical role as requirement
        PreferredInstrumentProminence = new KeyValuePair<Enums.InstrumentType, int>(instrumentType, preferredProminence);
    }

    public string GetRequirementText()
    {
        string requirementText = "";
        switch (RequirementType) {
            case Enums.GigRequirementType.INSTRUMENTS:
                requirementText += Enums.GigRequirementType.INSTRUMENTS;
                requirementText += " - ";
                requirementText += PreferredInstrumentProminence?.Key;
                requirementText += ": ";
                requirementText += PreferredInstrumentProminence?.Value;
                break;
            case Enums.GigRequirementType.TEMPO:
                requirementText += $"Tempo: {PreferredTempo}";
                break;
            case Enums.GigRequirementType.VOLUME:
                requirementText += $"Volume: {PreferredVolume}";
                break;
            case Enums.GigRequirementType.ORCHESTRA_SIZE:
                requirementText += $"Orchestra Size: {MinOrchestraSize}";
                break;
        }
        requirementText += $"({Importance})";
        return requirementText;
    }
}