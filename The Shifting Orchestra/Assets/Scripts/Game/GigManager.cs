using System.Collections.Generic;
using UnityEngine;

public class GigManager
{

    public List<GigOffer> GetGigOffers()
    {
        List<GigOffer> gigOffers = new List<GigOffer>();
        for (int i = 0; i < GameDatabase.Instance.GigOfferAmount; i++)
        {
            GigOffer gigOffer = new GigOffer();
            gigOffers.Add(gigOffer);
        }
        return gigOffers;
    }

    public void StartGig(GigOffer gigOffer)
    {
        /*
        TODO:
        - with DOTween do a little camera movement, that the orchestra is fully visible and in the middle
        - show musician animations
        - play music
        - wait for music to finish, then got to evaluation
        */
        MusicManager.Instance.PlayComposition();
    }


}
