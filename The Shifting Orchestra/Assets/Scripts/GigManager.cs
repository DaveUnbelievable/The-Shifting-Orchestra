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
        // TODO: start gig
        // TODO: show animations for playing gig
        // TODO: play music for gig
        // TODO: after music is finished, go to evaluation state

        MusicManager.Instance.PlayComposition();
    }


}
