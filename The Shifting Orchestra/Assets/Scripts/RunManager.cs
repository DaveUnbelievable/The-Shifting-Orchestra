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

    private Enums.RunState _currentRunState;
    public Enums.RunState CurrentRunState {
        get { return _currentRunState; }
        set {
            FinishRunState(_currentRunState);
            _currentRunState = value;
            BeginRunState(_currentRunState);
        }
    }
    
    public int CurrentMoney { get; private set; }
    public int CurrentReputation { get; private set; }

    // TEMP
    public void Start() {
        StartRun();
    }
    // TEMP

    public void StartRun() {
        // initialize
        _gigManager = new GigManager();
        // set base values
        CurrentMoney = GameDatabase.Instance.MoneyBase;
        CurrentReputation = GameDatabase.Instance.ReputationBase;
        // set initial run state
        CurrentRunState = Enums.RunState.GIG_OFFERS;
    }

    /// <summary>
    /// begins the given run state
    /// </summary>
    /// <param name="newRunState">run state to begin</param>
    /// <summary>
    private void BeginRunState(Enums.RunState newRunState) {
        switch (newRunState) {
            case Enums.RunState.GIG:
                break;
            case Enums.RunState.EVALUATION:
                break;
            case Enums.RunState.GIG_OFFERS:
                List<GigOffer> gigOffers = _gigManager.GetGigOffers();
                foreach (GigOffer gigOffer in gigOffers) {
                    Debug.Log("Gig Offer: " + gigOffer.MoneyReward + " money, " + gigOffer.ReputationReward + " reputation, " + gigOffer.MinimumOrchestraSize + " minimum orchestra size");
                    foreach (GigRequirement requirement in gigOffer.Requirements) {
                        if (requirement.RequiredInstruments != null) {
                            Debug.Log("Requirement: " + requirement.RequiredInstruments.Item1 + " " + requirement.RequiredInstruments.Item2 + ", weight: " + requirement.Weight);
                        } else if (requirement.PreferredTempo != null) {
                            Debug.Log("Requirement: preferred tempo: " + requirement.PreferredTempo + ", weight: " + requirement.Weight);
                        } else if (requirement.PreferredVolume != null) {
                            Debug.Log("Requirement: preferred volume: " + requirement.PreferredVolume + ", weight: " + requirement.Weight);
                        }
                    }
                }
                break;
            case Enums.RunState.PREPARATION:
                break;
        }
    }

    /// <summary>
    /// finishes the given run state
    /// </summary>
    /// <param name="finishedRunState">run state to finish</param>
    /// <summary>
    private void FinishRunState(Enums.RunState finishedRunState) {
        switch (finishedRunState) {
            case Enums.RunState.GIG:
                break;
            case Enums.RunState.EVALUATION:
                break;
            case Enums.RunState.GIG_OFFERS:
                break;
            case Enums.RunState.PREPARATION:
                break;
        }
    }



}
