using UnityEngine;
using UnityEngine.UIElements;

public class EndResultUI : MonoBehaviour
{
    private Label lbl_final_money;
    private Label lbl_final_reputation;
    private Button btn_end_run;

    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // final money
        lbl_final_money = root.Q<Label>("lbl_final_money");
        // final reputation
        lbl_final_reputation = root.Q<Label>("lbl_final_reputation");
        // finish button
        btn_end_run = root.Q<Button>("btn_end_run");
        btn_end_run.RegisterCallback<ClickEvent>(EndRun);

        RunManager.Instance.StateChanged += (state) =>
        {
            // show end result if run state is END
            if (state == Enums.RunState.END)
            {
                ShowEndResult();
            }
        };
    }

    private void ShowEndResult()
    {
        lbl_final_money.text = $"Final money: {RunManager.Instance.CurrentRunData.CurrentMoney}€";
        lbl_final_reputation.text = $"Final reputation: {RunManager.Instance.CurrentRunData.CurrentReputation}€";
    }

    private void EndRun(ClickEvent evt)
    {
        RunManager.Instance.EndRun();
    }
}
