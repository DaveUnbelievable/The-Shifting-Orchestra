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

    public enum MusicalRole {
        MELODY,
        COUNTER_MELODY,
        HARMONY,
        BASS,
        ACCENTS_FILLS,
        RHYTHM,
        SOLO,
    }

    public enum AudioClipSpeed
    {
        SLOWEST,
        SLOWER,
        NORMAL,
        FASTER,
        FASTEST,
    }

    public enum AudioClipKey
    {
        G_MAJOR,
        A_MAJOR,
        B_MAJOR,
        C_MAJOR,
        D_MAJOR,
        E_MAJOR,
        F_MAJOR,
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
