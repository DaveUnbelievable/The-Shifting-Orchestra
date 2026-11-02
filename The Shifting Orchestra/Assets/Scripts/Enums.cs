public static class Enums
{
    public enum InstrumentType {
        STRINGS,
        WOODWINDS,
        BRASS,
        PLUCKED_STRINGS,
        PIANO,
        CHOIR,
        MALLET_PERCUSSION,
        PERCUSSION,
    }

    public enum MusicalRoles {
        MELODY,
        COUNTER_MELODY,
        HARMONY,
        BASS,
        ACCENTS_FILLS,
        RHYTHM,
        SOLO,
    }

    public enum RunState {
        GIG,
        EVALUATION,
        GIG_OFFERS,
        PREPARATION,
    }

    public enum GigRequirementType {
        INSTRUMENTS,
        TEMPO,
        VOLUME,
    }

}
