using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDetailUI : MonoBehaviour
{
    [Header("UI References")]
    public Image artworkImage;
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    public void ShowDetails(CardSY card)
    {
        if (card == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (artworkImage != null)
            artworkImage.sprite = card.artwork;

        if (titleText != null)
            titleText.text = card.displayName;

        if (descriptionText != null)
            descriptionText.text = card.description;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
