using System;
using System.Collections.Generic;
using UnityEngine;

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
            case Enums.GigRequirementType.INSTRUMENTS:
                // get random instrument type and musical role
                Enums.InstrumentType instrumentType = (Enums.InstrumentType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Enums.InstrumentType)).Length);
                int preferredProminence = UnityEngine.Random.Range(0, 101);
                // create a tuple of the instrument type and musical role as requirement
                PreferredInstrumentProminence = new KeyValuePair<Enums.InstrumentType, int>(instrumentType, preferredProminence);
                break;
            case Enums.GigRequirementType.TEMPO:
                // Random between 70 and 130
                PreferredTempo = UnityEngine.Random.Range(GameDatabase.Instance.GigPreferredTempoMin, GameDatabase.Instance.GigPreferredTempoMax);
                break;
            case Enums.GigRequirementType.VOLUME:
                // Random between 50 and 100
                PreferredVolume = UnityEngine.Random.Range(GameDatabase.Instance.GigPreferredVolumeMin, GameDatabase.Instance.GigPreferredVolumeMax);
                break;
            case Enums.GigRequirementType.ORCHESTRA_SIZE:
                // Random between 
                int reputationDifference = 5;
                MinOrchestraSize = Mathf.Max(1, UnityEngine.Random.Range(RunManager.Instance.CurrentReputation - reputationDifference, RunManager.Instance.CurrentReputation + reputationDifference));
                break;
        }
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