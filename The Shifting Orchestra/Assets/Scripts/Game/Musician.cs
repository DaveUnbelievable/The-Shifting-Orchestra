using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Musician
{
    public string Name;
    public Enums.InstrumentType InstrumentType;
    public Enums.MusicianGender Gender;
    public Color SkinColor;
    public Color HairColor;
    public Color ClothingColor;
    public Color ShoeColor;

    public Musician(Enums.InstrumentType instrumentType, Enums.MusicianGender gender)
    {
        // set instrument type and gender
        InstrumentType = instrumentType;
        Gender = gender;
        // get random colors for skin, hair, clothing and shoes
        SkinColor = GameDatabase.Instance.SkinColors[Random.Range(0, GameDatabase.Instance.SkinColors.Count)];
        HairColor = GameDatabase.Instance.HairColors[Random.Range(0, GameDatabase.Instance.HairColors.Count)];
        ClothingColor = GameDatabase.Instance.ClothingColors[Random.Range(0, GameDatabase.Instance.ClothingColors.Count)];
        ShoeColor = GameDatabase.Instance.ShoeColors[Random.Range(0, GameDatabase.Instance.ShoeColors.Count)];
        // TODO: random Body Parts -> Generator
    }

    private string GenerateRandomName(Enums.MusicianGender gender)
    {
        // random last name
        string lastname = GameDatabase.Instance.LastNames[Random.Range(0, GameDatabase.Instance.LastNames.Count)];
        // random first name
        if (gender == Enums.MusicianGender.MALE)
        {
            // male
            string firstname = GameDatabase.Instance.MaleFirstNames[Random.Range(0, GameDatabase.Instance.MaleFirstNames.Count)];
            return firstname + " " + lastname;
        } else
        {
            // female
            string firstname = GameDatabase.Instance.FemaleFirstNames[Random.Range(0, GameDatabase.Instance.FemaleFirstNames.Count)];
            return firstname + " " + lastname;
        }
    }
}
