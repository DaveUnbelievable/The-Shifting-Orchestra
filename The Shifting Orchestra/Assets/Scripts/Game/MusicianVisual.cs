using System;
using System.Collections.Generic;
using UnityEngine;

public class MusicianVisual : MonoBehaviour
{
    public List<MusicianVisualPart> Parts;
    public List<MusicianVisualInstrument> Instruments;
    public List<GameObject> LeftHandTargets;
    public List<GameObject> RightHandTargets;

    private Musician musician;

    public void SetMusician(Musician musician)
    {
        this.musician = musician;
        // show correct parts for male or female
        foreach (MusicianVisualPart part in Parts)
        {
            if (part.Gender == musician.Gender)
            {
                part.Renderer.gameObject.SetActive(true);
            } else
            {
                part.Renderer.gameObject.SetActive(false);
            }
        }
        // show each part in correct color
        foreach (MusicianVisualPart part in Parts)
        {
            // only active parts
            if (part.Renderer.gameObject.activeSelf)
            {
                switch (part.Part)
                {
                    case Enums.MusicianPart.SKIN:
                        part.Renderer.color = musician.SkinColor;
                        break;
                    case Enums.MusicianPart.HAIR:
                        part.Renderer.color = musician.HairColor;
                        break;
                    case Enums.MusicianPart.CLOTHING:
                        part.Renderer.color = musician.ClothingColor;
                        break;
                    case Enums.MusicianPart.SHOE:
                        part.Renderer.color = musician.ShoeColor;
                        break;
                }
            }
        }
        // set correct instrument visible
        foreach (MusicianVisualInstrument instrument in Instruments)
        {
            // set the correct instrument active
            bool active = false;
            if (instrument.Type == musician.InstrumentType)
            {
                active = true;
                // set ik targets to hand position
                foreach (GameObject ikTargetLeft in LeftHandTargets)
                {
                    ikTargetLeft.transform.parent = instrument.LeftHandTarget.transform;
                    ikTargetLeft.transform.localPosition = Vector3.zero;
                }
                foreach (GameObject ikTargetRight in RightHandTargets)
                {
                    ikTargetRight.transform.parent = instrument.RightHandTarget.transform;
                    ikTargetRight.transform.localPosition = Vector3.zero;
                }
            }
            instrument.ParentObject.SetActive(active);
        }
        // start correct animation
        Animator animator = GetComponent<Animator>();
        switch (musician.InstrumentType)
        {
            case Enums.InstrumentType.STRINGS:
                animator.SetTrigger("violin");
                break;
            case Enums.InstrumentType.CHOIR:
                animator.SetTrigger("choir");
                break;
            case Enums.InstrumentType.PIANO:
                animator.SetTrigger("piano");
                break;
            case Enums.InstrumentType.MALLET_PERCUSSION:
                animator.SetTrigger("marimba");
                break;
            case Enums.InstrumentType.PERCUSSION:
                animator.SetTrigger("percussion");
                break;
            case Enums.InstrumentType.BRASS:
                animator.SetTrigger("trumpet");
                break;
            case Enums.InstrumentType.WOODWINDS:
                animator.SetTrigger("clarinet");
                break;
        }
    }

    public void SetAnimationSpeed(float value)
    {
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("animation_speed", value);
    }
}

[Serializable]
public struct MusicianVisualPart
{
    public Enums.MusicianPart Part;
    public Enums.MusicianGender Gender;
    public SpriteRenderer Renderer;
}