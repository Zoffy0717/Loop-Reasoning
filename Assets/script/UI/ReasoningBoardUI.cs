using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReasoningBoardUI : MonoBehaviour
{
    [Header("References")]
    public GameObject uiRoot; // The main reasoning board panel
    public HandManager handManager; 
    public CombineSystem synthesisSystem;
    public TestimonyCombineSystem testimonyCombineSystem;
    public CardInventory playerInventory;
    public CardDetailUI cardDetailUI;
    public GameObject testimonyBoard;
    public GameObject normalBoard;

    public Image slotAImage;
    public Image slotBImage;
    public TMP_Text slotAName;
    public TMP_Text slotBName;


    private CardSY slotACard;
    private CardSY slotBCard;
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Toggle(!isOpen);
        }
    }

    public void Toggle(bool show)
    {
        isOpen = show;
        uiRoot.SetActive(show);

        // Pause or resume game if needed
        if (show)
            GamePauseManager.Instance?.RequestPause("ReasoningBoard");
        else
            GamePauseManager.Instance?.RequestResume("ReasoningBoard");

        // Refresh cards when board opens
        if (show && handManager != null)
        {
            handManager.RefreshHand();
        }

        // Optionally reset slots or selections here
        if (!show)
        {
            ClearSlots();
        }
    }
    
    private void ClearSlots()
        {
            // Reset visuals in slot
            if (slotAImage != null)
                slotAImage.sprite = null;

            if (slotBImage != null)
                slotBImage.sprite = null;

        if (slotAName.text != null)
            slotAName.text = null;

        if (slotBName.text != null)
            slotBName.text = null;

        //if (slotAImage != null)
        //        slotAImage.color = new Color(1, 1, 1, 0.2f); // faint empty look
        //    if (slotBImage != null)
        //        slotBImage.color = new Color(1, 1, 1, 0.2f);

            // Reset data references
            slotACard = null;
            slotBCard = null;
    }

    private void ClearAllSlots(GameObject board)
    {
        if (board == null) return;

        SlotDropArea[] slots = board.GetComponentsInChildren<SlotDropArea>();

        foreach (var slot in slots)
        {
            if (slot.currentCard != null)
            {
                Destroy(slot.currentCard.gameObject);
                slot.currentCard = null;
            }
        }
    }

    public void ShowNormalBoard()
    {
        normalBoard.SetActive(true);
        testimonyBoard.SetActive(false);

        ClearAllSlots(normalBoard);
    }

    public void ShowTestimonyBoard()
    {
        normalBoard.SetActive(false);
        testimonyBoard.SetActive(true);

        ClearAllSlots(testimonyBoard);
    }


    public void OnCardDroppedIntoSlot(string slotName, CardSY card, Image targetImage)
    {
        if (slotName == "A")
        {
            slotACard = card;
            slotAImage.sprite = card.artwork;
            slotAImage.color = Color.white;
            slotAName.text = card.name;
        }
        else if (slotName == "B")
        {
            slotBCard = card;
            slotBImage.sprite = card.artwork;
            slotBImage.color = Color.white;
            slotBName.text = card.name;
        }

        if (slotACard != null && slotBCard != null)
        {
            var resultCard = synthesisSystem.TryCombine(slotACard, slotBCard);
            if (resultCard != null)
            {
                playerInventory.AddCard(resultCard);
                ClearSlots();
                if (handManager != null)
                    handManager.RefreshHand();
            }
        }

        // Testimony Board (3 slots)
        // -------------------------
        if (testimonyBoard.activeSelf)
        {
            SlotDropArea[] slots = testimonyBoard.GetComponentsInChildren<SlotDropArea>();

            List<CardSY> testimonyCards = new List<CardSY>();

            foreach (var s in slots)
            {
                if (s.currentCard != null)
                {
                    CardUI ui = s.currentCard.GetComponent<CardUI>();
                    if (ui != null)
                        testimonyCards.Add(ui.cardData);
                }
            }

            // must have exactly 3 cards before combining
            if (testimonyCards.Count == 3)
            {
                CardSY result = testimonyCombineSystem.TryCombineThree(
                    testimonyCards[0],
                    testimonyCards[1],
                    testimonyCards[2]
                );

                if (result != null)
                {
                    playerInventory.AddCard(result);
                    ClearAllSlots(testimonyBoard);

                    if (handManager != null)
                        handManager.RefreshHand();
                }
            }
        }
    }
}



