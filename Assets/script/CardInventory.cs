using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CardInventory : MonoBehaviour
{
    public List<CardSY> collectedCards = new List<CardSY>();

    private List<CardSY> newlyAddedCards = new List<CardSY>();

    public delegate void CardAddedHandler(CardSY card);
    
    public event CardAddedHandler OnCardAdded;

    public GameObject boardHintUI;

    public TMP_Text endHint;

    private float introDuration = 10f;

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
        newlyAddedCards.Add(newCard);

        OnCardAdded?.Invoke(newCard);

        if (boardHintUI != null)
        {
            if (collectedCards.Count == 2)
            {
                boardHintUI.SetActive(true);
            }
            else
            {
                boardHintUI.SetActive(false);
            }
        }
    }

    public List<CardSY> GetCardsByType(CardType t)
    {
        List<CardSY> result = new List<CardSY>();
        foreach (var c in collectedCards)
        {
            if (c.type == t)
                result.Add(c);
        }
        return result;
    }

    public bool HasCard(string cardID)
    {
        foreach (var c in collectedCards)
        {
            if (c.cardID == cardID)
                return true;
        }
        return false;
    }

    public bool HasCard(CardSY card)
    {
        return collectedCards.Contains(card);
    }

    public List<CardSY> GetAllCards()
    {
        return collectedCards;
    }

    private IEnumerator EndGame()
    {
        endHint.text = "Tech Demo will end in 10 sec.";
        Time.timeScale = 1f; // just in case game was paused
        yield return new WaitForSeconds(introDuration);

        SceneManager.LoadScene(2);
    }


    public void RemoveCard(CardSY card)
    {
        collectedCards.Remove(card);
    }

    public List<CardSY> ConsumeNewCards()
    {
        List<CardSY> result = new List<CardSY>(newlyAddedCards);
        newlyAddedCards.Clear();
        return result;
    }

    public bool HasNewCards()
    {
        return newlyAddedCards.Count > 0;
    }

    public bool HasAnyCard(List<string> cardIDs)
    {
        foreach (var id in cardIDs)
        {
            if (HasCard(id))
                return true;
        }
        return false;
    }
}
