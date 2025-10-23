using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInventory : MonoBehaviour
{
    public List<CardSY> collectedCards = new List<CardSY>();
    
    public delegate void CardAddedHandler(CardSY card);
    
    public event CardAddedHandler OnCardAdded;

    public void AddCard(CardSY newCard)
    {
        if (newCard == null)
        {
            return;
        }

        if (collectedCards.Contains(newCard)) {
            return;
        }

        collectedCards.Add(newCard);

        OnCardAdded?.Invoke(newCard);
    }

    public bool HasCard(CardSY card)
    {
        return collectedCards.Contains(card);
    }

    public List<CardSY> GetAllCards()
    {
        return collectedCards;
    }

}
