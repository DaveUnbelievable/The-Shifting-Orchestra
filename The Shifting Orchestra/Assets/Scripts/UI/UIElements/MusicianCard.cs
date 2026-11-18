using UnityEngine;
using UnityEngine.UIElements;

public class MusicianCard : VisualElement
{
    private Image img_musician;
    private Label lbl_name;
    private Label lbl_instrument;
    private Button btn_hire;
    private Musician musician;
    private PreparationUI parentUI;

    public MusicianCard(VisualTreeAsset template, PreparationUI parentUI)
    {
        this.parentUI = parentUI;

        template.CloneTree(this);

        // image
        img_musician = this.Q<Image>("img_musician");
        // name
        lbl_name = this.Q<Label>("lbl_name");
        // instrument
        lbl_instrument = this.Q<Label>("lbl_instrument");
        // hire button
        btn_hire = this.Q<Button>("btn_hire");
        btn_hire.RegisterCallback<ClickEvent>(HireMusician);
    }

    public void ShowMusician(Musician musician)
    {
        this.musician = musician;

        // set name
        lbl_name.text = musician.Name;
        // set instrument
        lbl_instrument.text = musician.InstrumentType.ToString();
        // TODO: set image
    }

    private void HireMusician(ClickEvent evt)
    {
        RunManager.Instance.HireMusician(musician);
        parentUI.ShowMusiciansForHire();
    }
}
