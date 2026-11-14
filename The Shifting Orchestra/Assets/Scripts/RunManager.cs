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

                UIManager.Instance.ShowUI(Enums.UserInterface.GIG_OFFERS);

                List<GigOffer> gigOffers = _gigManager.GetGigOffers();
                Debug.Log("Gig Offer: " + gigOffers[0].MoneyReward + " money, " + gigOffers[0].ReputationReward + " reputation, " + gigOffers[0].MinimumOrchestraSize + " minimum orchestra size");
                foreach (GigRequirement requirement in gigOffers[0].Requirements) {
                    if (requirement.PreferredInstrumentProminence != null) {
                        Debug.Log("Requirement: " + requirement.PreferredInstrumentProminence.Item1 + " " + requirement.PreferredInstrumentProminence.Item2 + ", weight: " + requirement.Weight);
                    } else if (requirement.PreferredTempo != null) {
                        Debug.Log("Requirement: preferred tempo: " + requirement.PreferredTempo + ", weight: " + requirement.Weight);
                    } else if (requirement.PreferredVolume != null) {
                        Debug.Log("Requirement: preferred volume: " + requirement.PreferredVolume + ", weight: " + requirement.Weight);
                    }
                }
                // choose gig (temporarily just use the first)
                _chosenGigOffer = gigOffers[0];
                // change to preparation
                CurrentRunState = Enums.RunState.PREPARATION;
                break;
            case Enums.RunState.PREPARATION:

                UIManager.Instance.ShowUI(Enums.UserInterface.PREPARATION);

                break;
            case Enums.RunState.GIG:

                UIManager.Instance.ShowUI(Enums.UserInterface.GIG);

                _gigManager.StartGig(_chosenGigOffer);
                break;
            case Enums.RunState.EVALUATION:

                _performance = _performanceManager.EvaluatePerformance(OrchestraManager.Instance, _chosenGigOffer);
                _performanceManager.UpdateReputationAndMoney(_performance, _chosenGigOffer);
                
                UIManager.Instance.ShowUI(Enums.UserInterface.EVALUATION);

                // _performanceManager.EvaluatePerformance(_orchestraManager, _chosenGigOffer);
                // TODO: get performance score
                // TODO: get money and reputation reward based on performance score
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
                // TODO: hide ui
                break;
            case Enums.RunState.GIG:
                _currentGigIndex++;
                break;
            case Enums.RunState.EVALUATION:
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


    public void AddMoney(int addValue)
    {
        CurrentMoney = Math.Clamp(CurrentMoney + addValue, 0, int.MaxValue);
    }

    public void AddReputation(int addValue)
    {
        CurrentReputation = Math.Clamp(CurrentReputation + addValue, 0, int.MaxValue);
    }


    public Performance GetPerformance()
    {
        return _performance;
    }

}
