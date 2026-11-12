using System.Collections.Generic;
using UnityEngine;

public class GigOffer
{
    public List<GigRequirement> Requirements { get; private set; }
    public int MinimumOrchestraSize { get; private set; }
    public int MoneyReward { get; private set; }
    public int ReputationReward { get; private set; }
    
    public GigOffer()
    {
        // create list of requirements for the gig offer
        Requirements = new List<GigRequirement>();

        // 50% chance of even having a tempo/volume requirement -> only one of them can be requirement at a time, not both
        if (Random.value < 0.5f)
        {
            int weight = GetRandomWeight();
            // add tempo requirement
            if (Random.value < 0.5f)
            {
                Requirements.Add(new GigRequirement(Enums.GigRequirementType.TEMPO, weight));
            }
            // add volume requirement
            else
            {
                Requirements.Add(new GigRequirement(Enums.GigRequirementType.VOLUME, weight));
            }
        }
        // amount of instrument requirements
        int instrumentRequirementAmount = Random.Range(GameDatabase.Instance.GigMinInstrumentRequirementAmount, GameDatabase.Instance.GigMaxInstrumentRequirementAmount + 1);
        List<Enums.InstrumentType> usedInstrumentTypes = new List<Enums.InstrumentType>();
        for (int i = 0; i < instrumentRequirementAmount; i++)
        {
            int weight = GetRandomWeight();
            // create a new gig requirement and add it to the list of requirements
            GigRequirement requirement = new GigRequirement(Enums.GigRequirementType.INSTRUMENTS, weight);
            // check if the requirement already exists in the list of requirements and if so, generate a new requirement until a unique one is found
            int maxRetries = 0;
            while (usedInstrumentTypes.Contains(requirement.PreferredInstrumentProminence.Item1))
            {
                requirement = new GigRequirement(Enums.GigRequirementType.INSTRUMENTS, weight);
                maxRetries++;
                if (maxRetries >= 15)
                {
                    Debug.LogWarning("Max retries reached for generating unique gig requirement. Some requirements may be duplicates.");
                    break;
                }
            }
            // add the unique requirement to the list of requirements
            Requirements.Add(requirement);
            // add instrument to used instrument types
            usedInstrumentTypes.Add(requirement.PreferredInstrumentProminence.Item1);
        }
        // create money reward, reputation reward, and minimum orchestra size for the gig offer according to current reputation of the player
        int currentReputation = RunManager.Instance.CurrentReputation;
        // TODO: adjust values to be balanced
        int minOrchestraSize = Mathf.Max(1, Random.Range(currentReputation - 5, currentReputation + 5));
        MinimumOrchestraSize = Random.Range(minOrchestraSize, minOrchestraSize + Random.Range(0, 20));
        MoneyReward = Random.Range(10 * currentReputation, 20 * currentReputation + 1);
        ReputationReward = Random.Range(1 * currentReputation,  5 * currentReputation + 1);
    }

    private int GetRandomWeight()
    {
        return Random.Range(1, 11);
    }
}
