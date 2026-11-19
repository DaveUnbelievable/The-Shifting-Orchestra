using System;
using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    #region Singleton
    public static RunManager Instance { get; private set; }
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private GigManager _gigManager;
    private PerformanceManager _performanceManager;

    private Enums.RunState _currentRunState;
    public Enums.RunState CurrentRunState {
        get { return _currentRunState; }
        set {
            FinishRunState(_currentRunState);
            _currentRunState = value;
            BeginRunState(_currentRunState);
            // call event that the state has changed
            StateChanged?.Invoke(_currentRunState);
        }
    }
    
    public RunData CurrentRunData;

    [HideInInspector]
    public List<GigOffer> GigOffers {get; private set;}
    [HideInInspector]
    public GigOffer ChosenGigOffer {get; private set;}
    [HideInInspector]
    public Performance GigPerformance {get; private set;}
    [HideInInspector]
    public List<Musician> MusiciansForHire {get; private set;}
    [HideInInspector]
    public bool RunFinished;
    [HideInInspector]
    public bool RunWon;
    private int _currentGigIndex;


    public Action<Enums.RunState> StateChanged;
    public Action RunStarted;
    public Action RunEnded;

    public void StartRun() {
        // initialize
        _gigManager = new GigManager();
        _performanceManager = new PerformanceManager();
        // set base values
        _currentGigIndex = 0;
        RunFinished = false;
        // CurrentMoney = GameDatabase.Instance.MoneyBase;
        // CurrentReputation = GameDatabase.Instance.ReputationBase;

        CurrentRunData.CurrentMoney = GameDatabase.Instance.MoneyBase;
        CurrentRunData.CurrentReputation = GameDatabase.Instance.ReputationBase;

        OrchestraLayout.Instance.GenerateLayout();
        OrchestraManager.Instance.CreateStartingOrchestra();
        // set initial run state
        CurrentRunState = Enums.RunState.GIG_OFFERS;

        RunStarted?.Invoke();
    }

    /// <summary>
    /// begins the given run state
    /// </summary>
    /// <param name="newRunState">run state to begin</param>
    /// <summary>
    private void BeginRunState(Enums.RunState newRunState) {
        // actions to perform when beginning a run state
        switch (newRunState) {
            case Enums.RunState.GIG_OFFERS:

                GetNewGigOffers();

                break;
            case Enums.RunState.PREPARATION:

                // generate random musician per instrument
                GenerateMusiciansForHire();

                break;
            case Enums.RunState.GIG:

                _gigManager.StartGig(ChosenGigOffer);

                break;
            case Enums.RunState.EVALUATION:

                // evaluate performance
                GigPerformance = _performanceManager.EvaluatePerformance(OrchestraManager.Instance, ChosenGigOffer);

                // add rewards to players money and reputation
                AddMoney(GigPerformance.MoneyReward);
                AddReputation(GigPerformance.RepuationReward);

                // check if player lost
                if (CurrentRunData.CurrentReputation <= 0)
                {
                    FinishRun(false);
                }
                // check if the run is finished (if the player has played all gigs)
                if (_currentGigIndex >= GameDatabase.Instance.GigsPerRun)
                {
                    FinishRun(true);
                }

                break;
            case Enums.RunState.END:

                break;
        }
    }

    /// <summary>
    /// finishes the given run state
    /// </summary>
    /// <param name="finishedRunState">run state to finish</param>
    /// <summary>
    private void FinishRunState(Enums.RunState finishedRunState) {
        // actions to perform when finishing a run state
        switch (finishedRunState) {
            case Enums.RunState.GIG_OFFERS:

                break;
            case Enums.RunState.PREPARATION:
            
                break;
            case Enums.RunState.GIG:

                _currentGigIndex++;

                break;
            case Enums.RunState.EVALUATION:

                break;
            case Enums.RunState.END:

                break;
        }
    }

    private void FinishRun(bool won)
    {
        RunFinished = true;
        RunWon = won;
    }

    public void ChooseGigOffer(GigOffer gigOffer)
    {
        // save chosen gig offer
        ChosenGigOffer = gigOffer;
        // move to preparation
        CurrentRunState = Enums.RunState.PREPARATION;
    }

    public bool GetNewGigOffers(bool reroll = false)
    {
        if (reroll)
        {
            if (CurrentRunData.CurrentMoney > GameDatabase.Instance.GigOfferRerollBaseCost)
            {
                AddMoney(-GameDatabase.Instance.GigOfferRerollBaseCost);
            } else
            {
                return false;
            }
        }
        GigOffers = _gigManager.GetGigOffers();
        return true;
    }

    public void GenerateMusiciansForHire(Enums.InstrumentType? onlyThisInstrumentType = null)
    {
        // only generate a musician for this instrument type
        if (onlyThisInstrumentType != null)
        {
            MusiciansForHire.Add(MusicianGenerator.Instance.GenerateRandomMusician((Enums.InstrumentType)onlyThisInstrumentType));
        }
        else // generate for each instrument type
        {
            MusiciansForHire = new List<Musician>();
            foreach (Enums.InstrumentType instrumentType in Enum.GetValues(typeof(Enums.InstrumentType)))
            {
                MusiciansForHire.Add(MusicianGenerator.Instance.GenerateRandomMusician(instrumentType));
            }
        }
    }

    private void AddMoney(int addValue)
    {
        CurrentRunData.CurrentMoney = Math.Clamp(CurrentRunData.CurrentMoney + addValue, 0, int.MaxValue);
    }

    private void AddReputation(int addValue)
    {
        CurrentRunData.CurrentReputation = Math.Clamp(CurrentRunData.CurrentReputation + addValue, 0, int.MaxValue);
    }

    public void HireMusician(Musician musician)
    {
        // add to orchestra
        OrchestraManager.Instance.HireMusician(musician);
        // remove from hire list
        MusiciansForHire.Remove(musician);
        // add new musician with the same instrument type
        GenerateMusiciansForHire(musician.InstrumentType);
    }

    public void FireMusician(Musician musician)
    {
        // remove from orchestra
        OrchestraManager.Instance.FireMusician(musician);
    }

    public void EndRun()
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
        // compare
        if (CurrentRunData.CurrentMoney + CurrentRunData.CurrentReputation > moneyHighscore + reputationHighscore)
        {
            // save new highscore
            PlayerPrefs.SetInt(GameDatabase.Instance.prefsMoneyHighscore, CurrentRunData.CurrentMoney);
            PlayerPrefs.SetInt(GameDatabase.Instance.prefsReputationHighscore, CurrentRunData.CurrentReputation);
        }
        // really end the run and go back to the main menu
        RunEnded?.Invoke();
    }

}
