using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotDropArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ReasoningBoardUI reasoningBoard;
    public string slotName; // e.g. "A" or "B"
    private Image slotImage;

    private void Awake()
    {
        slotImage = GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        CardUI cardUI = eventData.pointerDrag.GetComponent<CardUI>();
        if (cardUI != null)
        {
            reasoningBoard.OnCardDroppedIntoSlot(slotName, cardUI.cardData, slotImage);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotImage != null)
            slotImage.color = Color.yellow * 0.6f; // highlight
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (slotImage != null)
            slotImage.color = Color.white; // remove highlight
    }
}
