using System;
using System.Collections.Generic;
using UnityEngine;

public class MusicianVisual : MonoBehaviour
{
    public List<MusicianVisualPart> MusicianVisualParts;

    private Musician musician;

    public void SetMusician(Musician musician)
    {
        this.musician = musician;
        // show correct parts for male or female
        foreach (MusicianVisualPart part in MusicianVisualParts)
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
        foreach (MusicianVisualPart part in MusicianVisualParts)
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
    }
}

[Serializable]
public struct MusicianVisualPart
{
    public Enums.MusicianPart Part;
    public Enums.MusicianGender Gender;
    public SpriteRenderer Renderer;
}