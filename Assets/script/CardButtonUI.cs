using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardButtonUI : MonoBehaviour
{
    public Image icon;
    public Button button;
    private CardSY card;
    private ReasoningBoardUI board;

    public void Setup(CardSY c, ReasoningBoardUI b)
    {
        card = c;
        board = b;
        icon.sprite = c.artwork;
        button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        board.SelectCard(card);
    }
}
