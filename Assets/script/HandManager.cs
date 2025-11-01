using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{

    [Header("Prefabs & refs")]
    public GameObject cardUIPrefab;         
    public CardInventory cardInventory;     // reference to the player's inventory
    public Transform handTransform;     // parent RectTransform that holds the cards (under Canvas)
    public ReasoningBoardUI reasoningBoardUI;
    public CombineSystem CombineSystem;
    private bool isDragging = false;

    [Header("Layout")]
    public float fanSpread = 15f;           // max angle spread in degrees
    public float verticalSpacing = 10f;     
    public float spacing = 40f;             // horizontal spacing between cards (fallback)

    // cached instances for reuse
    private List<GameObject> instantiatedCards = new List<GameObject>();

    private List<CardUI> activeCardUIs = new List<CardUI>();//For Hightlight

    private void Awake()
    {
        if (cardInventory == null)
        {
            Debug.LogError("HandManager: cardInventory not assigned.");
            return;
        }
    }

    private void OnEnable()
    {
        // subscribe so hand updates when a new card is added
        cardInventory.OnCardAdded += OnCardAdded;
    }

    private void OnDisable()
    {
        cardInventory.OnCardAdded -= OnCardAdded;
    }

    private void Start()
    {
        RefreshHand();
    }

    private void OnCardAdded(CardSY newCard)
    {
        if (!isDragging)
            RefreshHand();
    }

    public void RefreshHand()
    {
        foreach (var go in instantiatedCards)
            Destroy(go);
        instantiatedCards.Clear();

        List<CardSY> cards = cardInventory.GetAllCards();
        int count = cards.Count;
        if (count == 0) return;

        for (int i = 0; i < count; i++)
        {
            CardSY cardData = cards[i];
            GameObject go = Instantiate(cardUIPrefab, handTransform);
            CardUI cardUI = go.GetComponent<CardUI>();
            if (cardUI != null)
                cardUI.Setup(cardData, reasoningBoardUI, this); // ✅ pass reference
                activeCardUIs.Add(cardUI);

            instantiatedCards.Add(go);
        }

        UpdateHandVisuals();

    }

    private void UpdateHandVisuals()
    {
        int cardCount = instantiatedCards.Count;
        if (cardCount == 0) return;
        if (cardCount == 1)
        {
            instantiatedCards[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            instantiatedCards[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }
        // central index (0 based)
        float mid = (cardCount - 1) / 2f;

        for (int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount-1) / 2f));
            instantiatedCards[i].transform.localRotation = Quaternion.Euler(0f,0f,rotationAngle);

            float horizontalOffset = (spacing * (i - (cardCount - 1) / 2f));
            float normalizedPosition = (2f * i / (cardCount - 1) - 1f); //Normalize card position between -1 and 1
            float verticalOffset = (verticalSpacing * (1 - normalizedPosition * normalizedPosition));


            instantiatedCards[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }

    public void HighlightCompatibleCards(CardSY draggedCard)
    {
        foreach (var ui in activeCardUIs)
        {
            if (ui == null || ui.cardData == null) continue;
            if (ui.cardData == draggedCard) continue;

            bool compatible = CombineSystem.CanCombine(draggedCard, ui.cardData);
            ui.Highlight(compatible);
        }
    }

    public void ClearHighlights()
    {
        foreach (var ui in activeCardUIs)
        {
            if (ui == null) continue;
            ui.Highlight(false);
        }
    }

    public void SetDragging(bool dragging)
    {
        isDragging = dragging;
    }
}
