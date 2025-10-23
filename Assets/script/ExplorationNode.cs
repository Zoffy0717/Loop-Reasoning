using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationNode : MonoBehaviour, IInteractable
{
    public CardSY[] dropCards;

    public bool autoDropOnEnter = true;

    public void Explore(PlayerInteraction player)
    {
        if (dropCards == null || dropCards.Length == 0) return;

        foreach (var card in dropCards)
        {
            player.cardInventory.AddCard(card);
        }

        UI_CardPopup.Instance.ShowCards(dropCards);
        ///gameObject.SetActive(false); // optional: only explore once
    }

    public void Interact(PlayerInteraction player)
    {
        if (!autoDropOnEnter)
        {
            Explore(player);
        }
    }
}
