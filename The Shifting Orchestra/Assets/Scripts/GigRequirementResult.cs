public class GigRequirementResult
{
    public GigRequirement Requirement;
    public int Deduction;
    public int WeighedDeduction;

    public GigRequirementResult(GigRequirement requirement, int score)
    {
        Requirement = requirement;
        Deduction = score;
    }
}
