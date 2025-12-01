using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationNode : MonoBehaviour, IInteractable
{
    public CardSY[] dropCards;
    private bool cardDroped = false;

    public bool autoDropOnEnter = true;

    public bool destroyAfter;

    public string scheduledItemID;

    public void Explore(PlayerInteraction player)
    {
        if (dropCards == null || dropCards.Length == 0 || cardDroped == true) return;

        foreach (var card in dropCards)
        {
            player.cardInventory.AddCard(card);
        }

        UI_CardPopup.Instance.ShowCards(dropCards);
        cardDroped = true;

        if (!string.IsNullOrEmpty(scheduledItemID))
            ItemSpawnManager.Instance.MarkItemPicked(scheduledItemID);

        if (destroyAfter)
        {
            Collider2D myCol = GetComponent<Collider2D>();
            myCol.enabled = false;
            gameObject.SetActive(false); 
        }
    }

    public void Interact(PlayerInteraction player)
    {
        if (!autoDropOnEnter)
        {
            Explore(player);
        }
    }
}
