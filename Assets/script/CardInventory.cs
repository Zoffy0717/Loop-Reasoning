using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardInventory : MonoBehaviour
{
    public List<CardSY> collectedCards = new List<CardSY>();
    
    public delegate void CardAddedHandler(CardSY card);
    
    public event CardAddedHandler OnCardAdded;

    public GameObject boardHintUI;

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
        
        if (collectedCards.Count == 2)
        {
            boardHintUI.SetActive(true);
        }
        else{
            boardHintUI.SetActive(false);
        }
        if (collectedCards.Count >= 7)
        {
            Debug.Log("Player reached 7 cards! Switching to End Scene...");
            Time.timeScale = 1f; // just in case game was paused
            SceneManager.LoadScene(2); // <-- make sure the name matches your scene name in Build Settings
        }
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
