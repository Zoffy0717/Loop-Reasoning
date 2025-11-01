using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReasoningBoardUI : MonoBehaviour
{
    [Header("References")]
    public GameObject uiRoot; // The main reasoning board panel
    public HandManager handManager; 
    public CombineSystem synthesisSystem;
    public CardInventory playerInventory;
    public CardDetailUI cardDetailUI;

    public Image slotAImage;
    public Image slotBImage;

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
            GamePauseManager.Instance.RequestPause();
        else
            GamePauseManager.Instance.RequestResume();

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
            // Reset visuals (e.g., empty slot sprite or transparent)
            if (slotAImage != null)
                slotAImage.sprite = null;

            if (slotBImage != null)
                slotBImage.sprite = null;

            // Optional: reset alpha if you use transparency
            if (slotAImage != null)
                slotAImage.color = new Color(1, 1, 1, 0.2f); // faint empty look
            if (slotBImage != null)
                slotBImage.color = new Color(1, 1, 1, 0.2f);

            // Reset data references
            slotACard = null;
            slotBCard = null;
    }

    public void OnCardDroppedIntoSlot(string slotName, CardSY card, Image targetImage)
    {
        if (slotName == "A")
        {
            slotACard = card;
            slotAImage.sprite = card.artwork;
            slotAImage.color = Color.white;
        }
        else if (slotName == "B")
        {
            slotBCard = card;
            slotBImage.sprite = card.artwork;
            slotBImage.color = Color.white;
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
            
    }
}



