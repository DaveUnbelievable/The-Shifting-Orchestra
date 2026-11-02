using System.Collections.Generic;
using UnityEngine;

public class GameDatabase : MonoBehaviour
{
    #region Singleton
    public static GameDatabase Instance { get; private set; }
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    // base money the player starts with
    public int MoneyBase = 100;
    // starting orchestra
    public List<StartingInstrumentGroup> StartingInstrumentGroups = new List<StartingInstrumentGroup>() {
        new StartingInstrumentGroup { InstrumentType = Enums.InstrumentType.STRINGS, MusicalRole = Enums.MusicalRoles.MELODY, Amount = 2, Volume = 70 },
        new StartingInstrumentGroup { InstrumentType = Enums.InstrumentType.BRASS, MusicalRole = Enums.MusicalRoles.HARMONY, Amount = 1, Volume = 70 },
        new StartingInstrumentGroup { InstrumentType = Enums.InstrumentType.PIANO, MusicalRole = Enums.MusicalRoles.BASS, Amount = 1, Volume = 70 },
        new StartingInstrumentGroup { InstrumentType = Enums.InstrumentType.PERCUSSION, MusicalRole = Enums.MusicalRoles.RHYTHM, Amount = 1, Volume = 70 },
        new StartingInstrumentGroup { InstrumentType = Enums.InstrumentType.CHOIR, MusicalRole = Enums.MusicalRoles.ACCENTS_FILLS, Amount = 2, Volume = 70 },
    };
    // base and max reputation
    public int ReputationBase = 10;
    public int ReputationMax = 100;
    // amount of offers to generate for the player to choose from
    public int GigOfferAmount = 3;
    // minimum and maximum amount of requirements a gig can have
    public int GigMinInstrumentRequirementAmount = 1;
    public int GigMaxInstrumentRequirementAmount = 3;
    // minimum and maximum preferred tempo for a gig requirement
    public int GigPreferredTempoMin = 70;
    public int GigPreferredTempoMax = 130;
    // minimum and maximum preferred volume for a gig requirement
    public int GigPreferredVolumeMin = 50;
    public int GigPreferredVolumeMax = 100;
    // base volume
    public int VolumeBase = 70;
    // skin colors
    public List<Color> SkinColors = new List<Color>() {
        new Color(1f, 0.8f, 0.6f), // light
        new Color(0.8f, 0.6f, 0.4f), // medium
        new Color(0.6f, 0.4f, 0.2f), // dark
    };
    // clothing colors
    public List<Color> ClothingColors = new List<Color>() {
        new Color(0.8f, 0.2f, 0.2f), // red
        new Color(0.2f, 0.8f, 0.2f), // green
        new Color(0.2f, 0.2f, 0.8f), // blue
        new Color(0.8f, 0.8f, 0.2f), // yellow
        new Color(0.8f, 0.2f, 0.8f), // purple
    };
    // hair colors
    public List<Color> HairColors = new List<Color>() {
        new Color(0.2f, 0.2f, 0.2f), // black
        new Color(0.6f, 0.4f, 0.2f), // brown
        new Color(0.8f, 0.8f, 0.2f), // blonde
        new Color(1f, 1f, 1f), // white
    };
}