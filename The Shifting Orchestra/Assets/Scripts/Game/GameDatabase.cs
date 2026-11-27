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

    public string prefsMoneyHighscore = "money_highscore";
    public string prefsReputationHighscore = "reputation_highscore";

    // total amount of gigs the player will play in a run
    public int GigsPerRun = 7;
    // base money the player starts with
    public int MoneyBase = 100;
    // base cost of gig offer reroll
    public int GigOfferRerollBaseCost = 5;
    // base cost to hire a musician
    public int HireMusicianBaseCost = 70;
    // starting orchestra
    public List<StartingInstrumentGroup> StartingInstrumentGroups = new List<StartingInstrumentGroup>() {
        new StartingInstrumentGroup { InstrumentType = Enums.InstrumentType.STRINGS, MusicalRole = Enums.MusicalRole.MELODY, Amount = 2, Volume = 70 },
        new StartingInstrumentGroup { InstrumentType = Enums.InstrumentType.BRASS, MusicalRole = Enums.MusicalRole.HARMONY, Amount = 1, Volume = 70 },
        new StartingInstrumentGroup { InstrumentType = Enums.InstrumentType.PIANO, MusicalRole = Enums.MusicalRole.BASS, Amount = 1, Volume = 70 },
        new StartingInstrumentGroup { InstrumentType = Enums.InstrumentType.PERCUSSION, MusicalRole = Enums.MusicalRole.RHYTHM, Amount = 1, Volume = 70 },
        new StartingInstrumentGroup { InstrumentType = Enums.InstrumentType.CHOIR, MusicalRole = Enums.MusicalRole.ACCENTS_FILLS, Amount = 2, Volume = 70 },
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
    public float GigPreferredVolumeMin = 50;
    public float GigPreferredVolumeMax = 100;
    // base volume
    public float VolumeBase = 70;
    // max and min volume
    public float VolumeMin = 20;
    public float VolumeMax = 100;
    // skin colors
    public List<Color> SkinColors;
    // hair colors
    public List<Color> HairColors;
    // clothing colors
    public List<Color> ClothingColors;
    // shoe colors
    public List<Color> ShoeColors;
    // musician first and last names
    public List<string> MaleFirstNames;
    public List<string> FemaleFirstNames;
    public List<string> LastNames;
}