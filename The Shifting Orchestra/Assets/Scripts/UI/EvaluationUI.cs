using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EvaluationUI : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset performanceRequirementEvaluationAsset;
    private Label lbl_performance;
    private ListView liv_requirement_performance;
    private Label lbl_gained_money;
    private Label lbl_gained_reputation;
    private Button btn_next_gig;
    private Button btn_finish;


    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        // performance
        lbl_performance = root.Q<Label>("lbl_performance");
        // list requirement performance
        liv_requirement_performance = root.Q<ListView>("liv_requirement_performance");
        // gained money
        lbl_gained_money = root.Q<Label>("lbl_gained_money");
        // gained reputation
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
        // show requirement scores
        SetupRequirementList(liv_requirement_performance, performance.RequirementScores);
        // show performance
        lbl_performance.text = $"performance: {performance.OverallPerformance.PercentageValue}/100%";
        lbl_gained_money.text = $"gained money: {performance.MoneyReward}/{performance.PerformedGigOffer.MoneyReward}€";
        lbl_gained_reputation.text = $"gained reputation: {performance.RepuationReward}/{performance.PerformedGigOffer.ReputationReward}";
    }

    private void SetupRequirementList(ListView listView, List<GigRequirementResult> requirementScores)
    {
        // sort by importance
        requirementScores.Sort((left, right) => EnumValues.GigRequirementImportanceValues[right.Requirement.Importance].CompareTo(EnumValues.GigRequirementImportanceValues[left.Requirement.Importance]));
        // set list to itemsource for requirement list
        listView.itemsSource = null;
        listView.makeItem = () => {
            return new PerformanceRequirementEvaluation(performanceRequirementEvaluationAsset);
        };
        listView.bindItem = (element, index) =>
        {
            ((PerformanceRequirementEvaluation)element).ShowEvaluation(requirementScores[index]);
        };
        listView.itemsSource = requirementScores;
        listView.Rebuild();
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
