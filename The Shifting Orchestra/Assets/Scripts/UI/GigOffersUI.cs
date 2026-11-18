using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GigOffersUI : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset gigOfferAsset;

    private VisualElement vel_gig_offers;
    private Button btn_reroll_gig_offers;

    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        
        vel_gig_offers = root.Q<VisualElement>("vel_gig_offers");
        btn_reroll_gig_offers = root.Q<Button>("btn_reroll_gig_offers");
        btn_reroll_gig_offers.text = $"Reroll for {GameDatabase.Instance.GigOfferRerollBaseCost}€";
        btn_reroll_gig_offers.RegisterCallback<ClickEvent>(RerollGigOffers);
        
        RunManager.Instance.StateChanged += (state) =>
        {
            // show gig offers if run state is GIG_OFFERS
            if (state == Enums.RunState.GIG_OFFERS)
            {
                ShowGigOffers();
            }
        };
    }

    private void RerollGigOffers(ClickEvent evt)
    {
        if (RunManager.Instance.GetNewGigOffers(true))
        {
            ShowGigOffers();
        }
        // TODO: check if player has enough money to reroll, if not disable button
    }

    private void ShowGigOffers()
    {
        // get gig offers from run manager
        List<GigOffer> gigOffers = RunManager.Instance.GigOffers;
        // remove previous gig offers
        vel_gig_offers.Clear();
        // fill ui with gig offers
        foreach (GigOffer gigOffer in gigOffers)
        {
            GigOfferCard card = new GigOfferCard(gigOfferAsset);
            vel_gig_offers.Add(card);
            card.ShowGigOffer(gigOffer);
        }
    }
}
