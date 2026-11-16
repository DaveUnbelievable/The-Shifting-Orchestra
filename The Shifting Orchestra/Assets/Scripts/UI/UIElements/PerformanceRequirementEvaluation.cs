using UnityEngine;
using UnityEngine.UIElements;

public class PerformanceRequirementEvaluation : VisualElement
{
    private Label lbl_requirement;
    private Label lbl_deduction;
    private Image img_checkmark;

    public PerformanceRequirementEvaluation(VisualTreeAsset template)
    {
        template.CloneTree(this);

        // requirement
        lbl_requirement = this.Q<Label>("lbl_requirement");
        // deduction
        lbl_deduction = this.Q<Label>("lbl_deduction");
        // image checkmark
        img_checkmark = this.Q<Image>("img_checkmark");
    }

    public void ShowEvaluation(GigRequirementResult gigRequirementResult)
    {
        lbl_requirement.text = gigRequirementResult.Requirement.RequirementType.ToString();

        lbl_deduction.style.display = DisplayStyle.None;
        img_checkmark.style.display = DisplayStyle.None;
        if (gigRequirementResult.Score == 0)
        {
            img_checkmark.style.display = DisplayStyle.Flex;
        }
        else
        {
            lbl_deduction.style.display = DisplayStyle.Flex;
            lbl_deduction.text = $"-{gigRequirementResult.Score}";
        }

    }
}
