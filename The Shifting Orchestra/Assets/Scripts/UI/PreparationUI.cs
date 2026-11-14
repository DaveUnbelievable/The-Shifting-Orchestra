using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PreparationUI : MonoBehaviour
{

    private DropdownField drp_key_change;
    private DropdownField drp_tempo_change;
    private DropdownField drp_instrument_group;
    private Slider sld_volume_change;
    private DropdownField drp_musical_role;
    private ListView liv_musicians;
    private Button btn_perform_gig;

    private InstrumentGroup currentSelectedInstrumentGroup;

    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
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
        // musicians
        liv_musicians = root.Q<ListView>("liv_musicians");
        // perform gig button
        btn_perform_gig = root.Q<Button>("btn_perform_gig");
        btn_perform_gig.RegisterCallback<ClickEvent>(PerformGig);
        // fill all dropdown fields
        FillDropdownFields();
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
        if (OrchestraManager.Instance.InstrumentGroupObjects.ContainsKey(instrumentType))
        {
            GameObject instrumentGroupObject = OrchestraManager.Instance.InstrumentGroupObjects[instrumentType];
            InstrumentGroup instrumentGroup = instrumentGroupObject.GetComponent<InstrumentGroup>();
            if (instrumentGroup != null)
            {
                currentSelectedInstrumentGroup = instrumentGroup;
                // volume
                sld_volume_change.value = instrumentGroup.Volume;
                // musical role
                drp_musical_role.index = (int)instrumentGroup.MusicalRole;
                // TODO: show musicians
            }
        }
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

    private void PerformGig(ClickEvent evt)
    {
        // set gig as current run state
        RunManager.Instance.CurrentRunState = Enums.RunState.GIG;
    }
}
