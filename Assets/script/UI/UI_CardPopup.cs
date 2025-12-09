using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CardPopup : MonoBehaviour
{
    public static UI_CardPopup Instance;
    public GameObject popupRoot;

    public Image cardArtwork;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI hintText;

    private bool isOpen = false;
    private Queue<CardSY> cardQueue = new Queue<CardSY>();

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            ShowNextOrClose();
        }
    }

    public void ShowCards(IEnumerable<CardSY> cards)
    {
        cardQueue.Clear();

        foreach (var c in cards)
            cardQueue.Enqueue(c);

        if (cardQueue.Count > 0)
        {
            popupRoot.SetActive(true);
            isOpen = true;
            ShowCard(cardQueue.Dequeue());
        }
        GamePauseManager.Instance?.RequestPause("CardPopup");
    }

    private void ShowCard(CardSY card)
    {
        cardArtwork.sprite = card.artwork;
        cardName.text = card.displayName;

        hintText.text = "Press C to continue";
    }

    private void ShowNextOrClose()
    {
        if (cardQueue.Count > 0)
        {
            ShowCard(cardQueue.Dequeue());
        }
        else
        {
            ClosePopup();
        }
    }

    void ClosePopup()
    {
        popupRoot.SetActive(false);
        isOpen = false;
        cardQueue.Clear();
        GamePauseManager.Instance?.RequestResume("CardPopup");
    }
}
