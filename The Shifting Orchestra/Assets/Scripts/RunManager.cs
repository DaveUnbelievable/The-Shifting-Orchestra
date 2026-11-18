using System;
using System.Collections.Generic;
using NaughtyAttributes;
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
    
    public int CurrentMoney { get; private set; }
    public int CurrentReputation { get; private set; }

    private int _currentGigIndex;
    private GigOffer _chosenGigOffer;
    private Performance _performance;
    private bool _runFinished;


    public Action<Enums.RunState> StateChanged;


    [Button("Start Run")]
    public void StartRun() {
        // initialize
        _gigManager = new GigManager();
        // _orchestraManager = new OrchestraManager();
        _performanceManager = new PerformanceManager();
        // set base values
        _currentGigIndex = 0;
        _runFinished = false;
        CurrentMoney = GameDatabase.Instance.MoneyBase;
        CurrentReputation = GameDatabase.Instance.ReputationBase;
        // _orchestraManager.CreateStartingOrchestra();
        OrchestraLayout.Instance.GenerateLayout();
        OrchestraManager.Instance.CreateStartingOrchestra();
        // set initial run state
        CurrentRunState = Enums.RunState.GIG_OFFERS;
        // temporarily set directly to preparation
        CurrentRunState = Enums.RunState.PREPARATION;
    }

    /// <summary>
    /// begins the given run state
    /// </summary>
    /// <param name="newRunState">run state to begin</param>
    /// <summary>
    private void BeginRunState(Enums.RunState newRunState) {
        // return if run is finished
        if (_runFinished)
        {
            return;
        }
        // actions to perform when beginning a run state
        switch (newRunState) {
            case Enums.RunState.GIG_OFFERS:

                List<GigOffer> gigOffers = _gigManager.GetGigOffers();
                Debug.Log($"Gig Offer: {gigOffers[0].MoneyReward} money, {gigOffers[0].ReputationReward} reputation");
                foreach (GigRequirement requirement in gigOffers[0].Requirements) {
                    Debug.Log(requirement.GetRequirementText());
                }
                // choose gig (temporarily just use the first)
                _chosenGigOffer = gigOffers[0];

                break;
            case Enums.RunState.PREPARATION:

                break;
            case Enums.RunState.GIG:

                _gigManager.StartGig(_chosenGigOffer);

                break;
            case Enums.RunState.EVALUATION:

                // evaluate performance
                _performance = _performanceManager.EvaluatePerformance(OrchestraManager.Instance, _chosenGigOffer);

                break;
        }
    }

    /// <summary>
    /// finishes the given run state
    /// </summary>
    /// <param name="finishedRunState">run state to finish</param>
    /// <summary>
    private void FinishRunState(Enums.RunState finishedRunState) {
        // return if run is finished
        if (_runFinished)
        {
            return;
        }
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

                // add rewards to players money and reputation
                AddMoney(_performance.MoneyReward);
                AddReputation(_performance.RepuationReward);

                // check if player lost
                if (CurrentReputation <= 0)
                {
                    EndRun(false);
                }
                // check if the run is finished (if the player has played all gigs)
                if (_currentGigIndex >= GameDatabase.Instance.TotalAmountGigs)
                {
                    EndRun(true);
                }

                break;
        }
    }

    private void EndRun(bool won)
    {
        _runFinished = true;
        Debug.Log("Run finished! Final money: " + CurrentMoney + ", final reputation: " + CurrentReputation + ", won: " + won);
    }


    private void AddMoney(int addValue)
    {
        CurrentMoney = Math.Clamp(CurrentMoney + addValue, 0, int.MaxValue);
    }

    private void AddReputation(int addValue)
    {
        CurrentReputation = Math.Clamp(CurrentReputation + addValue, 0, int.MaxValue);
    }


    public Performance GetPerformance()
    {
        return _performance;
    }

}
