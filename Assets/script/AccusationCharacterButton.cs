using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccusationCharacterButton : MonoBehaviour
{
    [Header("Character Info")]
    public string suspectID;              // "Richard", "Anna", etc.
    public List<string> requiredEvidenceCardIDs; // card needed to accuse
    public Image portraitImage;
    public Button button;

    private AccusationManager manager;

    private Color normalColor = Color.white;
    private Color disabledColor = Color.gray;
    private Color selectedColor = new Color(1f, 0.8f, 0.8f);

    public void Init(AccusationManager m, CardInventory inventory)
    {
        manager = m;

        bool hasEvidence = inventory.HasAnyCard(requiredEvidenceCardIDs);

        portraitImage.color = hasEvidence ? normalColor : disabledColor;
        button.interactable = hasEvidence;
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
