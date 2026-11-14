using System.Collections.Generic;
using UnityEngine;

public static class EnumValues
{
    public static Dictionary<Enums.AudioClipSpeed, int> AudioClipSpeedValues = new Dictionary<Enums.AudioClipSpeed, int> {
        {Enums.AudioClipSpeed.SLOWEST, 80},
        {Enums.AudioClipSpeed.SLOWER, 90},
        {Enums.AudioClipSpeed.NORMAL, 100},
        {Enums.AudioClipSpeed.FASTER, 110},
        {Enums.AudioClipSpeed.FASTEST, 120},
    };

    public static Dictionary<Enums.MusicalRole, float> MusicalRoleProminence = new Dictionary<Enums.MusicalRole, float> {
        {Enums.MusicalRole.MELODY, 100},
        {Enums.MusicalRole.COUNTER_MELODY, 90},
        {Enums.MusicalRole.ACCENTS_FILLS, 80},
        {Enums.MusicalRole.HARMONY, 60},
        {Enums.MusicalRole.BASS, 60},
        // {Enums.MusicalRole.SOLO, 100},
        {Enums.MusicalRole.RHYTHM, 70},
    };
}
