using System;

public class GigRequirement
{
    public Tuple<Enums.InstrumentType, Enums.MusicalRole> RequiredInstruments { get; private set; }
    public int? PreferredTempo { get; private set; }
    public int? PreferredVolume { get; private set; }
    public int Weight;

    public GigRequirement(Enums.GigRequirementType requirementType, int weight)
    {
        Weight = weight;
        switch (requirementType) {
            case Enums.GigRequirementType.INSTRUMENTS:
                // get random instrument type and musical role
                Enums.InstrumentType instrumentType = (Enums.InstrumentType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Enums.InstrumentType)).Length);
                Enums.MusicalRole musicalRole = (Enums.MusicalRole)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Enums.MusicalRole)).Length);
                // create a tuple of the instrument type and musical role as requirement
                RequiredInstruments = new Tuple<Enums.InstrumentType, Enums.MusicalRole>(instrumentType, musicalRole);
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