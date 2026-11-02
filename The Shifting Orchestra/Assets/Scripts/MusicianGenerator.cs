using UnityEngine;

public static class MusicianGenerator
{
    public static Musician GenerateRandomMusician(Enums.InstrumentType instrumentType)
    {
        Musician musician = new Musician
        {
            InstrumentType = instrumentType,
            Name = GenerateRandomName(),
        };
        return musician;
    }

    private static string GenerateRandomName()
    {
        // temp random generation TODO: implement proper name generation
        string[] names = { "Alice", "Bob", "Charlie" };
        int randomIndex = Random.Range(0, names.Length);
        string firstname = names[randomIndex];
        string[] lastnames = { "Smith", "Johnson", "Williams" };
        int randomIndex2 = Random.Range(0, lastnames.Length);
        string lastname = lastnames[randomIndex2];
        return firstname + " " + lastname;
    }
}
