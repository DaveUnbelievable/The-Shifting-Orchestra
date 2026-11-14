using UnityEngine;
using UnityEngine.UIElements;

public class EvaluationUI : MonoBehaviour
{
    private Label lbl_performance;
    private Label lbl_gained_money;
    private Label lbl_gained_reputation;
    private Button btn_next_gig;
    private Button btn_finish;


    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        // performance
        lbl_performance = root.Q<Label>("lbl_performance");
        // performance
        lbl_gained_money = root.Q<Label>("lbl_gained_money");
        // performance
        lbl_gained_reputation = root.Q<Label>("lbl_gained_reputation");
        // next gig -> hide if run lost
        btn_next_gig = root.Q<Button>("btn_next_gig");
        btn_next_gig.RegisterCallback<ClickEvent>(NextGig);
        // finish -> only show when run lost
        btn_finish = root.Q<Button>("btn_finish");
        btn_finish.RegisterCallback<ClickEvent>(Finish);

        RunManager.Instance.StateChanged += RunStateChanged;

    }

    private void RunStateChanged(Enums.RunState state)
    {
        // show evaluation if run state is EVALUATION
        if (state == Enums.RunState.EVALUATION)
        {
            ShowEvaluation();
        }
    }

    private void ShowEvaluation()
    {
        // get performance
        Performance performance = RunManager.Instance.GetPerformance();
        // show performance
        lbl_performance.text = $"performance: {performance.OverallPerformance.PercentageValue}/100%";
        lbl_gained_money.text = $"gained money: {performance.PerformedGigOffer.MoneyReward * performance.OverallPerformance.PercentageValue / 100}/{performance.PerformedGigOffer.MoneyReward}€";
        lbl_gained_reputation.text = $"gained reputation: {performance.PerformedGigOffer.ReputationReward * performance.OverallPerformance.PercentageValue / 100}/{performance.PerformedGigOffer.ReputationReward}€";
    }

    private void NextGig(ClickEvent evt)
    {
        // continue to next gig
        RunManager.Instance.CurrentRunState = Enums.RunState.GIG_OFFERS;
    }

    private void Finish(ClickEvent evt)
    {
        // back to main menu
    }
}
