using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotDropArea : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ReasoningBoardUI reasoningBoard;
    public string slotName; // e.g. "A" or "B"
    private Image slotImage;

    public GameObject currentCard;

    private Canvas canvas;

    private void Awake()
    {
        slotImage = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CardUI cardUI = eventData.pointerDrag.GetComponent<CardUI>();
        if (cardUI == null)
        {
            return;
        }

        if (currentCard != null)
        {
            Destroy (currentCard );
            currentCard = null;
        }

        currentCard = cardUI.gameObject;

        currentCard.transform.SetParent(transform);
        currentCard.transform.localPosition = Vector3.zero;

        CanvasGroup cg = currentCard.GetComponent<CanvasGroup>();
        if(cg != null )
        {
            cg.blocksRaycasts = true;
        }

        if (reasoningBoard != null)
        {
            reasoningBoard.OnCardDroppedIntoSlot(slotName,cardUI.cardData, slotImage);
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
