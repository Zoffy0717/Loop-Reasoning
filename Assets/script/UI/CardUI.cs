using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class CardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("References")]
    public Image artworkImage;
    public TMP_Text nameText;
    public TMP_Text description;
    private string cardIDs;

    [HideInInspector] public CardSY cardData;
    [HideInInspector] public ReasoningBoardUI reasoningBoard;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector2 originalPosition;

    public void Setup(CardSY card, ReasoningBoardUI board)
    {
        cardData = card;
        reasoningBoard = board;

        if (artworkImage != null) artworkImage.sprite = card.artwork;
        if (nameText != null) nameText.text = card.displayName;
        if (description != null) description.text = card.description;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        // Move to top of hierarchy so it’s not hidden behind other cards
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint);

        rectTransform.localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Re-enable raycast blocking
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // If the card was not dropped onto a valid SlotDropArea, return it to hand
        if (transform.parent == canvas.transform)
        {
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
        }
    }
}