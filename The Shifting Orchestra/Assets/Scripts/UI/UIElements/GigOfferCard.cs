using UnityEngine;
using UnityEngine.UIElements;

public class GigOfferCard : VisualElement
{
    private Label lbl_gig_offer;
    private Label lbl_requirements;
    private Label lbl_money_reward;
    private Label lbl_reputation_reward;
    private Button btn_accept;
    private GigOffer gigOffer;

    public GigOfferCard(VisualTreeAsset template)
    {
        template.CloneTree(this);

        // gig offer
        lbl_gig_offer = this.Q<Label>("lbl_gig_offer");
        // requirements
        lbl_requirements = this.Q<Label>("lbl_requirements");
        // money
        lbl_money_reward = this.Q<Label>("lbl_money_reward");
        // reputation
        lbl_reputation_reward = this.Q<Label>("lbl_reputation_reward");
        // accept button
        btn_accept = this.Q<Button>("btn_accept");
        btn_accept.RegisterCallback<ClickEvent>(AcceptGig);
    }

    public void ShowGigOffer(GigOffer gigOffer)
    {
        this.gigOffer = gigOffer;
        // show requirements
        string allRequirements = "Requirements:\n";
        foreach (GigRequirement requirement in gigOffer.Requirements)
        {
            allRequirements += $"{requirement.GetRequirementText()}\n";
        }
        lbl_requirements.text = allRequirements;
        // show money
        lbl_money_reward.text = $"Money: {gigOffer.MoneyReward} €";
        // show reputation
        lbl_reputation_reward.text = $"Reputation: {gigOffer.ReputationReward}";
    }

    private void AcceptGig(ClickEvent evt)
    {
        RunManager.Instance.ChooseGigOffer(gigOffer);
    }
}
