using System.Collections.Generic;
using UnityEngine;

public static class EnumDisplayNames
{
    public static Dictionary<Enums.AudioClipKey, string> AudioClipKeyDisplayNames = new Dictionary<Enums.AudioClipKey, string> {
        {Enums.AudioClipKey.A_MAJOR, "A Major"},
        {Enums.AudioClipKey.B_MAJOR, "B Major"},
        {Enums.AudioClipKey.C_MAJOR, "C Major"},
        {Enums.AudioClipKey.D_MAJOR, "D Major"},
        {Enums.AudioClipKey.E_MAJOR, "E Major"},
        {Enums.AudioClipKey.F_MAJOR, "F Major"},
        {Enums.AudioClipKey.G_MAJOR, "G Major"},
    };

    public static Dictionary<Enums.AudioClipSpeed, string> AudioClipSpeedDisplayNames = new Dictionary<Enums.AudioClipSpeed, string> {
        {Enums.AudioClipSpeed.SLOWEST, "slowest"},
        {Enums.AudioClipSpeed.SLOWER, "slow"},
        {Enums.AudioClipSpeed.NORMAL, "normal"},
        {Enums.AudioClipSpeed.FASTER, "fast"},
        {Enums.AudioClipSpeed.FASTEST, "fastest"},
    };

    public static Dictionary<Enums.InstrumentType, string> InstrumentTypeDisplayNames = new Dictionary<Enums.InstrumentType, string> {
        {Enums.InstrumentType.BRASS, "brass"},
        {Enums.InstrumentType.CHOIR, "choir"},
        {Enums.InstrumentType.MALLET_PERCUSSION, "mallet percussion"},
        {Enums.InstrumentType.PERCUSSION, "percussion"},
        {Enums.InstrumentType.PIANO, "piano"},
        {Enums.InstrumentType.PLUCKED_STRINGS, "plucked strings"},
        {Enums.InstrumentType.STRINGS, "strings"},
        {Enums.InstrumentType.WOODWINDS, "woodwinds"},
    };

    public static Dictionary<Enums.MusicalRole, string> MusicalRoleDisplayNames = new Dictionary<Enums.MusicalRole, string> {
        {Enums.MusicalRole.ACCENTS_FILLS, "accents and fills"},
        {Enums.MusicalRole.BASS, "bass"},
        {Enums.MusicalRole.COUNTER_MELODY, "counter melody"},
        {Enums.MusicalRole.HARMONY, "harmony"},
        {Enums.MusicalRole.MELODY, "melody"},
        {Enums.MusicalRole.RHYTHM, "rhythm"},
        {Enums.MusicalRole.SOLO, "solo"},
    };
}
