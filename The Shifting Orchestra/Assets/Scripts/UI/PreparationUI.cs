using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PreparationUI : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset musicianOverviewAsset;
    [SerializeField]
    private VisualTreeAsset musicianCardAsset;
    [SerializeField]
    private VisualTreeAsset upgradeAsset;
    [SerializeField]
    private List<OrchestraUpgrade> possibleUpgrades;

    private Label lbl_gig_requirements;
    private Label lbl_gig_money_reward;
    private Label lbl_gig_reputation_reward;

    private DropdownField drp_key_change;
    private DropdownField drp_tempo_change;
    private DropdownField drp_instrument_group;
    private Slider sld_volume_change;
    private DropdownField drp_musical_role;
    private ListView liv_musicians;
    private Button btn_perform_gig;
    private VisualElement vel_musician_market;
    private VisualElement vel_upgrades;

    private InstrumentGroup currentSelectedInstrumentGroup;

    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        // gig requirements
        lbl_gig_requirements = root.Q<Label>("lbl_gig_requirements");
        // gig money reward
        lbl_gig_money_reward = root.Q<Label>("lbl_gig_money_reward");
        // gig reputation reward
        lbl_gig_reputation_reward = root.Q<Label>("lbl_gig_reputation_reward");
        // key
        drp_key_change = root.Q<DropdownField>("drp_key_change");
        // tempo
        drp_tempo_change = root.Q<DropdownField>("drp_tempo_change");
        // instrument group
        drp_instrument_group = root.Q<DropdownField>("drp_instrument_group");
        // volume
        sld_volume_change = root.Q<Slider>("sld_volume_change");
        sld_volume_change.RegisterValueChangedCallback(evt =>
        {
            VolumeChanged(evt.newValue);
        });
        // musical role
        drp_musical_role = root.Q<DropdownField>("drp_musical_role");
        // fill all dropdown fields
        FillDropdownFields();
        // musicians
        liv_musicians = root.Q<ListView>("liv_musicians");
        // musician market
        vel_musician_market = root.Q<VisualElement>("vel_musician_market");
        // upgrades
        vel_upgrades = root.Q<VisualElement>("vel_upgrades");
        // perform gig button
        btn_perform_gig = root.Q<Button>("btn_perform_gig");
        btn_perform_gig.RegisterCallback<ClickEvent>(PerformGig);
        
        RunManager.Instance.StateChanged += (state) =>
        {
            if (state == Enums.RunState.PREPARATION)
            {
                ShowMusiciansForHire();
                ShowChosenGig();
                ShowUpgrades();
            }
        };
    }
    
    private void FillDropdownFields()
    {
        // key
        SetupEnumDropdown<Enums.AudioClipKey>(
            drp_key_change,
            key => KeyChanged(key),
            key => EnumDisplayNames.AudioClipKeyDisplayNames[key]
        );
        // tempo
        SetupEnumDropdown<Enums.AudioClipSpeed>(
            drp_tempo_change,
            tempo => TempoChanged(tempo),
            tempo => EnumDisplayNames.AudioClipSpeedDisplayNames[tempo]
        );
        // instrument group
        SetupEnumDropdown<Enums.InstrumentType>(
            drp_instrument_group,
            instrument => InstrumentGroupChanged(instrument),
            instrument => EnumDisplayNames.InstrumentTypeDisplayNames[instrument]
        );
        // musical role
        SetupEnumDropdown<Enums.MusicalRole>(
            drp_musical_role,
            role => MusicalRoleChanged(role),
            role => EnumDisplayNames.MusicalRoleDisplayNames[role]
        );
    }

    private void KeyChanged(Enums.AudioClipKey key)
    {
        OrchestraManager.Instance.Key = key;
    }

    private void TempoChanged(Enums.AudioClipSpeed tempo)
    {
        OrchestraManager.Instance.Tempo = tempo;
    }

    private void InstrumentGroupChanged(Enums.InstrumentType instrumentType)
    {
        if (OrchestraManager.Instance.InstrumentGroups.ContainsKey(instrumentType))
        {
            InstrumentGroup instrumentGroup = OrchestraManager.Instance.InstrumentGroups[instrumentType];
            if (instrumentGroup != null)
            {
                currentSelectedInstrumentGroup = instrumentGroup;
                // volume
                sld_volume_change.value = instrumentGroup.Volume;
                // musical role
                drp_musical_role.index = (int)instrumentGroup.MusicalRole;
                // show musicians
                SetupMusicianList(liv_musicians, instrumentGroup.Musicians);
            }
        }
    }

    private void SetupMusicianList(ListView listView, List<Musician> musicians)
    {
        listView.itemsSource = null;
        listView.makeItem = () => {
            return new MusicianOverview(musicianOverviewAsset);
        };
        listView.bindItem = (element, index) =>
        {
            ((MusicianOverview)element).SetMusician(musicians[index]);
        };
        listView.itemsSource = musicians;
        listView.Rebuild();
    }

    private void MusicalRoleChanged(Enums.MusicalRole role)
    {
        OrchestraManager.Instance.ChangeMusicalRoleAssignment(currentSelectedInstrumentGroup, role);
    }

    private void VolumeChanged(float volume)
    {
        OrchestraManager.Instance.SetVolume(currentSelectedInstrumentGroup, volume);
    }

    private void SetupEnumDropdown<T>(DropdownField dropdown, Action<T> onSelected, Func<T, string> getDisplayName = null) where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>().ToList();

        dropdown.choices = values
            .Select(v => getDisplayName?.Invoke(v) ?? v.ToString())
            .ToList();

        dropdown.RegisterValueChangedCallback(evt =>
        {
            int index = dropdown.index;
            if (index >= 0 && index < values.Count)
            {
                onSelected(values[index]);
            }
        });
    }

    public void ShowMusiciansForHire()
    {
        // clear previous
        vel_musician_market.Clear();
        // show musicians for hire
        foreach (Musician musician in RunManager.Instance.MusiciansForHire)
        {
            MusicianCard card = new MusicianCard(musicianCardAsset, this);
            vel_musician_market.Add(card);
            card.ShowMusician(musician);
        }
    }

    public void ShowChosenGig()
    {
        GigOffer gigOffer = RunManager.Instance.ChosenGigOffer;
        // show requirements
        string allRequirements = "Requirements:\n";
        foreach (GigRequirement requirement in gigOffer.Requirements)
        {
            allRequirements += $"{requirement.GetRequirementText()}\n";
        }
        lbl_gig_requirements.text = allRequirements;
        // show money
        lbl_gig_money_reward.text = $"Money: {gigOffer.MoneyReward} €";
        // show reputation
        lbl_gig_reputation_reward.text = $"Reputation: {gigOffer.ReputationReward}";
    }

    private void PerformGig(ClickEvent evt)
    {
        // set gig as current run state
        RunManager.Instance.CurrentRunState = Enums.RunState.GIG;
    }

    private void ShowUpgrades()
    {
        // delete previous upgrades
        vel_upgrades.Clear();
        // create new upgrade cards
        foreach (OrchestraUpgrade upgrade in possibleUpgrades)
        {
            
            vel_upgrades.Add(CreateUpgradeCard(upgrade));
        }

        // TODO: Upgrade Button Callback, Upgrade Cost, Check if enough money, ...
    }

    private VisualElement CreateUpgradeCard(OrchestraUpgrade upgrade)
    {
        TemplateContainer cardInstance = upgradeAsset.Instantiate();
        cardInstance.dataSource = upgrade;

        Button btn_buy_upgrade = cardInstance.Q<Button>("btn_buy_upgrade");
        btn_buy_upgrade.clicked += () =>
        {
            RunManager.Instance.BuyUpgrade(upgrade);
        };

        return cardInstance;

    }
}
