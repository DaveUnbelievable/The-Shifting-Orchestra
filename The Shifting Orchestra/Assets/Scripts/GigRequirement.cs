using System;
using UnityEngine.InputSystem.Android;

public class GigRequirement
{
    public Enums.GigRequirementType RequirementType;
    public Tuple<Enums.InstrumentType, int> PreferredInstrumentProminence { get; private set; }
    public int? PreferredTempo { get; private set; }
    public float? PreferredVolume { get; private set; }
    public int Weight;

    public GigRequirement(Enums.GigRequirementType requirementType, int weight)
    {
        RequirementType = requirementType;
        Weight = weight;
        switch (requirementType) {
            case Enums.GigRequirementType.INSTRUMENTS:
                // get random instrument type and musical role
                Enums.InstrumentType instrumentType = (Enums.InstrumentType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Enums.InstrumentType)).Length);
                int preferredProminence = UnityEngine.Random.Range(0, 101);
                // create a tuple of the instrument type and musical role as requirement
                PreferredInstrumentProminence = new Tuple<Enums.InstrumentType, int>(instrumentType, preferredProminence);
                break;
            case Enums.GigRequirementType.TEMPO:
                // Random between 70 and 130
                PreferredTempo = UnityEngine.Random.Range(GameDatabase.Instance.GigPreferredTempoMin, GameDatabase.Instance.GigPreferredTempoMax);
                break;
            case Enums.GigRequirementType.VOLUME:
                // Random between 50 and 100
                PreferredVolume = UnityEngine.Random.Range(GameDatabase.Instance.GigPreferredVolumeMin, GameDatabase.Instance.GigPreferredVolumeMax);
                break;
        }
    }
}