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

}
