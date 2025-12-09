using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccusationCharacterButton : MonoBehaviour
{
    [Header("Character Info")]
    public string suspectID;              // "Richard", "Anna", etc.
    public string requiredEvidenceCardID; // card needed to accuse
    public Image portraitImage;
    public Button button;

    private AccusationManager manager;

    private Color normalColor = Color.white;
    private Color disabledColor = Color.gray;
    private Color selectedColor = new Color(1f, 0.8f, 0.8f);

    public void Init(AccusationManager m, bool hasEvidence)
    {
        manager = m;

        if (!hasEvidence)
        {
            portraitImage.color = disabledColor;
            button.interactable = false;
        }
        else
        {
            portraitImage.color = normalColor;
            button.interactable = true;
        }
    }

    public void OnClick()
    {
        manager.SelectSuspect(this);
    }

    public void SetSelected(bool selected)
    {
        portraitImage.color = selected ? selectedColor : normalColor;
    }
}
