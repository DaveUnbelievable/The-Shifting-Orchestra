

using System.Collections.Generic;

public class Performance
{
    public Percentage OverallPerformance;
    public GigOffer PerformedGigOffer;
    // public Dictionary<GigRequirement, int> RequirementScoresOld;
    public List<GigRequirementResult> RequirementScores;
    // difference in orchestra size

    public Performance(GigOffer performedGigOffer)
    {
        PerformedGigOffer = performedGigOffer;
        // RequirementScoresOld = new Dictionary<GigRequirement, int>();
        RequirementScores = new List<GigRequirementResult>();
    }
}
