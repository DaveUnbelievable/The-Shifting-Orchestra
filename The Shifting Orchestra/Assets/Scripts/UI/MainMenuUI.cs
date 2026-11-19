using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    
    private Label lbl_highscore_money;
    private Label lbl_highscore_reputation;
    private Button btn_start_run;
    private Button btn_end_game;

    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        // highscore money
        lbl_highscore_money = root.Q<Label>("lbl_highscore_money");
        // highscore reputation
        lbl_highscore_reputation = root.Q<Label>("lbl_highscore_reputation");
        // button start run
        btn_start_run = root.Q<Button>("btn_start_run");
        btn_start_run.RegisterCallback<ClickEvent>(StartRun);
        // button end game
        btn_end_game = root.Q<Button>("btn_end_game");
        btn_end_game.RegisterCallback<ClickEvent>(EndGame);

        RunManager.Instance.RunEnded += UpdateHighscore;

        UpdateHighscore();
    }

    private void UpdateHighscore()
    {
        // set standard
        int moneyHighscore = 0;
        int reputationHighscore = 0;
        // search for highscore
        if (PlayerPrefs.HasKey(GameDatabase.Instance.prefsMoneyHighscore) && PlayerPrefs.HasKey(GameDatabase.Instance.prefsReputationHighscore))
        {
            moneyHighscore = PlayerPrefs.GetInt(GameDatabase.Instance.prefsMoneyHighscore);
            reputationHighscore = PlayerPrefs.GetInt(GameDatabase.Instance.prefsReputationHighscore);
        }
        // show highscore
        lbl_highscore_money.text = $"Money: {moneyHighscore}€";
        lbl_highscore_reputation.text = $"Reputation: {reputationHighscore}";
    }

    private void StartRun(ClickEvent evt)
    {
        RunManager.Instance.StartRun();
    }

    private void EndGame(ClickEvent evt)
    {
        Application.Quit();
    }
}
