using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class OrchestraPreparationUI : MonoBehaviour
{

    private DropdownField drp_key_change;
    private DropdownField drp_tempo_change;
    private DropdownField drp_instrument_group;
    private Slider sld_volume_change;
    private DropdownField drp_musical_role;
    private ListView liv_musicians;

    private InstrumentGroup currentSelectedInstrumentGroup;

    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        // key
        drp_key_change = root.Q<DropdownField>("drp_key_change");
        SetupEnumDropdown<Enums.AudioClipKey>(
            drp_key_change,
            key => KeyChanged(key),
            key => EnumDisplayNames.AudioClipKeyDisplayNames[key]
        );
        // tempo
        drp_tempo_change = root.Q<DropdownField>("drp_tempo_change");
        SetupEnumDropdown<Enums.AudioClipSpeed>(
            drp_tempo_change,
            tempo => TempoChanged(tempo),
            tempo => EnumDisplayNames.AudioClipSpeedDisplayNames[tempo]
        );
        // instrument group
        drp_instrument_group = root.Q<DropdownField>("drp_instrument_group");
        SetupEnumDropdown<Enums.InstrumentType>(
            drp_instrument_group,
            instrument => InstrumentGroupChanged(instrument),
            instrument => EnumDisplayNames.InstrumentTypeDisplayNames[instrument]
        );
        // volume
        sld_volume_change = root.Q<Slider>("sld_volume_change");
        sld_volume_change.RegisterValueChangedCallback(evt =>
        {
            VolumeChanged(evt.newValue);
        });
        // musical role
        drp_musical_role = root.Q<DropdownField>("drp_musical_role");
        SetupEnumDropdown<Enums.MusicalRole>(
            drp_musical_role,
            role => MusicalRoleChanged(role),
            role => EnumDisplayNames.MusicalRoleDisplayNames[role]
        );
        // musicians
        liv_musicians = root.Q<ListView>("liv_musicians");
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
        currentSelectedInstrumentGroup.MusicalRole = role;
    }

    private void VolumeChanged(float volume)
    {
        currentSelectedInstrumentGroup.Volume = volume;
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
}
