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

}
