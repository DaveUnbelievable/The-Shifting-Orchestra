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
        A_MAJOR,
        B_MAJOR,
        C_MAJOR,
        D_MAJOR,
        E_MAJOR,
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
        END,
    }

    public enum GigRequirementType {
        INSTRUMENTS,
        TEMPO,
        VOLUME,
        ORCHESTRA_SIZE,
    }

    public enum GigRequirementImportance
    {
        NICE_TO_HAVE,
        NORMAL,
        IMPORTANT,
        VERY_IMPORTANT,
    }

    public enum OrchestraPositionType
    {
        NONE,
        FRONT_LEFT,
        MIDDLE_LEFT,
        BACK_LEFT,
        FRONT_RIGHT,
        MIDDLE_RIGHT,
        BACK_RIGHT,
        BACK_FULL,
    }

    public enum UserInterface
    {
        GIG,
        EVALUATION,
        GIG_OFFERS,
        PREPARATION,
        END,
    }

    public enum OrchestraUpgrade
    {
        SIZE, // more musicians possible
    }

    public enum MusicianGender
    {
        MALE,
        FEMALE,
    }

    public enum MusicianPart
    {
        SKIN,
        HAIR,
        CLOTHING,
        SHOE,
    }
}
