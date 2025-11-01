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
    private HandManager handManager;

    public void Setup(CardSY card, ReasoningBoardUI board, HandManager manager)
    {
        cardData = card;
        reasoningBoard = board;
        handManager = manager;

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
        if (this == null || cardData == null) return;

        handManager?.SetDragging(true);

        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        if (canvas == null) return;

        transform.SetParent(canvas.transform);
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.8f;
        }

        if (handManager != null)
            handManager.HighlightCompatibleCards(cardData);

        if (reasoningBoard != null && reasoningBoard.cardDetailUI != null)
        {
            reasoningBoard.cardDetailUI.ShowDetails(cardData);
        }
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
        if (this == null) return;

        handManager?.SetDragging(false);

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }

        if (handManager != null)
            handManager.ClearHighlights();

        // If the card was not dropped onto a valid SlotDropArea, return it to hand
        if (transform != null && transform.parent == canvas.transform)
        {
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
        }
        if (reasoningBoard != null && reasoningBoard.cardDetailUI != null)
        {
            reasoningBoard.cardDetailUI.Hide();
        }
    }

    public void Highlight(bool on)
    {
        var img = artworkImage != null ? artworkImage : GetComponentInChildren<Image>();
        if (img != null)
        {
            img.color = on ? new Color(1f, 1f, 0.5f, 1f) : Color.white;
        }
    }
}