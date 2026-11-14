

using System.Collections.Generic;

public class Performance
{
    public Percentage OverallPerformance;
    public GigOffer PerformedGigOffer;
    public Dictionary<GigRequirement, int> RequirementScores;
    // difference in orchestra size

    public Performance(GigOffer performedGigOffer)
    {
        PerformedGigOffer = performedGigOffer;
        RequirementScores = new Dictionary<GigRequirement, int>();
    }
}
