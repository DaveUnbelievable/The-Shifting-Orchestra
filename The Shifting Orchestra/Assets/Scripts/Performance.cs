

using System.Collections.Generic;

public class Performance
{
    public Percentage OverallPerformance;
    public GigOffer PerformedGigOffer;
    public List<GigRequirementResult> RequirementScores;
    public int MoneyReward;
    public int RepuationReward;

    public Performance(GigOffer performedGigOffer)
    {
        PerformedGigOffer = performedGigOffer;
        RequirementScores = new List<GigRequirementResult>();
    }
}
