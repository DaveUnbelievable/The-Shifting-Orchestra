public static class Enums
{
    public enum InstrumentType {
        STRINGS,
        WOODWINDS,
        BRASS,
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
        // SOLO,
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
        // G_MAJOR,
        A_MAJOR,
        B_MAJOR,
        C_MAJOR,
        D_MAJOR,
        E_MAJOR,
        // F_MAJOR,
    }

    public enum MusicPiece
    {
        A_MAJOR_DISASTER,
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

    public enum OrchestraPositionType
    {
        FRONT_CENTER,
        FRONT_LEFT,
        FRONT_RIGHT,
        BACK_CENTER,
        BACK_LEFT,
        BACK_RIGHT,
        BACK_FULL,
        // SOLO,
    }

    public enum UserInterface
    {
        GIG,
        EVALUATION,
        GIG_OFFERS,
        PREPARATION,
    }
}
