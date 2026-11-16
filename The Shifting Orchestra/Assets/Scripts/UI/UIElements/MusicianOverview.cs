using UnityEngine;
using UnityEngine.UIElements;

public class MusicianOverview : VisualElement
{

    private Image img_musician;
    private Label lbl_name;
    private Label lbl_instrument;
    private Musician musician;

    public MusicianOverview(VisualTreeAsset template)
    {
        template.CloneTree(this);

        // image
        img_musician = this.Q<Image>("img_musician");
        // name
        lbl_name = this.Q<Label>("lbl_name");
        // instrument
        lbl_instrument = this.Q<Label>("lbl_instrument");
    }

    public void SetMusician(Musician musician)
    {
        // save musician
        this.musician = musician;
        // set name
        lbl_name.text = musician.Name;
        // set instrument
        lbl_instrument.text = musician.InstrumentType.ToString();
        // TODO: set image
    }
}
