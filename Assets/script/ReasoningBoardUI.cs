using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReasoningBoardUI : MonoBehaviour
{
    public static ReasoningBoardUI Instance;
    public GameObject uiRoot;
    public Transform cardListContainer;
    public GameObject cardButtonPrefab;
    public CombineSystem combineSystem;
    public CardInventory playerInventory;

    public Image slotAImage;
    public Image slotBImage;

    private CardSY slotA;
    private CardSY slotB;

    private void Awake()
    {
        Instance = this;
        uiRoot.SetActive(false);
    }

    public void Toggle(bool show)
    {
        uiRoot.SetActive(show);
        if (show)
            RefreshCardList();
        else
            ClearSlots();
    }

    public void RefreshCardList()
    {
        // Clear existing UI
        foreach (Transform child in cardListContainer)
            Destroy(child.gameObject);

        List<CardSY> allCards = playerInventory.GetAllCards();

        foreach (CardSY card in allCards)
        {
            GameObject btnObj = Instantiate(cardButtonPrefab, cardListContainer);
            CardButtonUI btn = btnObj.GetComponent<CardButtonUI>();
            btn.Setup(card, this);
        }
    }

    public void SelectCard(CardSY selectedCard)
    {
        if (slotA == null)
        {
            slotA = selectedCard;
            slotAImage.sprite = selectedCard.artwork;
            slotAImage.color = Color.white;
        }
        else if (slotB == null)
        {
            slotB = selectedCard;
            slotBImage.sprite = selectedCard.artwork;
            slotBImage.color = Color.white;

            TrySynthesize();
        }
        else
        {
            // Both filled → reset
            ClearSlots();
            slotA = selectedCard;
            slotAImage.sprite = selectedCard.artwork;
            slotAImage.color = Color.white;
        }
    }

    private void TrySynthesize()
    {
        if (slotA == null || slotB == null)
            return;

        CardSY result = combineSystem.TryCombine(slotA, slotB);

        if (result != null)
        {
            // Add the resulting card to the player’s inventory
            playerInventory.AddCard(result);

            // Show popup UI
            UI_CardPopup.Instance.ShowCards(new[] { result });

            Debug.Log($"[Reasoning] Created new card: {result.displayName}");
        }
        else
        {
            Debug.Log("[Reasoning] Invalid combination!");
        }

        ClearSlots();
    }

    private void ClearSlots()
    {
        slotA = null;
        slotB = null;
        slotAImage.sprite = null;
        slotBImage.sprite = null;
        slotAImage.color = Color.gray;
        slotBImage.color = Color.gray;
    }
}
